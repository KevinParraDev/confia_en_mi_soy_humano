using UnityEngine;
using UnityEngine.EventSystems;

public class LoseScreenView : MonoBehaviour
{
    [SerializeField] private GameObject homeButton;
    public void OnAppearButton()
    {
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
}
