using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSettingsView : SettingsBaseView
{
    [Header("Configuration Buttons")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private GraphicSettingsModel _model;

    protected override void Awake()
    {
        id = UI.Settings_Graphics;
    }

    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);

        foreach (var parameter in parameters)
        {
            if (parameter is GraphicSettingsModel model)
            {
                _model = model;

                break;
            }
        }

        PopulateResolutionDropdown();

        PopulateQualittyDropdown();

        SetUIComponentsValues(_model);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        qualityDropdown.onValueChanged.AddListener(SetQualitty);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.RemoveAllListeners();
        fullscreenToggle.onValueChanged.RemoveAllListeners();
    }

    private void SetUIComponentsValues(GraphicSettingsModel model)
    {
        resolutionDropdown.value = model.SelectedResolutionIndex;
        qualityDropdown.value = model.SelectedQualityIndex;
        fullscreenToggle.isOn = model.IsFullscreen;
    }

    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(_model.ResolutionNames.ToList());
    }

    private void PopulateQualittyDropdown()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(_model.QualityLevelNames.ToList());
    }

    private void SetResolution(int index)
    {
        _model.SelectedResolutionIndex = index;
    }

    private void SetQualitty(int index)
    {
        _model.SelectedQualityIndex = index;
    }

    private void SetFullscreen(bool isFullscreen)
    {
        _model.IsFullscreen = isFullscreen;
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
