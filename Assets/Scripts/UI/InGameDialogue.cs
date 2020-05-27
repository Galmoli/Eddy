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
    public struct DialoguePopUp
    {
        public string name;
        public global::DialoguePopUp  dialoguePopUp;
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
    [SerializeField] private Image dialogueImage;
    [SerializeField] private Image pointer;
    [SerializeField] private float timeToAdd;
    [SerializeField] private DialoguePopUp[] inGameDialogues;
    private DialoguePopUp _currentDialogue;
    private TextMeshProUGUI text;
    private Canvas _canvas;
    private Camera _mainCamera;

    private void Awake()
    {
        text = dialogueImage.GetComponentInChildren<TextMeshProUGUI>();
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _mainCamera = UIManager.Instance.mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) EnableDialogue("PopUp_1");
        
        if (!dialogueImage.gameObject.activeSelf) return;
            
        float minX = dialogueImage.GetPixelAdjustedRect().width * _canvas.scaleFactor / 2;
        float maxX = Screen.width - minX;
        float pminX = pointer.GetPixelAdjustedRect().width * _canvas.scaleFactor / 2;
        float pmaxX = Screen.width - pminX;
        
        float minY = dialogueImage.GetPixelAdjustedRect().height  * _canvas.scaleFactor / 2;
        float maxY = Screen.height - minY;
        float pminY = pointer.GetPixelAdjustedRect().height  * _canvas.scaleFactor / 2;
        float pmaxY = Screen.height - pminY;

        Vector3 cameraProjectedForward = Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up);
        
        Vector2 pos = _mainCamera.WorldToScreenPoint(_currentDialogue.target.position + cameraProjectedForward * 12);

        Vector2 pointerDir = ((Vector2)_mainCamera.WorldToScreenPoint(_currentDialogue.target.position) - pos).normalized;
        Vector2 pointerPos = GetPointerPos(pointerDir, minX, minY);
        pointerPos += pos;
        
        Vector2 pointerAngleV2 = ((Vector2) _mainCamera.WorldToScreenPoint(_currentDialogue.target.position) - pointerPos).normalized;
        

        var angle = Mathf.Atan2(pointerAngleV2.y, pointerAngleV2.x) * Mathf.Rad2Deg + 90;
        pointer.transform.rotation = Quaternion.AngleAxis(angle, pointer.transform.forward);

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
            if (pointerPos.x < Screen.width / 2)
            {
                pointerPos.x = pmaxX;
            }
            else
            {
                pointerPos.x = pminX;
            }
        }
        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pointerPos.x = Mathf.Clamp(pointerPos.x, pminX, pmaxX);
        pointerPos.y = Mathf.Clamp(pointerPos.y, pminY, pmaxY);

        dialogueImage.transform.position = Vector3.Lerp(dialogueImage.transform.position, pos, 0.1f);
        pointer.transform.position = Vector3.Lerp(pointer.transform.position, pointerPos, 0.1f);
    }

    public void EnableDialogue(string id)
    {
        _currentDialogue = inGameDialogues.First(d => d.dialoguePopUp.id == id);
        dialogueImage.gameObject.SetActive(true);
        if(_currentDialogue.dialoguePopUp.playerWalk) UIManager.Instance.popUpEnabled = true;
        StartCoroutine(AnimatedText(_currentDialogue));
    }

    private void DisableDialogue()
    {
        UIManager.Instance.popUpEnabled = false;
        dialogueImage.gameObject.SetActive(false);
    }

    private Vector2 GetPointerPos(Vector2 pointerDir, float sizeX, float sizeY)
    {
        Vector2 pointerPos;
        if (Mathf.Abs(pointerDir.x) >= Mathf.Abs(pointerDir.y))
        {
            if (pointerDir.x >= 0) pointerPos.x = sizeX;
            else pointerPos.x = -sizeX;
            
            pointerPos.y = pointerDir.y * sizeY;
        }
        else
        {
            if (pointerDir.y >= 0) pointerPos.y = sizeY;
            else pointerPos.y = -sizeY;

            pointerPos.x = pointerDir.x * sizeX;
        }

        return pointerPos;
    }
    
    private IEnumerator AnimatedText(DialoguePopUp d)
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
}
