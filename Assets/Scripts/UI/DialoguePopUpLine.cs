using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "Dialogue Line")]
public class DialoguePopUpLine : ScriptableObject
{
    [TextArea] public string line;
}
