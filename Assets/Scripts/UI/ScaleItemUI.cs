using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleItemUI : W2C
{
    [SerializeField] private ScalableObject _target;
    
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image[] _bgRings;
    [SerializeField] private Image[] _fgRings;
    [SerializeField] private Image _centerDot;

    [SerializeField] private Image _centralRing;
    [SerializeField] private Image _outerRing;
    
    // Start is called before the first frame update
    void Start()
    {
        Init(_target);
    }

    public void Init(ScalableObject target)
    {
        _target = target;
        SetPosition(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        float t = GetPercentScaled();
        _centralRing.fillAmount = Mathf.Clamp01(t);
        _outerRing.fillAmount = Mathf.Clamp01(t - 1);

        if (_target.WasHighlighted)
        {
            HandleActive(t);
        }
        else
        {
            HandleInactive(t);
        }
    }

    private float GetPercentScaled()
    {
        if (_target.CurrentScale >= _target.StartScale)
        {
            return 1 + Mathf.InverseLerp(_target.StartScale, _target.MaxScale, _target.CurrentScale);
        }

        return (_target.CurrentScale - _target.MinScale) / (_target.StartScale - _target.MinScale);
    }

    private void HandleActive(float t)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 12);
        Color fgCol = _gradient.Evaluate(t / 2);
        Color midCol = new Color(1, 1, 1, 1);
        Color bgCol = new Color(0, 0, 0, 0.4f);
        
        _centerDot.color = Color.Lerp(_centerDot.color, midCol, Time.deltaTime * 12);

        foreach (Image img in _fgRings)
        {
            img.color = Color.Lerp(img.color, fgCol, Time.deltaTime * 12);
        }
        
        foreach (Image img in _bgRings)
        {
            img.color = Color.Lerp(img.color, bgCol, Time.deltaTime * 12);
        }

    }

    private void HandleInactive(float t)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.8f, Time.deltaTime * 12);
        Color fgCol = new Color(1, 1, 1, 0.2f);
        Color midCol = new Color(1, 1, 1, 0.9f);
        Color bgCol = new Color(0, 0, 0, 0.0f);
        
        _centerDot.color = Color.Lerp(_centerDot.color, midCol, Time.deltaTime * 12);

        foreach (Image img in _fgRings)
        {
            img.color = Color.Lerp(img.color, fgCol, Time.deltaTime * 12);
        }
        
        foreach (Image img in _bgRings)
        {
            img.color = Color.Lerp(img.color, bgCol, Time.deltaTime * 12);
        }
    }
}
