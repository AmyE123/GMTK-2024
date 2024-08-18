using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleItemUI : MonoBehaviour
{
    [SerializeField] private ScalableObject _target;
    
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image[] _bgRings;
    [SerializeField] private Image[] _fgRings;
    [SerializeField] private Image _centerDot;

    [SerializeField] private Image _centralRing;
    [SerializeField] private Image _outerRing;
    
    [SerializeField] private bool _isActive;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = GetPercentScaled();
        
        HandleActive(t);
    }

    private float GetPercentScaled()
    {
        if (_target.CurrentScale >= _target.StartScale)
        {
            return 1 + Mathf.InverseLerp(_target.StartScale, _target.MaxScale, _target.CurrentScale);
        }

        return _target.CurrentScale / _target.StartScale;
    }

    private void HandleActive(float t)
    {
        Color col = _gradient.Evaluate(t / 2);
        _centerDot.color = col;
        
        foreach (Image img in _fgRings)
        {
            img.color = col;
        }

        _centralRing.fillAmount = Mathf.Clamp01(t);
        _outerRing.fillAmount = Mathf.Clamp01(t - 1);
    }
}
