#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MyUI
{
    public static class CloseButtonRegister
    {
        [MenuItem("GameObject/UI/MyExtentions/CloseButton")]
        public static void AddDialogPanel(MenuCommand menuCommand)
        {
            var parent = menuCommand.context as GameObject;
            var instance = Object.Instantiate(Resources.Load("CloseButton"), parent.transform) as GameObject;
            instance.name = nameof(CloseButton);
        }
    }
}

#endif