using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralDialogue : MonoBehaviour
{
    private static GeneralDialogue instance;

    public static GeneralDialogue Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GeneralDialogue>();
            return instance;
        }
    }
    
    public static Action<string> OnDialogueDisabled = delegate(string s) {  };
    
    [SerializeField] private Image dialogueImage;
    [SerializeField] private Conversation[] conversations;
    [SerializeField] private TextMeshProUGUI gdText;
    [SerializeField] private TextMeshProUGUI gdSpeaker;
    [SerializeField] private Image gdSpeakerImage;
    [SerializeField] private Image gdSpeakerBackground;
    private InputActions _input;
    private bool skipDialogue;
    private Conversation _conversation;

    private void Awake()
    {
        _input = new InputActions();
        _input.PlayerControls.SkipDialogue.started += ctx => skipDialogue = true;
    }

    public void EnableDialogue(string id)
    {
        _input.Enable();
        UIManager.Instance.paused = true;
        dialogueImage.gameObject.SetActive(true);
        _conversation = conversations.First(c => c.id == id);
        StartCoroutine(AnimatedText(_conversation));
    }

    public void DisableDialogue()
    {
        OnDialogueDisabled(_conversation.id);
        _input.Disable();
        UIManager.Instance.paused = false;
        dialogueImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) EnableDialogue("Conv");
    }

    private IEnumerator AnimatedText(Conversation conv)
    {
        var line = new StringBuilder();
        foreach (var d in conv.dialogues)
        {
            gdSpeaker.text = d.speaker;
            gdSpeaker.color = d.speakerColor;
            gdSpeakerImage.sprite = d.speakerImage;
            gdSpeakerBackground.color = d.speakerColor;
            
            foreach (var l in d.lines)
            {
                line = new StringBuilder();
                var c = l.line.ToCharArray();
            
                for (int i= 0; i < c.Length; i++)
                {
                    if (!skipDialogue)
                    {
                        if (c[i] == '<') //Rich text
                        {
                            while (c[i] != '>')
                            {
                                line.Append(c[i]);
                                i++;
                                yield return null;
                            }
                        }

                        if (c[i] == ' ') //Don't wait for spaces
                        {
                            while (c[i] == ' ')
                            {
                                line.Append(c[i]);
                                i++;
                                yield return null;
                            }
                        }

                        line.Append(c[i]);
                        gdText.text = line.ToString();
                        yield return new WaitForSeconds(0.03f);
                
                        if ((c[i] == '.' || c[i] == '?' || c[i] == '!') && i < c.Length - 1) //Wait for dots.
                        {
                            if (c[i + 1] != '.') yield return new WaitForSeconds(0.5f);
                        }
                    }
                    else
                    {
                        gdText.text = l.line;
                        i = c.Length - 1;
                        skipDialogue = false;
                    }
                }
            
                while (skipDialogue == false) {
                    yield return null;
                }
                skipDialogue = false;
            }
            if(d == conv.dialogues[conv.dialogues.Length - 1]) DisableDialogue();
        }
    }
}
