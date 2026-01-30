using UnityEngine;

public class AudioSettingsController : UIControllerBase, ISettingsController
{
    private AudioSettingsModel audioSettingsModel;

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is SettingsModel model)
        {
            audioSettingsModel = model.AudioSettings;

            if (views.Length > 0)
                views[0].Initialize(audioSettingsModel, this);
        }
        else
        {
            Debug.LogWarning("Initialize: The first parameter is not a valid AudioSettingsModel.");
        }
    }

    public void Save()
    {
        audioSettingsModel.SaveSettings();
        HideAllView();
    }

    public void Discard()
    {
        audioSettingsModel.LoadSettings();
    }

    public void Back()
    {
        audioSettingsModel.LoadSettings();
        HideAllView();
    }

    public override void Conclude()
    {
        HideAllView();

        if (views.Length > 0)
            views[0].Conclude();
    }
}
