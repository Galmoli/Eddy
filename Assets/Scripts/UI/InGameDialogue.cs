using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameDialogue : MonoBehaviour
{
    [Serializable]
    public struct DialoguePopUpStruct
    {
        public string name;
        public DialoguePopUp  dialoguePopUp;
        public Transform target;
    }

    private static InGameDialogue instance;
    public static InGameDialogue Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<InGameDialogue>();
            return instance;
        }
    }
    
    public static Action<string> OnDialogueDisabled = delegate(string s) {  };
    
    [SerializeField] private Image dialogueImage;
    [SerializeField] private float timeToAdd;
    [SerializeField] private GameObject rPointer;
    [SerializeField] private GameObject lPointer;
    [SerializeField] private Vector2 offsetDialogueLeft;
    [SerializeField] private Vector2 offsetDialogueRight;
    public DialoguePopUpStruct[] inGameDialogues;
    private DialoguePopUpStruct _currentDialogue;
    private TextMeshProUGUI text;
    private Canvas _canvas;
    private Camera _mainCamera;
    private Transform _player;

    private void Awake()
    {
        text = dialogueImage.GetComponentInChildren<TextMeshProUGUI>();
        _canvas = GetComponent<Canvas>();
        _player = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        _mainCamera = UIManager.Instance.mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueImage.gameObject.activeSelf) DialoguePosition(true);
    }

    public void EnableDialogue(string id)
    {
        _currentDialogue = inGameDialogues.First(d => d.dialoguePopUp.id == id);
        dialogueImage.gameObject.SetActive(true);
        DialoguePosition(false);
        if(_currentDialogue.dialoguePopUp.playerWalk) UIManager.Instance.popUpEnabled = true;
        StartCoroutine(AnimatedText(_currentDialogue));
    }

    private void DisableDialogue()
    {
        OnDialogueDisabled(_currentDialogue.dialoguePopUp.id);
        UIManager.Instance.popUpEnabled = false;
        dialogueImage.gameObject.SetActive(false);
    }
    
    private IEnumerator AnimatedText(DialoguePopUpStruct d)
    {
        var line = new StringBuilder();
        foreach (var l in d.dialoguePopUp.lines)
        {
            if (d.dialoguePopUp.instantText)
            {
                float timeToWait = 0f;
                text.text = l.line;
                var cs = l.line.ToCharArray();
                foreach (var c in cs)
                {
                    if (c == ' ') timeToWait += timeToAdd;
                }
                yield return new WaitForSeconds(timeToWait);
            }
            else
            {
                line = new StringBuilder();
                var c = l.line.ToCharArray();
                for (int i= 0; i < c.Length; i++)
                {
                    if (c[i] == '<')
                    {
                        while (c[i] != '>')
                        {
                            line.Append(c[i]);
                            i++;
                            yield return null;
                        }
                    }
                
                    line.Append(c[i]);
                    text.text = line.ToString();
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.9f);
            }
        }
        if (!d.dialoguePopUp.instantText) yield return new WaitForSeconds(5f);
        DisableDialogue();
    }

    private void DialoguePosition(bool lerp)
    {
        float minX = dialogueImage.GetPixelAdjustedRect().width * _canvas.scaleFactor / 2;
        float maxX = Screen.width - minX;
        
        float minY = dialogueImage.GetPixelAdjustedRect().height  * _canvas.scaleFactor / 2;
        float maxY = Screen.height - minY;
        
        Vector2 pos = _mainCamera.WorldToScreenPoint(_currentDialogue.target.position);
        Vector2 playerPos = _mainCamera.WorldToScreenPoint(_player.position);

        if (playerPos.x > pos.x)
        {
            pos += offsetDialogueLeft * _canvas.scaleFactor;
            rPointer.SetActive(false);
            lPointer.SetActive(true);
            text.alignment = TextAlignmentOptions.BottomLeft;
        }
        else
        {
            pos += offsetDialogueRight * _canvas.scaleFactor;
            rPointer.SetActive(true);
            lPointer.SetActive(false);
            text.alignment = TextAlignmentOptions.BottomRight;
        }

        if (Vector3.Dot(_currentDialogue.target.position - _mainCamera.transform.position, _mainCamera.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }
        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        if(lerp) dialogueImage.transform.position = Vector3.Lerp(dialogueImage.transform.position, pos, 0.25f);
        else dialogueImage.transform.position = pos;
    }
}
