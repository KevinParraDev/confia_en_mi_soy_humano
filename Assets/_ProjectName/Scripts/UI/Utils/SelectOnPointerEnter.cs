using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnPointerEnter : MonoBehaviour, IPointerEnterHandler, ISelectHandler, ISubmitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySFXByName(Constants.SFX_HOVER);
    }
    public void OnSubmit(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySFXByName(Constants.SFX_CLICK);
    }
}
