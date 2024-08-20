using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControls : InputChangeObject
{
    [SerializeField] private Transform _pcControls;
    [SerializeField] private Transform _playstationControls;
    
    void OnEnable()
    {
        OnInputChange(ControlModeDetection.ControlMode);
    }
    
    public override void OnInputChange(ControlMode mode)
    {
        _pcControls.gameObject.SetActive(mode == ControlMode.KeyboardMouse);
        _playstationControls.gameObject.SetActive(mode == ControlMode.Playstation);
    }
}
