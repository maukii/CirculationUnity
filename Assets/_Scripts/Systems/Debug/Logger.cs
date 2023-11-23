using UnityEngine;

[CreateAssetMenu(fileName = "Logger", menuName = "Logger/new Logger")]
public class Logger : ScriptableObject
{
    public bool showLogs = true;
    public string prefix;
    public Color logColor;


    public void LogInfo(string msg, Object sender = null) => LogMessage(msg, sender, LogType.Info);

    public void LogWarning(string msg, Object sender = null) => LogMessage(msg, sender, LogType.Warning);

    public void LogError(string msg, Object sender = null) => LogMessage(msg, sender, LogType.Error);

    private void LogMessage(string message, Object sender, LogType logType = LogType.Info)
    {
        if (!showLogs) return;

        string logMessage = $"[<color=#{ColorUtility.ToHtmlStringRGB(logColor)}>{prefix}</color>] {message}";

        switch (logType)
        {
            case LogType.Info:
                Debug.Log(logMessage, sender);
                break;
            case LogType.Warning:
                Debug.LogWarning(logMessage, sender);
                break;
            case LogType.Error:
                Debug.LogError(logMessage, sender);
                break;
            default:
                Debug.Log(logMessage, sender);
                break;
        }
    }
}
