using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMassUI : W2C
{
    [SerializeField] private Rigidbody2D _target;
    [SerializeField] private Text _text;
    
    public void Init(Rigidbody2D target)
    {
        _target = target;
        SetPosition(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _target.mass.ToString(".01") + "kg";
    }
}
