using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public float damage;
    public float timeToResetCombo;
    public float chargeTime;
}
