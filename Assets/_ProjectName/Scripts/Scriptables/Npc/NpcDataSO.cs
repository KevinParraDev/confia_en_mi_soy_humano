using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDataSO", menuName = "NPC/NpcDataSO")]
public class NpcDataSO : ScriptableObject
{
    public NPC npcType;
    public RuntimeAnimatorController animatorSkin;
    public List<SkinConversation> conversations;
    public Sprite maskIcon;

    public DialogNode GetStartNode(NPC playerSkin)
    {
        var conversation = conversations.Find(c => c.skinRequired == playerSkin);

        if (conversation == null || conversation.dialogNodes == null || conversation.dialogNodes.Count == 0)
        {
            Debug.LogWarning($"No hay conversación para {playerSkin} en {name}");
            return null;
        }

        return conversation.dialogNodes[0]; // primer nodo como inicio
    }
}

[Serializable]
public class NpcDialog
{
    public NPC npcToResponse;
    public List<string> dialogs;
}

[Serializable]
public class SkinConversation
{
    public NPC skinRequired;
    public List<DialogNode> dialogNodes;
}

[Serializable]
public class DialogNode
{
    public string id;
    public string text;
    public List<DialogOption> options;
    public bool endsConversation;
}

[Serializable]
public class DialogOption
{
    public string text;
    public string nextNodeId;
    public NPC requiredSkin; // opcional
    public int suspicionChange;
    public bool isLie;
}

public enum NPC
{
    None,
    Alien,
    Chef,
    Waiter,
    Guard,
    Bureaucrat_1,
    Trun,
    Bureaucrat_2,
    Bureaucrat_Bety
}
