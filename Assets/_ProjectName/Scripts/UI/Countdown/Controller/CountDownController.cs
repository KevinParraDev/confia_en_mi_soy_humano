using MEC;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CountDownController : UIControllerBase
{
    public Action CountDownCompleted;

    private ICountDownView[] timerViews;
    private CoroutineHandle timerCoroutine;
    private float timerDuration;
    private float currentTime;
    private bool isPaused = false;

    public override void Initialize(params object[] parameters)
    {
        timerDuration = (float)parameters[0];
        currentTime = timerDuration;

        if (views == null || views.Length == 0)
        {
            Debug.LogError("CountDownController: views es null o vacío.");
            return;
        }

        timerViews = views.Where(v => v is ICountDownView).Cast<ICountDownView>().ToArray();

        if (timerViews.Length == 0)
        {
            Debug.LogError("CountDownController: Ninguna vista implementa ICountDownView.");
            return;
        }

        foreach (UIViewBase view in views)
        {
            view.Initialize(timerDuration);
        }

        UpdateViews(timerDuration);
    }

    public void StartTimer()
    {
        Timing.KillCoroutines(timerCoroutine);
        timerCoroutine = Timing.RunCoroutine(TimerCountdown());
    }

    private IEnumerator<float> TimerCountdown()
    {
        while (currentTime > 0f)
        {
            while (isPaused) yield return Timing.WaitForOneFrame;

            yield return Timing.WaitForOneFrame;
            currentTime -= Time.deltaTime;
            UpdateViews(currentTime);
        }

        Conclude();
    }

    private void UpdateViews(float value)
    {
        if (timerViews == null || timerViews.Length == 0)
        {
            Debug.LogError("CountDownController: timerViews es null o vacío al intentar actualizar.");
            return;
        }

        foreach (ICountDownView timerView in timerViews)
        {
            timerView.UpdateTimer(value);
        }
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        timerDuration += timeToAdd;
        UpdateViews(currentTime);
    }

    public void SubtractTime(float timeToSubtract)
    {
        currentTime = Mathf.Max(0, currentTime - timeToSubtract);
        timerDuration = Mathf.Max(0, timerDuration - timeToSubtract);
        UpdateViews(currentTime);

        if (currentTime == 0)
        {
            Conclude();
        }
    }

    public void PauseTimer()
    {
        isPaused = !isPaused;
    }

    public void StopTimer()
    {
        Timing.KillCoroutines(timerCoroutine);

        UpdateViews(timerDuration);
    }

    public override void Conclude()
    {
        UpdateViews(0.0f);

        Timing.KillCoroutines(timerCoroutine);

        if (views != null && views.Length > 0)
        {
            foreach (UIViewBase view in views)
            {
                view.Conclude();
            }
        }

        CountDownCompleted?.Invoke();
    }
}
