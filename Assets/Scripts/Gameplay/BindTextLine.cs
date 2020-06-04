using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BindTextLine : MonoBehaviour
{
    TextMeshProUGUI text;
    public DialoguePopUpLine line;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = line.line;
    }
}
