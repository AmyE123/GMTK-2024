using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayGameButtonController : MonoBehaviour {

    public Animator fadeAnimator;
    public float fadeDuration = 1f;
    public float uiFadeDuration = 3f;
    public float sceneSwitchDelay = 1f;
    public Graphic[] uiElementsToFade;
    public AudioSource audioSource;

    public void OnPlayGameButtonPressed() {
        StartCoroutine(FadeUIAndScene());
    }

    private IEnumerator FadeUIAndScene() {

        StartCoroutine(FadeUIElements());
        StartCoroutine(FadeAudio());

        float delay = Mathf.Max(0f, uiFadeDuration - sceneSwitchDelay);
        yield return new WaitForSeconds(delay);

        StartCoroutine(FadeAndSwitchScene());
    }

    private IEnumerator FadeUIElements() {
        float elapsedTime = 0f;

        while (elapsedTime < uiFadeDuration) {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / uiFadeDuration);

            foreach (Graphic uiElement in uiElementsToFade) {
                Color color = uiElement.color;
                color.a = alpha;
                uiElement.color = color;
            }

            yield return null;
        }

        foreach (Graphic uiElement in uiElementsToFade) {
            Color color = uiElement.color;
            color.a = 0f;
            uiElement.color = color;
        }
    }

    private IEnumerator FadeAudio() {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < uiFadeDuration) {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / uiFadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
    }

    private IEnumerator FadeAndSwitchScene() {

        fadeAnimator.SetTrigger("FadeOutTrigger");

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene("IntroCutscene");
    }
}