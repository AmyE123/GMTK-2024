using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _moveInput;
    private bool _jumpPressed;
    PersonMovement _movement;

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
    }

    private float _timeUntilBlink = 1;
    private float _minBlinkTime = 3;
    private float _maxBlinkTime = 6;
    private float _timeToBlink = 1.1f;

    private void HandleEyelidPositions()
    {
        // TODO input system
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
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

    private void HandlePlayerInput()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();

        _movement.SetDesiredMove(_moveInput.x);
        _movement.SetJumpRequested(_jumpPressed);

        _jumpPressed = false;
    }
}