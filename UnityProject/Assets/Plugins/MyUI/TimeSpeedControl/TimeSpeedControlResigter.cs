#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public static class TimeSpeedControlResigter
{
    [MenuItem("GameObject/UI/MyExtentions/TimeSpeedControlResigter")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("TimeSpeedControl"), parent.transform) as GameObject;
        instance.name = "TimeSpeedControl";
    }
}

#endif
