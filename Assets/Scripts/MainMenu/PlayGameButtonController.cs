using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayGameButtonController : MonoBehaviour {

    public Animator fadeAnimator;
    public float fadeDuration = 1f;
    public float uiFadeDuration = 3f; 
    public Graphic[] uiElementsToFade; 

    public void OnPlayGameButtonPressed() {
        StartCoroutine(FadeUIAndScene());
    }

    private IEnumerator FadeUIAndScene() {

        StartCoroutine(FadeUIElements());

        yield return new WaitForSeconds(uiFadeDuration);

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

    private IEnumerator FadeAndSwitchScene() {

        //Taken out the fade to black effect for the time being - Until the intro cutscene is worked upon

        //fadeAnimator.SetTrigger("FadeOutTrigger");  


        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene("IntroCutscene");
    }
}