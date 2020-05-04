using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    private SwordProgressiveColliders _swordProgressiveColliders;
    public MeshRenderer[] mr;

    private void Awake()
    {
        _swordProgressiveColliders = FindObjectOfType<SwordProgressiveColliders>();
    }
    private void Update()
    {
        if(_swordProgressiveColliders.swordActive) SetSwordPos();
        else DisableSword();
    }

    private void SetSwordPos()
    {
        foreach (var m in mr)
        {
            m.sharedMaterial.SetFloat("rSword",4);
            m.sharedMaterial.SetVector("swordPos",_swordProgressiveColliders.transform.position);
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
