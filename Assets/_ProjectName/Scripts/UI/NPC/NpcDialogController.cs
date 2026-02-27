using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NpcDialogController : AnimatorControllerBase
{
    public Action OnConversationEnded;

    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Image playerIcon;

    private NpcDataSO currentNpcData;
    private DialogNode currentNode;

    [SerializeField] private Transform optionsContainer;
    [SerializeField] private Button buttonPrefab;

    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool open = false;

    [SerializeField] private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    public void StartConversation(Sprite npcSprite, DialogNode startNode, NpcDataSO npcData)
    {
        playerIcon.sprite = npcSprite;
        currentNpcData = npcData;
        currentNode = startNode;

        Open();
        ShowNode();
    }
    public void ShowDialog(Sprite icon, string dialog)
    {
        playerIcon.sprite = icon;
        dialogText.text = dialog;

        open = !open;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, open);
    }
    private void Open()
    {
        open = true;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, true);
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");
    }

    private void EndConversation()
    {
        ClearOptions();
        Close();
        currentNode = null;
        OnConversationEnded?.Invoke();
        currentNpcData.hasSpoken = true;
    }

    private void ShowNode()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentNode.text));

        ClearOptions();

        if (currentNode.endsConversation)
        {
            StartCoroutine(WaitAndEndAfterTyping());
            return;
        }

        foreach (var option in currentNode.options)
        {
            CreateOptionButton(option);
        }
    }

    private IEnumerator WaitAndEndAfterTyping()
    {
        while (isTyping)
            yield return null;

        yield return new WaitForSeconds(0.5f);
        EndConversation();
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateOptionButton(DialogOption option)
    {
        Button btn = Instantiate(buttonPrefab, optionsContainer);
        btn.GetComponentInChildren<TMP_Text>().text = option.text;

        btn.onClick.AddListener(() =>
        {
            if (option.suspicionChange > 0)
                AlarmBarController.IncreasedPanic?.Invoke(option.suspicionChange, SuspicionType.WrongInteraction);

            GoToNode(option.nextNodeId);
        });

        EventSystem.current.SetSelectedGameObject(btn.gameObject);
    }

    private void GoToNode(string nodeId)
    {
        currentNode = FindNodeById(nodeId);
        ShowNode();
    }

    private DialogNode FindNodeById(string id)
    {
        foreach (var convo in currentNpcData.conversations)
        {
            var node = convo.dialogNodes.Find(n => n.id == id);
            if (node != null)
                return node;
        }

        Debug.LogWarning("Nodo no encontrado: " + id);
        return null;
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in fullText)
        {
            SoundManager.Instance.PlaySFXByName(Constants.SFX_POP, 0.5f, 0.95f, 1.05f);
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void Close()
    {
        open = false;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");
    }
}