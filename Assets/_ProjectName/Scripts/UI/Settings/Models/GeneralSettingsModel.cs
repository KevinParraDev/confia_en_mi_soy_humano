using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System;
using System.Linq;

[Serializable]
public class GeneralSettingsModel : SettingsModelBase
{
    [SerializeField] private int selectedLanguageIndex = 0;
    [SerializeField] private string[] availableLanguages = Array.Empty<string>();

    public string[] AvailableLanguages => availableLanguages;

    public int SelectedLanguageIndex
    {
        get => selectedLanguageIndex;
        set
        {
            if (value >= 0 && value < availableLanguages.Length)
            {
                selectedLanguageIndex = value;
                ApplySettings();
            }
        }
    }

    public override void LoadSettings()
    {
        UpdateAvailableLanguages();
        selectedLanguageIndex = PlayerPrefs.GetInt("SelectedLanguageIndex", 0);
        ApplySettings();
    }

    public override void SaveSettings()
    {
        PlayerPrefs.SetInt("SelectedLanguageIndex", selectedLanguageIndex);
        PlayerPrefs.Save();
        ApplySettings();
    }

    private void UpdateAvailableLanguages()
    {
        if (LocalizationSettings.AvailableLocales != null)
        {
            availableLanguages = LocalizationSettings.AvailableLocales.Locales
                .Select(locale => locale.LocaleName) 
                .ToArray();
        }
        else
        {
            availableLanguages = new string[] { "English" }; 
        }
    }

    private void ApplySettings()
    {
        if (LocalizationSettings.AvailableLocales.Locales.Count > selectedLanguageIndex)
        {
            Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
    }
}
