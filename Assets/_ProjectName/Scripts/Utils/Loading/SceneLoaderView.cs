using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoaderView : UIViewBase
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text loaderText;

    protected override void Awake()
    {
        id = UI.Loading;
    }

    public override void Initialize(params object[] parameters)
    {

    }

    public void ProgressUpdate(float progress)
    {
        progressBar.value = progress;
        loaderText.text = ""+progress * 100;
    }

    public override void Conclude()
    {
        
    }
}
