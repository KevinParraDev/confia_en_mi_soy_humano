using UnityEngine;

public class ControlsSettingsController : UIControllerBase, ISettingsController
{
    private ControlSettingsModel controlsSettingsModel;

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is SettingsModel model)
        {
            controlsSettingsModel = model.ControlSettings;

            if (views.Length > 0)
                views[0].Initialize(controlsSettingsModel, this);
        }
        else
        {
            Debug.LogWarning("Initialize: The first parameter is not a valid GraphicSettingsModel.");
        }
    }

    public void Save()
    {
        controlsSettingsModel.SaveSettings();
        HideAllView();
    }

    public void Discard()
    {
        controlsSettingsModel.LoadSettings();
    }

    public void Back()
    {
        controlsSettingsModel.LoadSettings();
        HideAllView();
    }

    public override void Conclude()
    {
        HideAllView();

        if (views.Length > 0)
            views[0].Conclude();
    }
}
