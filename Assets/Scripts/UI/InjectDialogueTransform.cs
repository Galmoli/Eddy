using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InjectDialogueTransform : MonoBehaviour
{
    [SerializeField] private string[] ids;

    private void Start()
    {
        Inject();
    }

    private void Inject()
    {
        foreach (var id in ids)
        {
            for (var j = 0; j < InGameDialogue.Instance.inGameDialogues.Length; j++)
            {
                if (InGameDialogue.Instance.inGameDialogues[j].dialoguePopUp.id  == id)
                {
                    InGameDialogue.Instance.inGameDialogues[j].target = transform;
                }
            }
        }
    }
}
