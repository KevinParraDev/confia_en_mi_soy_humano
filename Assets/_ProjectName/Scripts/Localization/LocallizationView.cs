using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizationView : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent localizeStringEvent;

    public void SetLocalization(string tableName, string key)
    {
        if (localizeStringEvent == null)
        {
            Debug.LogError("LocalizeStringEvent not assigned");
            return;
        }
        
        localizeStringEvent.StringReference.TableReference = tableName;
        localizeStringEvent.StringReference.TableEntryReference = key;
        localizeStringEvent.RefreshString();
    }
}
