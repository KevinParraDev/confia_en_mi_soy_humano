using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class ControlsSettingsView : SettingsBaseView
{
    [Header("Configuration Buttons")]
    [SerializeField] private TMP_InputField upActionInputField;
    [SerializeField] private TMP_InputField downActionInputField;
    [SerializeField] private TMP_InputField leftActionInputField;
    [SerializeField] private TMP_InputField rightActionInputField;
    [SerializeField] private TMP_InputField interactActionInputField;

    private ControlSettingsModel _model;

    protected override void Awake()
    {
        id = UI.Settings_Controls;
    }

    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);

        foreach (var parameter in parameters)
        {
            if (parameter is ControlSettingsModel model)
            {
                _model = model;

                break;
            }
        }

        SetUIComponentsValues(_model);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        upActionInputField.onSelect.AddListener((string param) => StartRebindAction("Up"));
        downActionInputField.onSelect.AddListener((string param) => StartRebindAction("Down"));
        leftActionInputField.onSelect.AddListener((string param) => StartRebindAction("Left"));
        rightActionInputField.onSelect.AddListener((string param) => StartRebindAction("Right"));
        interactActionInputField.onSelect.AddListener((string param) => StartRebindAction("Interact"));
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        upActionInputField.onSelect.RemoveAllListeners();
        downActionInputField.onSelect.RemoveAllListeners();
        leftActionInputField.onSelect.RemoveAllListeners();
        rightActionInputField.onSelect.RemoveAllListeners();
        interactActionInputField.onSelect.RemoveAllListeners();
    }

    private void SetUIComponentsValues(ControlSettingsModel model)
    {
        upActionInputField.text = model.UpAction.Substring("<Keyboard>/".Length).ToUpper();
        downActionInputField.text = model.DownAction.Substring("<Keyboard>/".Length).ToUpper();
        leftActionInputField.text = model.LeftAction.Substring("<Keyboard>/".Length).ToUpper();
        rightActionInputField.text = model.RightAction.Substring("<Keyboard>/".Length).ToUpper();
        interactActionInputField.text = model.InteractAction.Substring("<Keyboard>/".Length).ToUpper();
    }

    protected override void SetSave()
    {
        base.SetSave();

        ResetInputsFields();
    }

    protected override void SetDiscard()
    {
        base.SetDiscard();

        SetUIComponentsValues(_model);

        ResetInputsFields();
    }

    protected override void SetBack()
    {
        base.SetBack();

        SetUIComponentsValues(_model);

        ResetInputsFields();
    }

    private void ResetInputsFields()
    {
        upActionInputField.interactable = true;
        downActionInputField.interactable = true;
        leftActionInputField.interactable = true;
        rightActionInputField.interactable = true;
        interactActionInputField.interactable = true;
    }

    private void StartRebindAction(string actionName)
    {
        StartCoroutine(WaitForKeyPress(actionName));
    }

    private IEnumerator WaitForKeyPress(string actionName)
    {
        TMP_InputField inputField = null;
        string currentAction = string.Empty;

        switch (actionName)
        {
            case "Up":
                inputField = upActionInputField;
                currentAction = _model.UpAction;
                break;
            case "Down":
                inputField = downActionInputField;
                currentAction = _model.DownAction;
                break;
            case "Left":
                inputField = leftActionInputField;
                currentAction = _model.LeftAction;
                break;
            case "Right":
                inputField = rightActionInputField;
                currentAction = _model.RightAction;
                break;
            case "Interact":
                inputField = interactActionInputField;
                currentAction = _model.InteractAction;
                break;
        }

        inputField.text = currentAction.Substring("<Keyboard>/".Length);
        inputField.interactable = true;
        yield return null;

        while (!Keyboard.current.anyKey.wasPressedThisFrame)
            yield return null;

        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                switch (actionName)
                {
                    case "Up":
                        _model.UpAction = $"<Keyboard>/{key.name}";
                        break;
                    case "Down":
                        _model.DownAction = $"<Keyboard>/{key.name}";
                        break;
                    case "Left":
                        _model.LeftAction = $"<Keyboard>/{key.name}";
                        break;
                    case "Right":
                        _model.RightAction = $"<Keyboard>/{key.name}";
                        break;
                    case "Interact":
                        _model.InteractAction = $"<Keyboard>/{key.name}";
                        break;
                }

                inputField.text = $"{key.name}".ToUpper();
                inputField.interactable = false;

                break;
            }
        }
    }

    public override void Conclude()
    {
        StopCoroutine(WaitForKeyPress(""));
    }
}
