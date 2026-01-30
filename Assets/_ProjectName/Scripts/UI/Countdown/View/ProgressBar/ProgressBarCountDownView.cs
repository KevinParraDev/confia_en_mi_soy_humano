using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ProgressBarCountDownView : UIViewBase, ICountDownView
{
    [Header("References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image fillImage;

    [Header("Critical Mode")]
    [SerializeField] [Range(0,1)] private float criticalThreshold = 0.1f;
    [SerializeField] private Color criticalColor = Color.red;
    [SerializeField] private float colorTransitionTime = 0.3f;

    private float maxTime;
    private Tween colorTween;
    private Color originalColor;

    protected override void Awake()
    {
        id = UI.ProgressBarCountDown;

        if (progressBar == null)
            progressBar = GetComponent<Slider>();

        if (fillImage == null && progressBar.fillRect != null)
            fillImage = progressBar.fillRect.GetComponent<Image>();

        if (fillImage != null)
            originalColor = fillImage.color;
    }

    public override void Initialize(params object[] parameters)
    {
        maxTime = (float)parameters[0];
        ResetVisuals();
    }

    public void UpdateTimer(float timeRemaining)
    {
        float progress = timeRemaining / maxTime;

        if (progressBar != null)
            progressBar.value = progress;

        if (progress <= criticalThreshold)
        {
            AnimateColor(criticalColor);
        }
        else
        {
            AnimateColor(originalColor);
        }
    }

    private void AnimateColor(Color targetColor)
    {
        if (fillImage == null) return;

        colorTween?.Kill();
        colorTween = fillImage.DOColor(targetColor, colorTransitionTime);
    }

    private void ResetVisuals()
    {
        progressBar.value = 1f;
        if (fillImage != null) fillImage.color = originalColor;
    }

    public override void Conclude()
    {
        if (progressBar != null)
            progressBar.value = 0;

        colorTween?.Kill();
        if (fillImage != null) fillImage.color = originalColor;
    }
}
