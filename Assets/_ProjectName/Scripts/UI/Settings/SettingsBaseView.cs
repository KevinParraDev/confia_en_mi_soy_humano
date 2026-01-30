using UnityEngine;
using UnityEngine.UI;

public class SettingsBaseView : UIViewBase
{
    [Header("General Buttons")]
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button discardBtn;
    [SerializeField] private Button backBtn;

    private ISettingsController _controller;

    protected override void Awake()
    {
       
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        saveBtn.onClick.AddListener(SetSave);
        discardBtn.onClick.AddListener(SetDiscard);
        backBtn.onClick.AddListener(SetBack);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        saveBtn.onClick.RemoveAllListeners();
        discardBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
    }

    public override void Initialize(params object[] parameters)
    {
        if (parameters.Length == 0)
            return;

        foreach (var parameter in parameters)
        {
            if (parameter is ISettingsController controller)
            {
                _controller = controller;

                break;
            }
        }

        AddListeners();
    }

    protected virtual void SetDiscard()
    {
        _controller.Discard();

        Debug.Log("Discard");
    }

    protected virtual void SetBack()
    {
        _controller.Back();
    }

    protected virtual void SetSave()
    {
        _controller.Save();

        Debug.Log("Save");
    }

    public override void Conclude()
    {
        RemoveListeners();
    }

}
