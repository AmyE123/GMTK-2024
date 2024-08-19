using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private float _aimStartDistance = 0.5f;
    [SerializeField] private float maxRayDistance = 100f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material _lineMaterial;
    [SerializeField] private Player _player;

    // Player sound effect
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioClip _beamAudioClip;

    public float ScaleMeter { get; private set; } = 0.5f;
    public float MaxScale { get; private set; } = 1.0f;
    private bool _useMouse = false;
    private Vector3 _lastMousePos;
    
    [SerializeField] private HandAnimation _handAnimation;
    
    private Vector3 _aimDirection = Vector3.right;

    [SerializeField] private LayerMask _hitMask;

    public void SetLookDirection(Vector2 direction)
    {
        _aimDirection = direction.normalized;
        _useMouse = false;
    }
    
    public void ResetScaleMeter(float starting, float maximum)
    {
        ScaleMeter = starting;
        MaxScale = maximum;
    }

    public void UseUp(float amount)
    {
        ScaleMeter -= amount;
    }

    public float ClampAmountCanUse(float amount)
    {
        // Because we're using this up, we need to reverse it
        amount = -amount;
        
        if (ScaleMeter + amount < 0)
        {
            amount = -ScaleMeter;
        }
        if (ScaleMeter + amount > MaxScale)
        {
            amount = MaxScale - ScaleMeter;
        }

        return -amount;
    }

    void Start()
    {
        // Yes, this does not belong here
        Application.targetFrameRate = 60;
        lineRenderer.positionCount = 2;
        _lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (_lastMousePos != Input.mousePosition)
            _useMouse = true;
        
        if (_useMouse)
            GetAimDirectionFromMouse();
    
        UpdateBeam();
        UpdateBeamAnimation();
        
        _player.SetAimDirection(_aimDirection);
    }

    private float _slurpOffset = 0;

    private void UpdateBeamAnimation()
    {
        float targetVolume = 0f;

        if (Player.PressingGrow)
        {
            targetVolume = 1f;
            _slurpOffset -= Time.deltaTime * 2;
            _handAnimation.SetGrowing();
        }
        else if (Player.PressingShrink)
        {
            targetVolume = 1f;
            _slurpOffset += Time.deltaTime * 2;
            _handAnimation.SetShrinking();
        }

        _playerAudioSource.volume = Mathf.Lerp(_playerAudioSource.volume, targetVolume, Time.deltaTime * 5f);
        _lineMaterial.SetTextureOffset("_MainTex", Vector2.right * _slurpOffset);
    }

    private void GetAimDirectionFromMouse()
    {
        _lastMousePos = Input.mousePosition;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.z = 0;

        Vector3 differenceVector = targetPosition - transform.position;

        if (differenceVector.magnitude > 0)
        {
            _aimDirection = differenceVector.normalized;
        }
    }

    void UpdateBeam()
    {
        Vector3 firingPoint = transform.position + (_aimDirection * _aimStartDistance * transform.localScale.x);

        RaycastHit2D firstHit = Physics2D.Raycast(transform.position, _aimDirection, _aimStartDistance * transform.localScale.x, _hitMask);
        _handAnimation.SetPosition(firingPoint, _aimDirection);
        lineRenderer.SetPosition(0, firingPoint);

        if (firstHit.collider != null)
        {
            // If there is a collision between the players body and their hand, hide the ray
            lineRenderer.enabled = false;
            lineRenderer.SetPosition(1, firstHit.point);
            _handAnimation.SetPosition(firstHit.point, _aimDirection);


            BeamObject hitObject = firstHit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null)
            {
                hitObject.HitWithRay(firstHit.point, _aimDirection, firstHit.normal, this);
            }

            return;
        }
        
        lineRenderer.enabled = true;


        RaycastHit2D hit = Physics2D.Raycast(firingPoint, _aimDirection, maxRayDistance, _hitMask);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null)
            {
                hitObject.HitWithRay(hit.point, _aimDirection, hit.normal, this);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, firingPoint + (_aimDirection * 999));
        }
    }
}