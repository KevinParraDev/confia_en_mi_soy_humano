using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainView : UIViewBase
{
    private IMainController mainController;
    [SerializeField] private Button playButton;

    protected override void Awake()
    {
        id = UI.Main;
    }

    public override void Initialize(params object[] parameters)
    {
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        mainController = parameters[0] as IMainController;
        if (mainController == null)
            Debug.LogError($"MainController is missing in {gameObject.name}");

        AddListeners();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        playButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        mainController?.OnPlay();
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        playButton.onClick.RemoveListener(OnClick);
    }

    public override void Conclude()
    {
        RemoveListeners();
    }
}
