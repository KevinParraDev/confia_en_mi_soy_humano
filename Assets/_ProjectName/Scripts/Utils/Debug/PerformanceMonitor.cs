using UnityEngine;

public class PerformanceMonitor : MonoBehaviour
{
    private float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        int width = Screen.width, height = Screen.height;
        Rect rect = new Rect(10, 10, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100;
        style.normal.textColor = Color.white;

        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);

        GUI.Label(rect, text, style);

        Rect memoryRect = new Rect(10, 40, width, height * 2 / 100);
        string memoryText = string.Format("Memory: {0:0.0} MB", System.GC.GetTotalMemory(false) / (1024 * 1024));
        GUI.Label(memoryRect, memoryText, style);
    }
}
