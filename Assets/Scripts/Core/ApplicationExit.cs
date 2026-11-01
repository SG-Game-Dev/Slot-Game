using UnityEngine;

namespace UnderpinSlots
{
    public static class ApplicationExit
    {
        // Returns true if the application could actually be closed.
        public static bool TryQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return true;
#elif UNITY_WEBGL
            // A browser tab cannot be closed from a script, so the caller shows a goodbye message.
            Debug.Log("Exit requested (WebGL): nothing to close.");
            return false;
#else
            Application.Quit();
            return true;
#endif
        }
    }
}
