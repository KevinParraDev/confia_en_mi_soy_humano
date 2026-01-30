using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class RandomTextModel
{
    public List<string> GetRandomTextKeys(string entryBaseName)
    {
        List<string> randomKeys = new List<string>();
        int index = 1;

        while (true)
        {
            string key = $"{entryBaseName}_{index}";
            string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString("RandomTexts", key);
            if (string.IsNullOrEmpty(localizedString) || localizedString.StartsWith("[MISSING"))
            {
                break;
            }
            randomKeys.Add(key);
            index++;
        }

        return randomKeys;
    }
    
    public string GetRandomTextKey(string entryBaseName)
    {
        List<string> keys = GetRandomTextKeys(entryBaseName);
        if (keys.Count > 0)
        {
            int randomIndex = Random.Range(0, keys.Count);
            return keys[randomIndex];
        }

        return "[MISSING TEXT]";
    }
}
