using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGameButtonController : MonoBehaviour {
    private string lastVisitedURL;

    void Start() {
#if UNITY_WEBGL
        lastVisitedURL = Application.absoluteURL;
        if (string.IsNullOrEmpty(lastVisitedURL)) {
            lastVisitedURL = "https://itch.io/"; // Defaults to https://itch.io/
        }
#endif
    }

    public void Quit() {
        Debug.Log("Quit button pressed");
#if UNITY_STANDALONE
        Application.Quit();
#elif UNITY_WEBGL
        Application.OpenURL(lastVisitedURL);
#elif UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}