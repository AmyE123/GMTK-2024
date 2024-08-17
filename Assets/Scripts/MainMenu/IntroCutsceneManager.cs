using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : MonoBehaviour {

    public Image[] cutsceneImages;

    private int currentImageIndex = 0;
    private float timeElapsed = 0f;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float displayDuration = 3f;

    void Start() {
        for (int i = 0;i < cutsceneImages.Length;i++) {
            SetImageAlpha(cutsceneImages[i], 0f);
            cutsceneImages[i].gameObject.SetActive(i == 0);
        }
    }

    void Update() {
        timeElapsed += Time.deltaTime;

        float fadeInEnd = fadeDuration;
        float fadeOutStart = fadeInEnd + displayDuration;
        float fadeOutEnd = fadeOutStart + fadeDuration;

        if (timeElapsed < fadeInEnd) {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            SetImageAlpha(cutsceneImages[currentImageIndex], alpha);
        } else if (timeElapsed >= fadeOutStart && timeElapsed < fadeOutEnd) {
            float alpha = Mathf.Lerp(1f, 0f, (timeElapsed - fadeOutStart) / fadeDuration);
            SetImageAlpha(cutsceneImages[currentImageIndex], alpha);
        } else if (timeElapsed >= fadeOutEnd) {
            cutsceneImages[currentImageIndex].gameObject.SetActive(false);
            currentImageIndex++;
            timeElapsed = 0f; 

            if (currentImageIndex < cutsceneImages.Length) {
                cutsceneImages[currentImageIndex].gameObject.SetActive(true);
            } else {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    void SetImageAlpha(Image image, float alpha) {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}