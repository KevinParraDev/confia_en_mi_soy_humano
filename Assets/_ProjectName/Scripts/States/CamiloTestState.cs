using UnityEngine;

public class CamiloTestState : GameStateBase
{
    [SerializeField]
    private AlarmBarController alarmController;

    [SerializeField]
    private NPCBaseController[] npcsAgentList;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private DishesDeskInteractable dishDesk;

    protected override void Awake()
    {
        base.Awake();
    }

    private void DefaultValues()
    {

    }

    public void Dependencies()
    {
        DefaultValues();
    }

    public override void EnterState()
    {
        base.EnterState();
        StartGameplay();
    }


    private void StartGameplay()
    {
        alarmController.Initialize();

        foreach (var npc in npcsAgentList)
        {
            npc.Initialize(player);
        }

        

        dishDesk.AddDish();
        dishDesk.AddDish();
        dishDesk.AddDish();
        dishDesk.AddDish();

    }

    private void GameOver()
    {
        alarmController.Conclude();
        foreach (var npc in npcsAgentList)
        {
            npc.Conclude();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
