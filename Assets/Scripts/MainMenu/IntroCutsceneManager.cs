using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections; // Include for Coroutine

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField] private ParallaxImage[] _cutsceneParallaxImages;
    [SerializeField] private Image[] _cutsceneImages;
    [SerializeField] private TextMeshProUGUI _voSubtitleText;
    [SerializeField] private TextMeshProUGUI _skipText;
    [SerializeField] private AudioSource _voAudio;
    [SerializeField] private float _fadeDuration = 0.5f;

    private int _currentImageIndex = 0;
    private float _currentTimer = 0f;
    private Vector3 _initialCameraPosition;

    void Start()
    {
        _initialCameraPosition = Camera.main.transform.position;
        DisplayCurrentImage();
        StartCoroutine(FadeInText(_voSubtitleText));
        StartCoroutine(FadeInText(_skipText));
    }

    void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_currentImageIndex < _cutsceneParallaxImages.Length)
        {
            ParallaxImage currentCutscene = _cutsceneParallaxImages[_currentImageIndex];
            float fadeOutStart = currentCutscene.displayDuration;
            float fadeOutEnd = fadeOutStart + _fadeDuration;

            if (_currentTimer >= fadeOutStart && _currentTimer < fadeOutEnd)
            {
                _currentImageIndex++;
                _currentTimer = 0f;

                if (_currentImageIndex < _cutsceneParallaxImages.Length)
                {
                    DisplayCurrentImage();
                }
                else
                {
                    SceneManager.LoadScene("GameScene");
                }
            }

            float zoomAmount = Mathf.Lerp(currentCutscene.initialZoom, currentCutscene.finalZoom, _currentTimer / fadeOutStart);
            ScaleRectTransforms(zoomAmount);
            Vector3 panOffset = Vector3.Lerp(Vector3.zero, new Vector3(currentCutscene.panAmount.x * zoomAmount, currentCutscene.panAmount.y * zoomAmount, 0), _currentTimer / fadeOutStart);
            ApplyParallaxEffect(panOffset);
        }
    }

    IEnumerator FadeInText(TextMeshProUGUI textElement)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);
            textElement.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        textElement.color = new Color(1f, 1f, 1f, 1f); // Ensure it stays fully visible
    }

    void DisplayCurrentImage()
    {
        if (_currentImageIndex >= _cutsceneParallaxImages.Length) return;

        ParallaxImage currentParallaxImage = _cutsceneParallaxImages[_currentImageIndex];
        for (int i = 0; i < _cutsceneImages.Length; i++)
        {
            _cutsceneImages[i].gameObject.SetActive(i < currentParallaxImage.layers.Length);
            if (i < currentParallaxImage.layers.Length)
            {
                _cutsceneImages[i].sprite = currentParallaxImage.layers[i].image;
                _cutsceneImages[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }

        if (_voAudio != null && currentParallaxImage.audioClip != null)
        {
            _voAudio.clip = currentParallaxImage.audioClip;
            _voAudio.Play();
        }

        _voSubtitleText.text = currentParallaxImage.subtitleText;
    }

    void ScaleRectTransforms(float scaleFactor)
    {
        foreach (Image image in _cutsceneImages)
        {
            if (image.gameObject.activeSelf)
            {
                image.rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
        }
    }

    void ApplyParallaxEffect(Vector3 panOffset)
    {
        for (int i = 0; i < _cutsceneImages.Length; i++)
        {
            if (i < _cutsceneParallaxImages[_currentImageIndex].layers.Length)
            {
                Vector3 parallaxOffset = _cutsceneParallaxImages[_currentImageIndex].GetParallaxOffset(_initialCameraPosition + panOffset, i);
                _cutsceneImages[i].rectTransform.localPosition = parallaxOffset;
            }
        }
    }
}