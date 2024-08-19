using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextBurst : W2C
{
    [SerializeField] Vector2 _driftAmount;
    [SerializeField] RectTransform _rect;
        
    [Header("Timings")]
    [SerializeField] float _scaleTime;
    [SerializeField] float _aliveTime;
    [SerializeField] float _fadeTime;

    [Space(8)]
    [SerializeField] Text _text;
    [SerializeField] float _randomOffset;

    void Start()
    {
        _rect.localScale = Vector3.zero;
        _rect.DOScale(1, _scaleTime).SetEase(Ease.OutBack);

        _rect.DOAnchorPos(_rect.anchoredPosition + _driftAmount, _aliveTime)
            .SetEase(Ease.Linear);

        _canvasGroup.DOFade(0, _fadeTime)
            .SetDelay(_aliveTime - _fadeTime)
            .SetEase(Ease.Linear);

        Destroy(gameObject, _aliveTime + 0.5f);           
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
