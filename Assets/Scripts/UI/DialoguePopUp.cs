using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue PopUp", menuName = "DialoguePopUp")]
public class DialoguePopUp : ScriptableObject
{
    public string id;
    public DialoguePopUpLine[] lines;
    public bool playerWalk;
}
