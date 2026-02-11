using UnityEngine;

public class DialogueState : IState
{
    NPCBaseController npcController;
    Transform dialogueStartPosition;
    bool isDialogueShown = false;
    float dialogueDuration = 3f;

    float dialogueTimer = 0f;

    public DialogueState(NPCBaseController npcController, Transform dialogueStartPosition)
    {
        this.npcController = npcController;
        this.dialogueStartPosition = dialogueStartPosition;
    }

    public void Enter()
    {
        npcController.SetNewDestination(dialogueStartPosition.position);
        npcController.ResumeMovement();

        dialogueTimer = 0f;
        isDialogueShown = false;
    }

    public void Execute()
    {
        if (isDialogueShown)
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer >= dialogueDuration)
            {
                npcController.CloseDialogue();
            }

            return;
        }

        if (npcController.HasReachedDestination() && !isDialogueShown)
        {
            npcController.StopMovement();
            npcController.ShowDialogue();
            isDialogueShown = true;

            return;
        }

        npcController.SetNewDestination(dialogueStartPosition.position);
    } 

    public void Exit()
    {
    }
}
