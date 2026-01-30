using System;
using UnityEngine;

public class MainController : UIControllerBase, IMainController
{
    public Action PlayActivated;

    private MainView mainView;
    [SerializeField] private PlayerEntity playerEntity;

    public override void Initialize(params object[] parameters)
    {
        

        mainView = views[0] as MainView;
        if (mainView == null)
        {
            Debug.LogError("MainController - View is not a MainView");
            return;
        }

        mainView.Initialize(this);
        ShowAllView();
    }

    public void OnPlay()
    {
        Debug.Log("OnClick-MainController");
        PlayActivated?.Invoke();
    }

    public override void Conclude()
    {
        mainView?.Conclude();
        HideAllView();
    }
}