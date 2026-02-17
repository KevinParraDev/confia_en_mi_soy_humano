public class Globals
{
    //Audio
    public static bool MusicActivated = true;
    public static bool SFXActivated = true;

    //Debug Mode
#if UNITY_EDITOR
    public static bool SceneDebugModeActivated = false;
    public static bool DebugCollisionActivated = false;
    public static bool InputDebugModeActivated = false;
    public static bool ConsoleDebug = false;
#else
    public static bool SceneDebugModeActivated = false;
    public static bool DebugCollisionActivated = false;
    public static bool InputDebugModeActivated = false;
    public static bool ConsoleDebug = false;
#endif
}