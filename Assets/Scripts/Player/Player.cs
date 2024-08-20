using UnityEngine;
using UnityEngine.InputSystem;

[System.Flags]
public enum ButtonsPressed
{
    None = 0,
    Shrink = 1,
    Grow = 2
}

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumpPressed;
    PersonMovement _movement;
    private PlayerBeam _beam;

    public static bool PressingGrow => ButtonsPressed.HasFlag(ButtonsPressed.Grow);
    public static bool PressingShrink => ButtonsPressed.HasFlag(ButtonsPressed.Shrink);
    private static ButtonsPressed ButtonsPressed;

    private ButtonsPressed _warningsGiven;

    [Header("Eye Animations")]
    [SerializeField] private Transform[] _eyes;
    [SerializeField] private float[] _eyeMoveMultiplier = new float[] { 0.1f, 0.1f };
    
    // Top left, bottom left, top right, bottom right
    [SerializeField] private Transform[] _eyelids = new Transform[4];
    [SerializeField] private float[] _openPositions = new float[4];
    [SerializeField] private float[] _blinkPositions = new float[4];
    [SerializeField] private float[] _squintPositions = new float[4];
    
    private Vector3[] _eyeStartPos = new Vector3[2];
    
    void Start()
    {
        _movement = GetComponent<PersonMovement>();
        _input = new();
        _input.Enable();

        _input.Player.Jump.performed += JumpPressed;
        _input.Player.Interact.performed += InteractPressed;
        
        _eyeStartPos[0] = _eyes[0].localPosition;
        _eyeStartPos[1] = _eyes[1].localPosition;
        _beam = GetComponent<PlayerBeam>();
    }

    public void InitLevel(LevelContainer level)
    {
        _beam = GetComponent<PlayerBeam>();
        _beam.ResetScaleMeter(level.StartingScaleMeter, level.MaximumScaleMeter);
        GetComponent<ScalableObject>().Init(level.MinimumPlayerScale, level.MaximumPlayerScale);
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        for (int i = 0; i < 2; i++)
        {
            _eyes[i].localPosition = _eyeStartPos[i] + (aimDirection * _eyeMoveMultiplier[i]);
        }
    }

    private void Update()
    {
        HandlePlayerInput();
        HandleEyelidPositions();
        HandleGrowWarning();
        HandleShrinkWarning();
    }

    private void HandleGrowWarning()
    {
        if (_warningsGiven.HasFlag(ButtonsPressed.Grow))
        {
            if (PressingGrow == false)
            {
                _warningsGiven &= ~ButtonsPressed.Grow;
            }
        }
        else if (PressingGrow && _beam.ScaleMeter <= 0.01f)
        {
            var tb = W2C.InstantiateAs<TextBurst>(Resources.Load<GameObject>("TextBurst"));
            tb.SetPosition(transform.position);
            tb.SetText("Out of Shlorp!");
            _warningsGiven |= ButtonsPressed.Grow;
        }
    }
    
    private void HandleShrinkWarning()
    {
        if (_warningsGiven.HasFlag(ButtonsPressed.Shrink))
        {
            if (PressingShrink == false)
            {
                _warningsGiven &= ~ButtonsPressed.Shrink;
            }
        }
        else if (PressingShrink && _beam.ScaleMeter >= _beam.MaxScale * 0.99f)
        {
            var tb = W2C.InstantiateAs<TextBurst>(Resources.Load<GameObject>("TextBurst"));
            tb.SetPosition(transform.position);
            tb.SetText("Shlorp is full!");
            _warningsGiven |= ButtonsPressed.Shrink;
        }
    }

    private float _timeUntilBlink = 1;
    private float _minBlinkTime = 3;
    private float _maxBlinkTime = 6;
    private float _timeToBlink = 1.1f;

    private void HandleEyelidPositions()
    {
        if (PressingGrow || PressingShrink)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 lpos = _eyelids[i].localPosition;
                lpos.y = Mathf.Lerp(lpos.y, _squintPositions[i], 20 * Time.deltaTime);
                _eyelids[i].localPosition = lpos;
            }

            if (_timeUntilBlink < 0)
                _timeUntilBlink = 1;
            
            return;
        }
        
        _timeUntilBlink -= Time.deltaTime;

        if (_timeUntilBlink < 0)
        {
            // Is this the most obtuse way possible of doing this? yes, probably, leave me alone
            float leftBlinkPercent = Mathf.Clamp01(-_timeUntilBlink / (_timeToBlink / 2)) * 2;
            leftBlinkPercent = Mathf.PingPong(leftBlinkPercent, 1) * 1.25f;
            
            Vector3 tl = _eyelids[0].localPosition;
            Vector3 bl = _eyelids[1].localPosition;

            tl.y = Mathf.Lerp(_openPositions[0], _blinkPositions[0], leftBlinkPercent);
            bl.y = Mathf.Lerp(_openPositions[1], _blinkPositions[1], leftBlinkPercent);

            _eyelids[0].localPosition = tl;
            _eyelids[1].localPosition = bl;
            
            float rightBlinkPercent = Mathf.Clamp01((-_timeToBlink * 0.3f)-_timeUntilBlink / (_timeToBlink / 2)) * 2;
            rightBlinkPercent = Mathf.PingPong(rightBlinkPercent, 1) * 1.25f;
            
            Vector3 tr = _eyelids[2].localPosition;
            Vector3 br = _eyelids[3].localPosition;

            tr.y = Mathf.Lerp(_openPositions[2], _blinkPositions[2], rightBlinkPercent);
            br.y = Mathf.Lerp(_openPositions[3], _blinkPositions[3], rightBlinkPercent);

            _eyelids[2].localPosition = tr;
            _eyelids[3].localPosition = br;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 lpos = _eyelids[i].localPosition;
                lpos.y = Mathf.Lerp(lpos.y, _openPositions[i], 12 * Time.deltaTime);
                _eyelids[i].localPosition = lpos;
            }
        }

        if (_timeUntilBlink < -_timeToBlink)
        {
            _timeUntilBlink = Random.Range(_minBlinkTime, _maxBlinkTime);
        }
    }


    // Input Event
    private void JumpPressed(InputAction.CallbackContext ctx)
    {
        
        _jumpPressed = true;
    }

    // Input Event
    private void InteractPressed(InputAction.CallbackContext ctx)
    {

    }

    public void EnableSpaceMode()
    {
        _isSpaceMode = true;
    }
    
    private bool _isSpaceMode;
    
    private void HandlePlayerInput()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();
        _lookInput = _input.Player.Look.ReadValue<Vector2>();
        ButtonsPressed = ButtonsPressed.None;

        if (_input.Player.Shrink.IsPressed())
            ButtonsPressed |= ButtonsPressed.Shrink;

        if (_input.Player.Grow.IsPressed() && _isSpaceMode == false)
            ButtonsPressed |= ButtonsPressed.Grow;

        _movement.SetDesiredMove(_moveInput.x);
        if (_isSpaceMode == false)
            _movement.SetJumpRequested(_jumpPressed);

        if (_lookInput.magnitude > 0.15f)
            _beam.SetLookDirection(_lookInput);

        _jumpPressed = false;
    }
}