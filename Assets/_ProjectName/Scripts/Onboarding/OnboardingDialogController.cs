using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OnboardingDialogController : AnimatorControllerBase
{
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputAction submitAction;

    [Header("UI References")]
    [SerializeField] private RectTransform suspicionBar;
    [SerializeField] private RectTransform mapUI;

    [Header("Game References")]
    [SerializeField] private OvniButtonController ovniButton;

    private bool isTyping;
    private bool submitPressed;
    private bool finishTypingInstantly;

    private bool firstDialogFinished;
    private bool postTransformationDialogPlayed;

    private string currentFullText;

    protected override void Awake()
    {
        base.Awake();
        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    private void OnEnable()
    {
        FaceContainerController.onNPCIsCopied += OnFirstTransformation;

        submitAction.Enable();
        submitAction.canceled += OnSubmit;

        if (ovniButton != null)
            ovniButton.Failed += OnOvniInteractionFailed;
    }

    private void OnDisable()
    {
        FaceContainerController.onNPCIsCopied -= OnFirstTransformation;

        submitAction.canceled -= OnSubmit;
        submitAction.Disable();

        if (ovniButton != null)
            ovniButton.Failed -= OnOvniInteractionFailed;
    }

    private void Start()
    {
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        suspicionBar.localScale = Vector3.zero;
        mapUI.localScale = Vector3.zero;

        StartCoroutine(StartIntro());
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (isTyping)
        {
            finishTypingInstantly = true;
        }
        else
        {
            submitPressed = true;
        }
    }

    private IEnumerator StartIntro()
    {
        yield return ShowDialogSequence(
        "Hola agente. Estamos a punto de llegar a la Tierra para la misión.",
        "Tu trabajo será infiltrarte en la Casa Blanca y destruir el planeta. Trata de no llamar la atención.",
        "Para lograrlo debes usar tu habilidad de transformarte en otras personas.",
        "Acércate a un rostro para poder robarlo."
        );

        EnableMovement();
    }

    private void EnableMovement()
    {
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        firstDialogFinished = true;
    }

    private void OnFirstTransformation()
    {
        if (!firstDialogFinished || postTransformationDialogPlayed)
            return;

        ovniButton.Initialized();
        postTransformationDialogPlayed = true;
        StartCoroutine(PostTransformationSequence());
    }

    private IEnumerator PostTransformationSequence()
    {
        yield return new WaitForSeconds(2.3f);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        yield return ShowDialogSequence(
        "Con cada rostro podrás hacer diferentes cosas.",
        "Pero si haces algo sospechoso aumentará tu barra de alerta."
        );

        yield return AnimateAppear(suspicionBar);

        yield return ShowDialogSequence(
        "No dejes que se llene o irán por ti."
        );

        yield return ShowDialogSequence(
        "Si te sientes perdido, abre el mapa."
        );

        yield return AnimateAppear(mapUI);

        yield return ShowDialogSequence(
        "Buena suerte."
        );

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);
    }

    private void OnOvniInteractionFailed()
    {
        StartCoroutine(OvniFailedSequence());
    }

    private IEnumerator OvniFailedSequence()
    {
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        yield return ShowDialogSequence(
        "Vuelve a tu forma original para iniciar la misión."
        );

        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");
    }

    private IEnumerator ShowDialogSequence(params string[] messages)
    {
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, true);

        foreach (var msg in messages)
        {
            yield return TypeText(msg);

            submitPressed = false;
            yield return new WaitUntil(() => submitPressed);
        }
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        finishTypingInstantly = false;

        currentFullText = fullText;
        dialogText.text = "";

        foreach (char letter in fullText)
        {
            if (finishTypingInstantly)
            {
                dialogText.text = fullText;
                break;
            }

            SoundManager.Instance.PlaySFXByName(Constants.SFX_POP, 0.5f, 0.95f, 1.05f);
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        dialogText.text = fullText;
        isTyping = false;
    }

    private IEnumerator AnimateAppear(RectTransform target)
    {
        float time = 0f;
        float duration = 0.4f;

        target.localScale = Vector3.zero;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        target.localScale = Vector3.one;
    }
}