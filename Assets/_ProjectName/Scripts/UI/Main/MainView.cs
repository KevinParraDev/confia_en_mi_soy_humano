using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainView : UIViewBase
{
    private IMainController mainController;
    [SerializeField] private Button playButton;
    [SerializeField] private Button CreditsButton;
    [SerializeField] private Button BackButton;

    private Animator animator;

    protected override void Awake()
    {
        id = UI.Main;
        animator = GetComponent<Animator>();
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
        playButton.onClick.AddListener(OnClickPlay);
        CreditsButton.onClick.AddListener(OnClickCredits);
        BackButton.onClick.AddListener(OnClickBack);
    }

    private void OnClickPlay()
    {
        mainController?.OnPlay();
        animator.SetTrigger(Constants.ANIM_PANNEL_Start);
    }
    private void OnClickCredits()
    {
        animator.SetBool(Constants.ANIM_PANNEL_APPEAR, true);
        EventSystem.current.SetSelectedGameObject(BackButton.gameObject);
    }
    private void OnClickBack()
    {
        animator.SetBool(Constants.ANIM_PANNEL_APPEAR, false);
        EventSystem.current.SetSelectedGameObject(CreditsButton.gameObject);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        playButton.onClick.RemoveListener(OnClickPlay);
        CreditsButton.onClick.RemoveListener(OnClickCredits);
        BackButton.onClick.RemoveListener(OnClickBack);
    }

    public override void Conclude()
    {
        RemoveListeners();
    }
}
