using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlMode
{
    KeyboardMouse,
    Playstation
};

public class ControlModeDetection : MonoBehaviour
{
    public static ControlMode ControlMode { get; private set; }
    
    private static ControlModeDetection _instance;
    private InputSystem_Actions _input;
    private Vector3 _lastMousePos;

    
    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        _input = new();
        _input.Enable();
        
        _input.Player.Jump.performed += ButtonPressed;
        _input.Player.Shrink.performed += ButtonPressed;
        _input.Player.Grow.performed += ButtonPressed;
        _input.Player.Move.performed += JoystickUsed;
        _input.Player.Look.performed += JoystickUsed;
        _lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        if (ControlMode != ControlMode.KeyboardMouse)
        {
            if (_lastMousePos != Input.mousePosition)
            {
                ChangeMode(ControlMode.KeyboardMouse);
            }
        }
        
        _lastMousePos = Input.mousePosition;
    }

    private void ChangeMode(ControlMode mode)
    {
        ControlMode = mode;

        foreach (InputChangeObject obj in FindObjectsOfType<InputChangeObject>())
        {
            obj.OnInputChange(ControlMode);
        }
    }
    
    private void ButtonPressed(InputAction.CallbackContext ctx)
    {
        ControlMode conType = GetInputType(ctx.control.device);
        
        if (conType != ControlMode)
        {
            ChangeMode(conType);
        }
    }

    private void JoystickUsed(InputAction.CallbackContext ctx)
    {
        Vector2 vec = ctx.ReadValue<Vector2>();
        if (vec.magnitude > 0.2f)
        {
            ButtonPressed(ctx);
        }
    }

    private ControlMode GetInputType(InputDevice dev)
    {
        if (dev.description.deviceClass == "Keyboard" || dev.description.deviceClass == "Mouse")
            return ControlMode.KeyboardMouse;

        if (dev.description.manufacturer == "Sony Interactive Entertainment")
            return ControlMode.Playstation;

        if (dev.description.manufacturer == "Nintendo")
            return ControlMode.Playstation;
        
        if (dev.description.interfaceName == "XInput")
            return ControlMode.Playstation;

        return ControlMode.Playstation;
    }

}
