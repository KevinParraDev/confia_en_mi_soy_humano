using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettingsView : SettingsBaseView
{
    [Header("Configuration Buttons")]
    [SerializeField] private TMP_Dropdown languageDropdown;

    private GeneralSettingsModel _model;

    protected override void Awake()
    {
        id = UI.Settings_General;
    }

    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);

        foreach (var parameter in parameters)
        {
            if (parameter is GeneralSettingsModel model)
            {
                _model = model;

                break;
            }
        }

        PopulateLanguageDropdown();

        SetUIComponentsValues(_model);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        languageDropdown.onValueChanged.AddListener(SetLanguage);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        languageDropdown.onValueChanged.RemoveAllListeners();
    }

    private void SetUIComponentsValues(GeneralSettingsModel model)
    {
        languageDropdown.value = model.SelectedLanguageIndex;
    }

    private void PopulateLanguageDropdown()
    {
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(_model.AvailableLanguages.ToList());
    }

    private void SetLanguage(int index)
    {
        _model.SelectedLanguageIndex = index;
    }

    protected override void SetDiscard()
    {
        base.SetDiscard();

        SetUIComponentsValues(_model);
    }

    protected override void SetBack()
    {
        base.SetBack();

        SetUIComponentsValues(_model);
    }
}
