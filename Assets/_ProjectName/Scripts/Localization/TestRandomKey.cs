using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationTest : MonoBehaviour
{
    [SerializeField] private LocalizationView localizationView;
    [SerializeField] private string tableName = "UI_Text";
    [SerializeField] private string entryBaseName = "random_message";

    private void Start()
    {
        LocalizationSettings.InitializationOperation.Completed += _ => SetRandomText();
    }

    private void SetRandomText()
    {
        int randomIndex = Random.Range(1, 4);
        Debug.Log(randomIndex);
        string randomKey = $"{entryBaseName}_{randomIndex}";

        if (localizationView != null)
        {
            localizationView.SetLocalization(tableName, randomKey);
        }
        else
        {
            Debug.LogError("LocalizationView is not assigned!");
        }
    }
}