using UnityEngine;

public class GraphicSettingsController : UIControllerBase, ISettingsController
{
    private GraphicSettingsModel graphicSettingsModel;

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is SettingsModel model)
        {
            graphicSettingsModel = model.GraphicSettings;

            if (views.Length > 0)
                views[0].Initialize(graphicSettingsModel, this);
        }
        else
        {
            Debug.LogWarning("Initialize: The first parameter is not a valid GraphicSettingsModel.");
        }
    }

    public void Save()
    {
        graphicSettingsModel.SaveSettings();
        HideAllView();
    }

    public void Discard()
    {
        graphicSettingsModel.LoadSettings();
    }

    public void Back()
    {
        graphicSettingsModel.LoadSettings();
        HideAllView();
    }

    public override void Conclude()
    {
        HideAllView();

        if (views.Length > 0)
            views[0].Conclude();
    }
}
