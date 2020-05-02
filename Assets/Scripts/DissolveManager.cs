using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    private TestSwordFeature _swordFeature;
    public MeshRenderer[] mr;

    private void Awake()
    {
        _swordFeature = FindObjectOfType<TestSwordFeature>();
    }
    private void Update()
    {
        if(_swordFeature.swordActive) SetSwordPos();
        else DisableSword();
    }

    private void SetSwordPos()
    {
        foreach (var m in mr)
        {
            m.sharedMaterial.SetFloat("rSword",4);
            m.sharedMaterial.SetVector("swordPos",_swordFeature.transform.position);
        }
    }

    private void DisableSword()
    {
        foreach (var m in mr)
        {
            m.sharedMaterial.SetFloat("rSword",0);
        }
    }
}
