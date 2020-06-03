using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New General Dialogue", menuName = "General Dialogue")]
public class GeneralDialoguePopUp : ScriptableObject
{
    public string speaker;
    public Sprite speakerImage;
    public Color speakerColor;
    public DialoguePopUpLine[] lines;
}
