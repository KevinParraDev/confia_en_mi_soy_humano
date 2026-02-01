using System;
using UnityEngine;

public class AlarmBarController : MonoBehaviour
{
    public static Action onPanicAlarm;

    private int panicLevel = 0;

    [SerializeField]
    private float decreaseRatio = 1.0f;
    private float timer = 0.0f;
    private bool inGame = false;

    private AlarmBarView view;

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

    }

    public void Conclude()
    {
        NPCBaseController.onSuspiciosAction -= IncreaseAlarmBar;
    }

    public void IncreaseAlarmBar(int increaseAmount, SuspicionType type)
    {
        panicLevel = Mathf.Min(increaseAmount + panicLevel, 100);

        // Update Bar View
        view.UpdateAlarmBar(panicLevel, 100);
        if(panicLevel == 100)
        {
            onPanicAlarm?.Invoke();
        }
    }

    public void Update()
    {
        if (inGame)
        {
            if (panicLevel > 0)
            {
                timer += Time.deltaTime;
                if (timer > decreaseRatio)
                {
                    //panicLevel = Mathf.Max(0, panicLevel - 1);

                    // Update Bar View
                    view.UpdateAlarmBar(panicLevel, 100);
                    Debug.Log(panicLevel);
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