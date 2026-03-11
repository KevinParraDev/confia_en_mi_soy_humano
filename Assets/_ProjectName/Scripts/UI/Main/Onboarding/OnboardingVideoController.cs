using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OnboardingVideoController : MonoBehaviour
{
    public event Action Ended;

    [Header("Input")]
    [SerializeField] private InputAction skipAction;

    [Header("Skip UI")]
    [SerializeField] private RectTransform skipRoot;
    [SerializeField] private Image chargeSkipIMG;
    [SerializeField] private float skipHoldTime = 2f;
    [SerializeField] private float skipScaleAnimTime = 0.2f;

    private float skipTimer;
    private bool skipping;
    private Tween skipTween;

    private void Awake()
    {
        chargeSkipIMG.fillAmount = 0f;
        skipRoot.localScale = Vector3.zero;
    }

    public void Initialize()
    {
        AddListeners();
    }

    private void Update()
    {
        if (!skipping) return;

        skipTimer += Time.deltaTime;
        chargeSkipIMG.fillAmount = skipTimer / skipHoldTime;

        if (skipTimer >= skipHoldTime)
        {
            skipping = false;
            Ended?.Invoke();
        }
    }

    private void AddListeners()
    {
        skipAction.Enable();

        skipAction.started += OnSkipStarted;
        skipAction.canceled += OnSkipCanceled;
    }
    private void OnSkipStarted(InputAction.CallbackContext context)
    {
        skipTimer = 0f;
        skipping = true;
        chargeSkipIMG.fillAmount = 0f;

        skipTween?.Kill();
        skipTween = skipRoot
            .DOScale(1f, skipScaleAnimTime)
            .SetEase(Ease.OutBack);
    }

    private void OnSkipCanceled(InputAction.CallbackContext context)
    {
        skipping = false;
        skipTimer = 0f;
        chargeSkipIMG.fillAmount = 0f;

        skipTween?.Kill();
        skipTween = skipRoot
            .DOScale(0f, skipScaleAnimTime)
            .SetEase(Ease.InBack);
    }

    private void RemoveListeners()
    {
        skipAction.started -= OnSkipStarted;
        skipAction.canceled -= OnSkipCanceled;
    }

    public void Conclude()
    {
        skipAction.Disable();
        RemoveListeners();
    }
}
