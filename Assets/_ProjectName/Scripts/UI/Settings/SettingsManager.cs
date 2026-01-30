using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour, ISettingManager
{
    [Serializable]
    private class SettingsControllers
    {
        public SettingsTypes SettingType;
        public GameObject Prefab;
        public bool IsActivated;
    }

    [Header("Configuration")]
    [SerializeField] private SettingsControllers[] settingsControllers;
    [SerializeField] private KeyCode openKey =KeyCode.Escape;

    [Header("References")]
    [SerializeField] private SettingsModel settingsModel;
    [SerializeField] private SettingsController mainSettingsController;

    private bool isSettingsOpen = false;

    //TO DO mejorar
    private Dictionary<SettingsTypes, UIControllerBase> controllersActives = new();

    public void Initialize()
    {
        isSettingsOpen = false;

        settingsModel?.LoadSettings();

        mainSettingsController.Initialize(this);

        mainSettingsController.CanvasAlpha(true);

        SetSettingsEnables();

        SetupButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            ToggleSettingsView();
        }
    }

    private void ToggleSettingsView()
    {
        if (isSettingsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void SetSettingsEnables()
    {
        foreach (var controller in settingsControllers)
        {
            if (!controller.IsActivated) continue;

            GameObject controllerPrefab = Instantiate(controller.Prefab, transform);
            if (controllerPrefab.TryGetComponent(out UIControllerBase uiController))
            {
                uiController.Initialize(settingsModel);
                uiController.HideAllView();
                controllersActives[controller.SettingType] = uiController;
            }
            else
            {
                Debug.LogWarning($"Prefab {controller.Prefab.name} dont have UIControllerBase.");
            }
        }
    }

    private void SetupButtons()
    {
        List<SettingsTypes> activeSettings = controllersActives.Keys.ToList();
        List<Action> actions = controllersActives.Values.Select(controller => (Action)controller.ShowAllView).ToList();

        mainSettingsController.SetSettingsButton(activeSettings.ToArray(), actions.ToArray());
    }

    public void Open()
    {
        mainSettingsController.CanvasAlpha(false);
        isSettingsOpen = true;
    }

    public void Close()
    {
        mainSettingsController.CanvasAlpha(true);
        isSettingsOpen = false;
    }

    public void Conclude()
    {
        mainSettingsController.Conclude();

        foreach (var controller in controllersActives.Values)
        {
            controller.Conclude();
        }

        controllersActives.Clear();
    }
}

