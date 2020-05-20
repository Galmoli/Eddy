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
    public static InGameDialogue Intsance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<InGameDialogue>();
            return instance;
        }
    }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private DialoguePopUp[] inGameDialogues;
    private DialoguePopUp _currentDialogue;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = dialogueImage.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) EnableDialogue("tumbaDani");
        
        if (!dialogueImage.gameObject.activeSelf) return;
            
        float minX = dialogueImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        
        float minY = dialogueImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;
        
        Vector2 pos = mainCamera.WorldToScreenPoint(_currentDialogue.target.position);

        if (Vector3.Dot(_currentDialogue.target.position - mainCamera.transform.position, mainCamera.transform.forward) < 0)
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

        dialogueImage.transform.position = pos;
    }

    public void EnableDialogue(string id)
    {
        _currentDialogue = inGameDialogues.First(d => d.dialoguePopUp.id == id);
        dialogueImage.gameObject.SetActive(true);
        StartCoroutine(AnimatedText(_currentDialogue));
    }

    public void DisableDialogue()
    {
        dialogueImage.gameObject.SetActive(false);
    }

    private IEnumerator AnimatedText(DialoguePopUp d)
    {
        var line = new StringBuilder();
        foreach (var l in d.dialoguePopUp.lines)
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
}
