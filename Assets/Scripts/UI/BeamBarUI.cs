using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private Text _text;
    [SerializeField] private RectTransform _referenceRect;
    
    private PlayerBeam _beam;

    // Update is called once per frame
    void Update()
    {
        if (_beam == null)
            _beam = FindObjectOfType<PlayerBeam>();

        if (_beam == null)
            return;

        if (_beam.MaxScale == 0)
            return;

        float percent = Mathf.Clamp01(_beam.ScaleMeter / _beam.MaxScale);
        _barRect.sizeDelta = new Vector2(_referenceRect.rect.width * percent, _barRect.sizeDelta.y);
        _text.text = $"{Mathf.RoundToInt(percent * 100)}%";
    }
}
