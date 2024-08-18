using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform _barRect;
    
    private PlayerBeam _beam;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_beam == null)
            _beam = FindObjectOfType<PlayerBeam>();

        if (_beam == null)
            return;

        if (_beam.MaxScale == 0)
            return;
        
        _barRect.localScale = new Vector3(_beam.ScaleMeter / _beam.MaxScale, 1, 1);
    }
}
