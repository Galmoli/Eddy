using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelperDialogueController : MonoBehaviour
{
    private static HelperDialogueController instance;

    public static HelperDialogueController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<HelperDialogueController>();
            return instance;
        }
    }


    public DialoguePopUpLine[] titles;
    public DialoguePopUpLine[] descriptions;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public GameObject helperDialogueMenu;

    int currentIndex;

    private void Awake()
    {
        
    }

    public void ShowHelper(int index)
    {
        HideHelper(currentIndex);
        helperDialogueMenu.SetActive(true);

        title.text = titles[index].line;
        description.text = descriptions[index].line;

        currentIndex = index;
    }

    public void HideHelper(int index)
    {
        if (currentIndex == index)
            helperDialogueMenu.SetActive(false);
    }
  
}
