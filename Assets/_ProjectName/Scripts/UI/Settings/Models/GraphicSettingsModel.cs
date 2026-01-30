using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphicSettingsModel : SettingsModelBase
{
    private enum ResolutionType
    {
        HD,
        FullHD,
        QHD,
        UHD_4K
    }

    private enum QualityLevelType
    {
        Low,
        Medium,
        High,
        Ultra
    }

    [SerializeField] private ResolutionType selectedResolution = ResolutionType.FullHD;
    [SerializeField] private bool isFullscreen = true;
    [SerializeField] private QualityLevelType selectedQualityLevel = QualityLevelType.High;

    private static readonly Dictionary<ResolutionType, Resolution> resolutionDictionary = new()
    {
        { ResolutionType.HD, new Resolution { width = 1280, height = 720 } },
        { ResolutionType.FullHD, new Resolution { width = 1920, height = 1080 } },
        { ResolutionType.QHD, new Resolution { width = 2560, height = 1440 } },
        { ResolutionType.UHD_4K, new Resolution { width = 3840, height = 2160 } }
    };

    public string[] ResolutionNames => Enum.GetNames(typeof(ResolutionType));

    public string[] QualityLevelNames => Enum.GetNames(typeof(QualityLevelType));

    public int SelectedResolutionIndex
    {
        get => (int)selectedResolution;
        set
        {
            if (Enum.IsDefined(typeof(ResolutionType), value))
            {
                selectedResolution = (ResolutionType)value;
            }
        }
    }

    public bool IsFullscreen
    {
        get => isFullscreen;
        set => isFullscreen = value;
    }

    public int SelectedQualityIndex
    {
        get => (int)selectedQualityLevel;
        set
        {
            if (Enum.IsDefined(typeof(QualityLevelType), value))
            {
                selectedQualityLevel = (QualityLevelType)value;
            }
        }
    }

    public override void LoadSettings()
    {
        string savedResolution = PlayerPrefs.GetString("SelectedResolutionKey", ResolutionType.FullHD.ToString());
        if (Enum.TryParse(savedResolution, out ResolutionType parsedResolution))
        {
            selectedResolution = parsedResolution;
        }
        else
        {
            selectedResolution = ResolutionType.FullHD;
        }

        string savedQuality = PlayerPrefs.GetString("SelectedQualityKey", QualityLevelType.High.ToString());
        if (Enum.TryParse(savedQuality, out QualityLevelType parsedQuality))
        {
            selectedQualityLevel = parsedQuality;
        }
        else
        {
            selectedQualityLevel = QualityLevelType.High;
        }

        isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;

        ApplySettings();
    }

    public override void SaveSettings()
    {
        PlayerPrefs.SetString("SelectedResolutionKey", selectedResolution.ToString());
        PlayerPrefs.SetString("SelectedQualityKey", selectedQualityLevel.ToString());
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        ApplySettings();
    }

    private void ApplySettings()
    {
        if (resolutionDictionary.TryGetValue(selectedResolution, out Resolution resolution))
        {
            Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        }
        else
        {
            selectedResolution = ResolutionType.FullHD;
            ApplySettings();
        }

        QualitySettings.SetQualityLevel((int)selectedQualityLevel);
    }
}
