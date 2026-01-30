using UnityEngine;

[CreateAssetMenu(fileName = "SettingsModel", menuName = "Settings/SettingsModel")]
public class SettingsModel : ScriptableObject
{
    [SerializeField] private AudioSettingsModel audioSettings;
    [SerializeField] private GraphicSettingsModel graphicSettings;
    [SerializeField] private ControlSettingsModel controlSettings;
    [SerializeField] private GeneralSettingsModel generalSettings;

    public AudioSettingsModel AudioSettings { get { return audioSettings; } }

    public GraphicSettingsModel GraphicSettings { get { return graphicSettings; } }

    public ControlSettingsModel ControlSettings { get { return controlSettings; } }

    public GeneralSettingsModel GeneralSettings { get { return generalSettings; } }

    public void LoadSettings()
    {
        audioSettings.LoadSettings();
        graphicSettings.LoadSettings();
        controlSettings.LoadSettings();
        generalSettings.LoadSettings();
    }

    [ContextMenu("Save Configuration")]
    public void SaveSettings()
    {
        audioSettings.SaveSettings();
        graphicSettings.SaveSettings();
        controlSettings.SaveSettings();
        generalSettings.SaveSettings();

        Debug.Log("Settings Saved");
    }
}
