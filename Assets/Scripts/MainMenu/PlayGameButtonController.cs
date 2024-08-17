using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayGameButtonController : MonoBehaviour
{
    public Animator fadeAnimator; 
    public float fadeDuration = 1f; 

    public void OnPlayGameButtonPressed() {
        StartCoroutine(FadeAndSwitchScene());
    }

    private IEnumerator FadeAndSwitchScene() {
        fadeAnimator.SetTrigger("FadeOutTrigger");

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene("IntroCutscene");
    }
}
