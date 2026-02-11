using UnityEngine;

public class InitialDialogueState : IState
{
    NPCBaseController npcController;
    Transform dialogueStartPosition;
    bool isDialogueShown = false;
    float dialogueDuration = 3f;

    float dialogueTimer = 0f;

    public InitialDialogueState(NPCBaseController npcController, Transform dialogueStartPosition)
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
                npcController.CloseInitialDialogue();
            }

            return;
        }

        if (npcController.HasReachedDestination() && !isDialogueShown)
        {
            npcController.StopMovement();
            npcController.ShowInitialDialogue();
            isDialogueShown = true;

            return;
        }

        npcController.SetNewDestination(dialogueStartPosition.position);
    } 

    public void Exit()
    {
    }
}
