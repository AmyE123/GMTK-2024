using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayGameButtonController : MonoBehaviour
{
    public Animator fadeAnimator;
    public float fadeDuration = 1f;
    public float uiFadeDuration = 3f;
    public float sceneSwitchDelay = 1f;
    public Graphic[] uiElementsToFade;
    public AudioSource audioSource;
    public Image topBar, bottomBar; // References to the top and bottom black bars
    public Image backgroundImage; // Reference to the background image

    public void OnPlayGameButtonPressed()
    {
        StartCoroutine(FadeUIAndScene());
    }

    private IEnumerator FadeUIAndScene()
    {
        StartCoroutine(FadeBlackBars()); // Start fading in black bars
        StartCoroutine(FadeUIElements());
        StartCoroutine(FadeAudio());
        StartCoroutine(ZoomBackgroundImage()); // Start zooming the background image

        float delay = Mathf.Max(0f, uiFadeDuration - sceneSwitchDelay);
        yield return new WaitForSeconds(delay);

        StartCoroutine(FadeAndSwitchScene());
    }

    private IEnumerator FadeUIElements()
    {
        float elapsedTime = 0f;

        while (elapsedTime < uiFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / uiFadeDuration);

            foreach (Graphic uiElement in uiElementsToFade)
            {
                Color color = uiElement.color;
                color.a = alpha;
                uiElement.color = color;
            }

            yield return null;
        }

        foreach (Graphic uiElement in uiElementsToFade)
        {
            Color color = uiElement.color;
            color.a = 0f;
            uiElement.color = color;
        }
    }

    private IEnumerator FadeBlackBars()
    {
        float elapsedTime = 0f;

        while (elapsedTime < uiFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / uiFadeDuration);
            Color topColor = topBar.color;
            Color bottomColor = bottomBar.color;
            topColor.a = bottomColor.a = alpha;
            topBar.color = topColor;
            bottomBar.color = bottomColor;

            yield return null;
        }
    }

    private IEnumerator FadeAudio()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < uiFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / uiFadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
    }

    private IEnumerator FadeAndSwitchScene()
    {
        fadeAnimator.SetTrigger("FadeOutTrigger");

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene("IntroCutscene");
    }

    private IEnumerator ZoomBackgroundImage()
    {
        Vector3 initialScale = backgroundImage.rectTransform.localScale; // Get the initial scale
        Vector3 targetScale = initialScale * 0.95f; // Increase the zoom target to 30% larger
        float zoomDuration = uiFadeDuration; // You can adjust this duration if needed
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            backgroundImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / zoomDuration);
            yield return null;
        }
    }
}