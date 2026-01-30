using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : UIViewBase
{
    [SerializeField] private SettingsButtons[] settingsButtons;
    [SerializeField] private Button backButton;

    private ISettingManager settingManager;

    protected override void Awake()
    {
        id = UI.Settings;
    }

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is ISettingManager manager)
        {
            settingManager = manager;
        }

        AddListeners();
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        backButton.onClick.AddListener(SetClose);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        backButton.onClick.RemoveAllListeners();
    }

    private void SetClose()
    {
        Debug.Log("Closed");

        settingManager.Close();
    }

    public SettingsButtons[] GetSettingsButton()
    {
        return settingsButtons;
    }

    public override void Conclude()
    {
        RemoveListeners();
    }

    [Serializable]
    public class SettingsButtons
    {
        public SettingsTypes SettingType;
        public Button Button;
    }
}
