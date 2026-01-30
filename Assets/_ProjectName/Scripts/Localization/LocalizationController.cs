using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationController : MonoBehaviour
{
    private string currentLocaleCode;
    public void Initialize()
    {
        currentLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            ToggleLanguage();
        }
    }

    void ToggleLanguage()
    {
        currentLocaleCode = currentLocaleCode == "en" ? "es" : "en";
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(currentLocaleCode);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"Idioma cambiado a: {locale.LocaleName}");
        }
        else
        {
            Debug.LogWarning($"No se encontró el idioma: {currentLocaleCode}");
        }
    }
}
