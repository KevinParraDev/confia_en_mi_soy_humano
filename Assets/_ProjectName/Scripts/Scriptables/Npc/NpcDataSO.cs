using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDataSO", menuName = "NPC/NpcDataSO")]
public class NpcDataSO : ScriptableObject
{
    public NPC npcType;
    public RuntimeAnimatorController animatorSkin;
    public List<NpcDialog> dialogs;

    public string GetDialogForNpc(NPC npc)
    {
        var dialogData = dialogs.Find(d => d.npcToResponse == npc);

        if (dialogData == null || dialogData.dialogs == null || dialogData.dialogs.Count == 0)
        {
            Debug.LogWarning($"No hay diálogos para el NPC {npc} en {name}");
            return "...";
        }

        int index = UnityEngine.Random.Range(0, dialogData.dialogs.Count);
        return dialogData.dialogs[index];
    }
}

[Serializable]
public class NpcDialog
{
    public NPC npcToResponse;
    public List<string> dialogs;
}
public enum NPC
{
    None,
    Alien,
    Chef,
    Waiter,
    Guard,
    Bureaucrat,
    Trun
}
