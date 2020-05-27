using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelperController : MonoBehaviour
{
    private static UIHelperController instance;
    public static UIHelperController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIHelperController>();
            return instance;
        }
    }

    public enum HelperAction
    {
        Move,
        Jump,
        Attack,
        NailSword,
        Drag,
        Scanner,
        None
    }

    [SerializeField] private GameObject helpersParent;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject ButtonA;
    [SerializeField] private GameObject ButtonB;
    [SerializeField] private GameObject ButtonY;
    [SerializeField] private GameObject ButtonX;
    [SerializeField] private GameObject ButtonR1;
    [SerializeField] private GameObject Joystcik;
    
    [HideInInspector] public HelperAction actionToComplete;

    public void EnableHelper(HelperAction action, Transform anchor)
    {
        background.SetActive(true);
        actionToComplete = action;
        switch (action)
        {
            case HelperAction.Jump:
                ButtonA.SetActive(true);
                break;
            case HelperAction.Drag:
                ButtonB.SetActive(true);
                break;
            case HelperAction.NailSword:
                ButtonY.SetActive(true);
                break;
            case HelperAction.Attack:
                ButtonX.SetActive(true);
                break;
            case HelperAction.Scanner:
                ButtonR1.SetActive(true);
                break;
            case HelperAction.Move:
                Joystcik.SetActive(true);
                break;
        }
        helpersParent.transform.position = UIManager.Instance.mainCamera.WorldToScreenPoint(anchor.position);
    }

    public void DisableHelper()
    {
        background.SetActive(false);
        actionToComplete = HelperAction.None;
        
        ButtonA.SetActive(false);
        ButtonB.SetActive(false);
        ButtonX.SetActive(false);
        ButtonY.SetActive(false);
        ButtonR1.SetActive(false);
        Joystcik.SetActive(false);
    }
}
