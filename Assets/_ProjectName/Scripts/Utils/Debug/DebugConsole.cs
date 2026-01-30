using System;
using UnityEngine;
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour
{
    private string input = "";
    private List<string> logs = new List<string>();
    private Vector2 scrollPosition;
    private bool isVisible = false;

    private Dictionary<string, Action<string[]>> commands;

    private void Awake()
    {
        InitializeCommands();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) // Toggle console with ` key
        {
            isVisible = !isVisible;
        }
    }

    private void OnGUI()
    {
        if (!isVisible) return;

        GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Debug Console");

        // Display logs
        GUILayout.BeginArea(new Rect(20, 20, Screen.width - 40, Screen.height - 100));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
        foreach (var log in logs)
        {
            GUILayout.Label(log);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // Input field
        GUILayout.BeginArea(new Rect(20, Screen.height - 70, Screen.width - 40, 50));
        GUILayout.BeginHorizontal();
        input = GUILayout.TextField(input, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Execute", GUILayout.Width(100)))
        {
            ExecuteCommand(input);
            input = "";
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, Action<string[]>>
        {
            { "clear", args => logs.Clear() },
            { "help", args => logs.Add("Available commands: clear, help, fps, timeScale [value]") },
            { "fps", args => logs.Add($"FPS: {1.0f / Time.unscaledDeltaTime:F2}") },
            { "timeScale", args =>
                {
                    if (args.Length > 1 && float.TryParse(args[1], out float scale))
                    {
                        Time.timeScale = scale;
                        logs.Add($"Time scale set to {scale}");
                    }
                    else
                    {
                        logs.Add("Invalid argument for timeScale");
                    }
                }
            },
            { "memory", args => logs.Add($"Memory: {System.GC.GetTotalMemory(false) / (1024 * 1024):F2} MB") },
            { "log", args => logs.Add(string.Join(" ", args, 1, args.Length - 1)) }
        };
    }

    private void ExecuteCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;

        logs.Add($"> {command}");
        string[] args = command.Split(' ');

        if (commands.TryGetValue(args[0], out var action))
        {
            try
            {
                action(args);
            }
            catch (Exception ex)
            {
                logs.Add($"Error executing command: {ex.Message}");
            }
        }
        else
        {
            logs.Add($"Unknown command: {args[0]}");
        }
    }
}
