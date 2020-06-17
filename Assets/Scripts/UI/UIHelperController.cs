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
        SpinAttack,
        None
    }

    [System.Serializable]
    public class Helper
    {
        public GameObject helper;
        [HideInInspector]public Transform parent;
        [HideInInspector] public Vector3 parentOffset;
    }

    [SerializeField] private Helper[] helpers;
    [HideInInspector] public List<HelperAction> actionsToComplete = new List<HelperAction>();

    private void Update()
    {
        for (int i = 0; i < actionsToComplete.Count; i++)
        {
            helpers[(int)actionsToComplete[i]].helper.transform.position = helpers[(int)actionsToComplete[i]].parent.position + helpers[(int)actionsToComplete[i]].parentOffset;
            helpers[(int)actionsToComplete[i]].helper.transform.forward = Camera.main.transform.forward;          
        }
    }

    public void EnableHelper(HelperAction action, Vector3 anchor, Transform parent)
    {
        if (helpers[(int)action].parent != parent)
        {
            actionsToComplete.Add(action);
            helpers[(int)action].helper.GetComponent<Animator>().SetTrigger("Enter");
            helpers[(int)action].helper.transform.position = anchor;
            helpers[(int)action].parent = parent;
            helpers[(int)action].parentOffset = helpers[(int)action].helper.transform.position - parent.transform.position;
        }
    }

    public void DisableHelper(float time, HelperAction action)
    {
        StartCoroutine(DisableHelperTimed(time,action));
    }

    IEnumerator DisableHelperTimed(float time, HelperAction action)
    {
        helpers[(int)action].helper.GetComponent<Animator>().SetTrigger("Exit");
        yield return new WaitForSeconds(time);
        actionsToComplete.Remove(action);
        helpers[(int)action].parent = null;
    }

    public void DisableHelper(HelperAction action)
    {
        helpers[(int)action].helper.GetComponent<Animator>().SetTrigger("Exit");
        actionsToComplete.Remove(action);
        helpers[(int)action].parent = null;
    }
}
