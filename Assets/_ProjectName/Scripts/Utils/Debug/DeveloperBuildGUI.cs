using System;
using UnityEngine;

public class DeveloperBuildGUI : MonoBehaviour
{
    [SerializeField] VersionType versionType;
    [SerializeField] string bundleName = "team.nex.tmnt";
    private string buildInfo;

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            TimeZoneInfo timeZone;
            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");
            }
            catch (TimeZoneNotFoundException)
            {
                timeZone = TimeZoneInfo.Utc;
            }

            DateTime colombiaDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            string currentDate = colombiaDateTime.ToString("yyyyMMdd");
            string buildType = versionType == VersionType.Staging ? "staging" : "production";

            buildInfo = $"{currentDate}-{bundleName}-{buildType}-{Application.version}";
            Debug.Log($"Build Info: {buildInfo}");
        }
    }

    private void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUI.Label(new Rect(10, Screen.height - 30, 500, 20), buildInfo);
        }
    }
}
