using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGameButtonController : MonoBehaviour {
    public void Quit() {
        Debug.Log("Quit button pressed");
#if UNITY_STANDALONE
        Application.Quit();
#elif UNITY_WEBGL
    Application.OpenURL("about:blank"); 
#elif UNITY_EDITOR
    EditorApplication.isPlaying = false;
#endif
    }
}