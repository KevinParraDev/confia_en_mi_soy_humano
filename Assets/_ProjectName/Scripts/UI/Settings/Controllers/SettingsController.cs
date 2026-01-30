using System;
using System.Linq;

public class SettingsController : UIControllerBase
{
    private ISettingManager settingManager;

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is ISettingManager manager)
        {
            settingManager = manager;
        }

        if (views.Length > 0)
            views[0].Initialize(settingManager);
    }

    public void SetSettingsButton(SettingsTypes[] activeSettings, Action[] actions)
    {
        if (views.Length == 0 || !(views[0] is SettingsView view)) return;

        var settingsPairs = activeSettings.Zip(actions, (setting, action) => new { setting, action });

        foreach (var button in view.GetSettingsButton())
        {
            var match = settingsPairs.FirstOrDefault(pair => pair.setting == button.SettingType);

            if (match != null)
            {
                button.Button.gameObject.SetActive(true); 
                button.Button.onClick.RemoveAllListeners();
                button.Button.onClick.AddListener(() => match.action?.Invoke());
            }
            else
            {
                button.Button.gameObject.SetActive(false);
            }
        }
    }

    public override void Conclude()
    {
        HideAllView();

        if (views.Length > 0)
            views[0].Conclude();
    }
}
