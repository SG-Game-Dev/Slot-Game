using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderpinSlots
{
    public class SceneLoader : ISceneLoader
    {
        public void Load(string sceneName)
        {
            // Always un-pause before leaving a scene, otherwise the next scene starts frozen.
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
    }
}
