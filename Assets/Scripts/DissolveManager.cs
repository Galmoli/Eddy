using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    private TestSwordFeature _swordFeature;
    public MeshRenderer mr;

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
        print("Active");
        mr.sharedMaterial.SetFloat("rSword",4);
        mr.sharedMaterial.SetVector("swordPos",_swordFeature.transform.position);
    }

    private void DisableSword()
    {
        mr.sharedMaterial.SetFloat("rSword",0);
    }
}
