using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIControllerBase : MonoBehaviour
{
    protected UIViewBase[] views;
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        views = GetComponentsInChildren<UIViewBase>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public abstract void Initialize(params object[] parameters);

    public void CanvasAlpha(bool isHide)
    {
        canvasGroup.alpha = isHide?0:1;
        canvasGroup.interactable = !isHide;
        canvasGroup.blocksRaycasts = !isHide;
    }

    public UIViewBase GetViewByID(UI viewID)
    {
        foreach (UIViewBase view in views)
        {
            if (view.ID == viewID)
            {
                return view;
            }
        }
        return null;
    }

    public void SetViewActive(UI viewID, bool isActive)
    {
        UIViewBase view = GetViewByID(viewID);
        if (view != null)
        {
            view.gameObject.SetActive(isActive);
        }
    }

    public void ShowAllView()
    {
        if(views.Length>0)
        {
            foreach (UIViewBase view in views)
            {
                view.gameObject.SetActive(true);
            }
        }
    }

    public void HideAllView()
    {
        if (views.Length > 0)
        {
            foreach (UIViewBase view in views)
            {
                view.gameObject.SetActive(false);
            }
        }
    }

    protected virtual void AddListeners() { }
    protected virtual void RemoveListeners() { }
    public abstract void Conclude();
}