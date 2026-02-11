using UnityEngine;
using UnityEngine.UIElements;

public class ChefNpcController : NPCBaseController
{
    [Header("InitialDialogue State")]
    [SerializeField] private Transform initialDialoguePosition;
    [TextArea(3, 5)]
    [SerializeField] private string dialogueText;

    protected override void SetupStateMachine()
    {
        InitialDialogueState initialDialogueState = new InitialDialogueState(this, initialDialoguePosition);
        StunState stunState = new StunState(this, 5f, true);

        stateMachine.AddState(NPCState.GoToDialogue, initialDialogueState);
        stateMachine.AddState(NPCState.Stun, stunState);

        stateMachine.ChangeState(NPCState.GoToDialogue);
    }

    public override void ShowInitialDialogue()
    {
        if(TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.StartDialogue(dialogueText);
        }
    }

    public override void CloseInitialDialogue()
    {
        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.CloseDialogue();
        }

        characterView.SetBoolAnimation(Constants.ANIM_SCARRY, true);
        stateMachine.ChangeState(NPCState.Stun);
    }
}
