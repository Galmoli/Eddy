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
        Talk,
        Read,
        Pick,
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
    [SerializeField] private GameObject ButtonTalk;
    [SerializeField] private GameObject ButtonRead;
    [SerializeField] private GameObject ButtonPick;

    [HideInInspector] public HelperAction actionToComplete;

    private Transform parent = null;
    private Vector3 parentOffset;

    private void Update()
    {
        if (parent)
            transform.position = parent.transform.position + parentOffset;
        transform.forward = Camera.main.transform.forward;
    }

    public void EnableHelper(HelperAction action, Vector3 anchor)
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
            case HelperAction.Talk:
                ButtonTalk.SetActive(true);
                break;
            case HelperAction.Read:
                ButtonRead.SetActive(true);
                break;
            case HelperAction.Pick:
                ButtonPick.SetActive(true);
                break;
        }
        helpersParent.transform.position = anchor;
    }

    public void EnableHelper(HelperAction action, Vector3 anchor, Transform parent)
    {
        EnableHelper(action, anchor);
        this.parent = parent;
        parentOffset = transform.position - parent.position;
    }

    public void DisableHelper()
    {
        parent = null;
        background.SetActive(false);
        actionToComplete = HelperAction.None;
        
        ButtonA.SetActive(false);
        ButtonB.SetActive(false);
        ButtonX.SetActive(false);
        ButtonY.SetActive(false);
        ButtonR1.SetActive(false);
        Joystcik.SetActive(false);
        ButtonTalk.SetActive(false);
        ButtonRead.SetActive(false);
        ButtonPick.SetActive(false);
    }

    public void DisableHelper(float time)
    {
        StartCoroutine(DisableHelperTimed(time));
    }

    IEnumerator DisableHelperTimed(float time)
    {
        yield return new WaitForSeconds(time);
        DisableHelper();
    }

    public void DisableHelper(HelperAction action)
    {
        switch (action)
        {
            case HelperAction.Jump:
                ButtonA.SetActive(false);
                break;
            case HelperAction.Drag:
                ButtonB.SetActive(false);
                break;
            case HelperAction.NailSword:
                ButtonY.SetActive(false);
                break;
            case HelperAction.Attack:
                ButtonX.SetActive(false);
                break;
            case HelperAction.Scanner:
                ButtonR1.SetActive(false);
                break;
            case HelperAction.Move:
                Joystcik.SetActive(false);
                break;
            case HelperAction.Talk:
                ButtonTalk.SetActive(false);
                break;
            case HelperAction.Read:
                ButtonRead.SetActive(false);
                break;
            case HelperAction.Pick:
                ButtonPick.SetActive(false);
                break;
        }
    }
}
