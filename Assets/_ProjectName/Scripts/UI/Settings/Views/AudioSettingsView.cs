using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsView : SettingsBaseView
{
    [Header("Configuration Buttons")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Toggle muteMusicToggle;
    [SerializeField] private Toggle muteEffectsToggle;

    private AudioSettingsModel _model;

    protected override void Awake()
    {
        id = UI.Settings_Audio;
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        musicVolumeSlider.onValueChanged.AddListener(value => _model.MusicVolume = value);
        effectsVolumeSlider.onValueChanged.AddListener(value => _model.EffectsVolume = value);
        muteMusicToggle.onValueChanged.AddListener(value => _model.MuteMusic = value);
        muteEffectsToggle.onValueChanged.AddListener(value => _model.MuteEffects = value);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectsVolumeSlider.onValueChanged.RemoveAllListeners();
        muteMusicToggle.onValueChanged.RemoveAllListeners();
        muteEffectsToggle.onValueChanged.RemoveAllListeners();
    }

    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);

        foreach (var parameter in parameters)
        {
            if (parameter is AudioSettingsModel model)
            {
                _model = model;

                break;
            }
        }

        SetUIComponentsValues(_model);
    }

    private void SetUIComponentsValues(AudioSettingsModel model)
    {
        musicVolumeSlider.value = model.MusicVolume;
        effectsVolumeSlider.value = model.EffectsVolume;
        muteMusicToggle.isOn = model.MuteMusic;
    }

    protected override void SetBack()
    {
        base.SetBack();

        SetUIComponentsValues(_model);
    }

    protected override void SetDiscard()
    {
        base.SetDiscard();

        SetUIComponentsValues(_model);
    }
}
