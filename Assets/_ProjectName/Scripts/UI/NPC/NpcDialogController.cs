using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NpcDialogController : AnimatorControllerBase
{
    public Action OnConversationEnded;

    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Image playerIcon;

    private NpcDataSO currentNpcData;
    private DialogNode currentNode;

    [SerializeField] private Transform optionsContainer;
    [SerializeField] private Button buttonPrefab;

    private bool open = false;

    [SerializeField] private float typingSpeed = 0.03f;
    private Coroutine typingCoroutine;
    private bool isTyping;
    public void ShowDialog(Sprite icon, string dialog)
    {
        playerIcon.sprite = icon;
        dialogText.text = dialog;

        open = !open;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, open);
    }

    public void StartConversation(Sprite npcSprite, DialogNode startNode, NpcDataSO npcData)
    {
        playerIcon.sprite = npcSprite;
        currentNpcData = npcData;
        currentNode = startNode;

        ShowNode();
    }
    private void EndConversation()
    {
        ClearOptions();

        Close();

        currentNode = null;
        AlarmBarController.IncreasedPanic?.Invoke(15, SuspicionType.WrongInteraction);

        OnConversationEnded?.Invoke();
    }
    private void ShowNode()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentNode.text));

        ClearOptions();

        if (currentNode.endsConversation)
        {
            Invoke(nameof(EndConversation), 1f);
            return;
        }

        foreach (var option in currentNode.options)
        {
            CreateOptionButton(option);
        }

        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, true);
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
        EventSystem.current.SetSelectedGameObject(btn.gameObject);

        btn.GetComponentInChildren<TMP_Text>().text = option.text;

        btn.onClick.AddListener(() =>
        {
            GoToNode(option.nextNodeId);
        });
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
            SoundManager.Instance.PlaySFXByName(Constants.SFX_POP, 0.5f);
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    public void Close()
    {
        open = false;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);
    }
}
