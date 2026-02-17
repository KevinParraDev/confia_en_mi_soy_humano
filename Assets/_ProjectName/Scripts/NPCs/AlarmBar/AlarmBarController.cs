using System;
using UnityEngine;

public class AlarmBarController : MonoBehaviour
{
    public static Action onPanicAlarm;
    public static Action onAlarmStopped;

    private int panicLevel = 0;

    [SerializeField]
    private bool inGame = false;

    private bool isInAlarm = false;

    [SerializeField]
    private float alarmDuration = 10f;

    private float alarmTimer = 0f;

    private AlarmBarView view;

    public int currentGuardsChasing = 0;

    public void Initialize()
    {
        panicLevel = 0;
        inGame = true;

        view = GetComponentInChildren<AlarmBarView>();
        if(view == null)
        {
            Debug.LogWarning("View Alarm not Set");
        }
        view.Initialize();
        view.UpdateAlarmBar(panicLevel, 100);

        // Suspucios Actions from NPCS
        NPCBaseController.onSuspiciosAction += IncreaseAlarmBar;
        GuardNPCController.onGuardChase += IncreaseGuardsChasing;
        GuardNPCController.onGuardStopChase += DecreaseGuardsChasing;
    }

    public void Conclude()
    {
        NPCBaseController.onSuspiciosAction -= IncreaseAlarmBar;
        GuardNPCController.onGuardChase -= IncreaseGuardsChasing;
        GuardNPCController.onGuardStopChase -= DecreaseGuardsChasing;
    }

    private void IncreaseGuardsChasing()
    {
        currentGuardsChasing++;
    }

    private void DecreaseGuardsChasing()
    {
        currentGuardsChasing--;
    }

    public void IncreaseAlarmBar(int increaseAmount, SuspicionType type)
    {
        panicLevel = Mathf.Min(increaseAmount + panicLevel, 100);

        // Update Bar View
        view.IncreaseAlarmBar(panicLevel, 100, type);
        if(panicLevel == 100 && !isInAlarm)
        {
            SoundManager.Instance.PlaySFXByName(Constants.SFX_ALARM);
            onPanicAlarm?.Invoke();
            isInAlarm = true;
        }
    }

    public void Update()
    {
        if (inGame)
        {
            if (isInAlarm)
            {
                alarmTimer += Time.deltaTime;
                if (alarmTimer >= alarmDuration)
                {
                    if (currentGuardsChasing == 0)
                    {
                        isInAlarm = false;
                        onAlarmStopped?.Invoke();
                        panicLevel = 0;
                        view.UpdateAlarmBar(panicLevel, 100);
                        alarmTimer = 0f;
                    }
                }
            }
        }
    }
}

public enum SuspicionType
{
    WrongZone,
    FoodLack,
    WrongInteraction,
    VisibleTransformation,
}