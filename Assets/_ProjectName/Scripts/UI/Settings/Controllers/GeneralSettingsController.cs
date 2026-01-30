using UnityEngine;

public class GeneralSettingsController : UIControllerBase, ISettingsController
{
    private GeneralSettingsModel generalSettingsModel;

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is SettingsModel model)
        {
            generalSettingsModel = model.GeneralSettings;

            if (views.Length > 0)
                views[0].Initialize(generalSettingsModel, this);
        }
        else
        {
            Debug.LogWarning("Initialize: The first parameter is not a valid GraphicSettingsModel.");
        }
    }

    public void Save()
    {
        generalSettingsModel.SaveSettings();
        HideAllView();
    }

    public void Discard()
    {
        generalSettingsModel.LoadSettings();
    }

    public void Back()
    {
        generalSettingsModel.LoadSettings();
        HideAllView();
    }

    public override void Conclude()
    {
        HideAllView();

        if (views.Length > 0)
            views[0].Conclude();
    }
}
