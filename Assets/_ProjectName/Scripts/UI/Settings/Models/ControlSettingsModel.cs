using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ControlSettingsModel : SettingsModelBase
{
    private const string MoveActionName = "Move";
    private const string InteractActionName = "Interact";

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string upAction = "<Keyboard>/w";
    [SerializeField] private string downAction = "<Keyboard>/s";
    [SerializeField] private string rightAction = "<Keyboard>/d";
    [SerializeField] private string leftAction = "<Keyboard>/a";
    [SerializeField] private string interactAction = "<Keyboard>/e";

    public string UpAction { get => upAction; set => upAction = value; }
    public string DownAction { get => downAction; set => downAction = value; }
    public string RightAction { get => rightAction; set => rightAction = value; }
    public string LeftAction { get => leftAction; set => leftAction = value; }
    public string InteractAction { get => interactAction; set => interactAction = value; }

    public override void LoadSettings()
    {
        upAction = PlayerPrefs.GetString("MoveAction", "<Keyboard>/w");
        interactAction = PlayerPrefs.GetString("InteractAction", "<Keyboard>/e");

        ApplySettings();
    }

    public override void SaveSettings()
    {
        PlayerPrefs.SetString("MoveAction", upAction);
        PlayerPrefs.SetString("InteractAction", interactAction);
        PlayerPrefs.Save();
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (inputActions == null)
        {
            Debug.LogWarning("⚠ An InputActionAsset has not been assigned.");
            return;
        }

        var move = inputActions.FindAction(MoveActionName);
        var interact = inputActions.FindAction(InteractActionName);

        if (move != null)
            move.ApplyBindingOverride(2, upAction);
        else
            Debug.LogWarning("⚠ Action 'Move' not found.");

        if (move != null)
            move.ApplyBindingOverride(4, downAction);
        else
            Debug.LogWarning("⚠ Action 'Move' not found.");

        if (move != null)
            move.ApplyBindingOverride(8, rightAction);
        else
            Debug.LogWarning("⚠ Action 'Move' not found.");

        if (move != null)
            move.ApplyBindingOverride(6, leftAction);
        else
            Debug.LogWarning("⚠ Action 'Move' not found.");

        if (interact != null)
            interact.ApplyBindingOverride(0, interactAction);
        else
            Debug.LogWarning("⚠ Action 'Interact' not found.");
    }
}
