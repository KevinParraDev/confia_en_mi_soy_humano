using System;
using UnityEngine;

[Serializable]
public class AudioSettingsModel : SettingsModelBase
{
    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private float effectsVolume = 1.0f;
    [SerializeField] private bool muteMusic = false;
    [SerializeField] private bool muteEffects = false;

    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    public float EffectsVolume { get { return effectsVolume; } set { effectsVolume = value; } }
    public bool MuteMusic { get { return muteMusic; } set { muteMusic = value; } }
    public bool MuteEffects { get { return muteEffects; } set { muteEffects = value; } }

    public override void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
        muteMusic = PlayerPrefs.GetInt("MuteMusic", 0) == 1;
        muteEffects = PlayerPrefs.GetInt("MuteEffects", 0) == 1;

        ApplySettings();
    }

    public override void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
        PlayerPrefs.SetInt("MuteMusic", muteMusic ? 1 : 0);
        PlayerPrefs.SetInt("MuteEffects", muteEffects ? 1 : 0);
        PlayerPrefs.Save();
        ApplySettings();
    }

    private void ApplySettings()
    {
        //if (MusicManager.Instance != null)
        //{
        //    var musicManager = MusicManager.Instance;

        //    if (muteMusic)
        //        musicManager.DesactiveAll();
        //    else
        //        musicManager.ActiveAll();

        //    musicManager.SetMixerVolume(musicVolume);
        //}

        //if (SoundManager.Instance != null)
        //{
        //    var soundManager = SoundManager.Instance;

        //    if (muteEffects)
        //        soundManager.DesactiveAll();
        //    else
        //        soundManager.ActiveAll();

        //    soundManager.SetMixerVolume(effectsVolume);
        //}
    }
}
