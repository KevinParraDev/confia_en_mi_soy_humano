using UnityEngine;

[CreateAssetMenu(fileName = "NpcDataSO", menuName = "NPC/NpcDataSO")]
public class NpcDataSO : ScriptableObject
{
    public NPC npcType;
    public RuntimeAnimatorController animatorSkin;
}

public enum NPC
{
    None,
    Alien,
    Chef,
    Waiter,
    Guard,
    Bureaucrat
}
