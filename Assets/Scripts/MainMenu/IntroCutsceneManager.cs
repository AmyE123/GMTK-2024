using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class IntroCutsceneManager : MonoBehaviour
{
    // Cutscene params
    [SerializeField] private ParallaxImage[] _cutsceneParallaxImages;
    [SerializeField] private Image[] _cutsceneImages;
    [SerializeField] private TextMeshProUGUI _voSubtitleText;
    [SerializeField] private AudioSource _voAudio;
    [SerializeField] private float _fadeDuration = 0.5f;

    private int _currentImageIndex = 0;
    private float _currentTimer = 0f;
    private Vector3 _initialCameraPosition;

    void Start()
    {
        _initialCameraPosition = Camera.main.transform.position;
        DisplayCurrentImage();
    }

    void Update()
    {
        _currentTimer += Time.deltaTime;

        ParallaxImage currentCutscene = _cutsceneParallaxImages[_currentImageIndex];       
        float fadeInEnd = _fadeDuration;
        float fadeOutStart = fadeInEnd + currentCutscene.displayDuration;
        float fadeOutEnd = fadeOutStart + _fadeDuration;

        if (_currentTimer < fadeInEnd)
        {
            float alpha = Mathf.Lerp(0f, 1f, _currentTimer / _fadeDuration);
            SetLayerAlpha(alpha, _fadeDuration);
        }
        else if (_currentTimer >= fadeOutStart && _currentTimer < fadeOutEnd)
        {
            float alpha = Mathf.Lerp(1f, 0f, (_currentTimer - fadeOutStart) / _fadeDuration);
            SetLayerAlpha(alpha, _fadeDuration);
        }
        else if (_currentTimer >= fadeOutEnd)
        {
            _currentImageIndex++;
            _currentTimer = 0f;

            if (_currentImageIndex < _cutsceneParallaxImages.Length)
            {
                DisplayCurrentImage();
            }
            else
            {
                // TODO: This is prone to error
                SceneManager.LoadScene("GameScene");
            }
        }

        if (_currentImageIndex < _cutsceneParallaxImages.Length)
        {
            float zoomAmount = Mathf.Lerp(currentCutscene.initialZoom, currentCutscene.finalZoom, _currentTimer / (fadeInEnd + currentCutscene.displayDuration));
            ScaleRectTransforms(zoomAmount);

            Vector3 panOffset = Vector3.Lerp(
                Vector3.zero,
                new Vector3(currentCutscene.panAmount.x * zoomAmount, currentCutscene.panAmount.y * zoomAmount, 0),
                _currentTimer / (fadeInEnd + currentCutscene.displayDuration)
            );

            ApplyParallaxEffect(panOffset);
        }
    }

    void DisplayCurrentImage()
    {
        ParallaxImage currentParallaxImage = _cutsceneParallaxImages[_currentImageIndex];

        for (int i = 0; i < _cutsceneImages.Length; i++)
        {
            if (i < currentParallaxImage.layers.Length)
            {
                _cutsceneImages[i].sprite = currentParallaxImage.layers[i].image;
                _cutsceneImages[i].color = new Color(1f, 1f, 1f, 0f); // start transparent
                _cutsceneImages[i].gameObject.SetActive(true);
            }
            else
            {
                _cutsceneImages[i].gameObject.SetActive(false);
            }
        }

        // Play audio vo
        if (_voAudio != null && currentParallaxImage.audioClip != null)
        {
            _voAudio.clip = currentParallaxImage.audioClip;
            _voAudio.Play();
        }

        // Show subtitle text
        if (_voSubtitleText != null)
        {
            _voSubtitleText.text = currentParallaxImage.subtitleText;
        }
    }

    void SetLayerAlpha(float alpha, float fadeDuration)
    {
        ParallaxImage currentParallaxImage = _cutsceneParallaxImages[_currentImageIndex];

        for (int i = 0; i < _cutsceneImages.Length; i++)
        {
            if (i < currentParallaxImage.layers.Length)
            {
                float layerAlpha = alpha;

                // Background should fade in faster
                if (i == 0)
                {
                    float backgroundFadeDuration = 0.5f * fadeDuration;
                    layerAlpha = Mathf.Lerp(0f, 1f, _currentTimer / backgroundFadeDuration);
                }

                Color color = _cutsceneImages[i].color;
                color.a = layerAlpha;
                _cutsceneImages[i].color = color;
            }
        }
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
        ParallaxImage currentParallaxImage = _cutsceneParallaxImages[_currentImageIndex];

        for (int i = 0; i < _cutsceneImages.Length; i++)
        {
            if (i < currentParallaxImage.layers.Length)
            {
                Vector3 parallaxOffset = currentParallaxImage.GetParallaxOffset(_initialCameraPosition + panOffset, i);
                _cutsceneImages[i].rectTransform.localPosition = parallaxOffset;
            }
        }
    }
}