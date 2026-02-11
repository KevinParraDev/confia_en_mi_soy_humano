using UnityEngine;

public class DialogueTriggerNPC : MonoBehaviour
{
    [SerializeField]
    NPCBaseController npcBaseController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _))
        {
            npcBaseController.SetToInitialDialogue();
            gameObject.SetActive(false);
        }
    }
}
