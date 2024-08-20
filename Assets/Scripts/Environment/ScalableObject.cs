using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;
    [SerializeField] private bool _showMass;
    [SerializeField] private bool _showScaleUI = true;
    [SerializeField] private float _maxScale = 3;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _scaleSpeedMultiplier = 1f;
    
    public float CurrentScale => transform.localScale.x;
    public float MaxScale => _maxScale;
    public float MinScale => _minScale;
    public float StartScale { get; private set; }
    public bool WasHighlighted => _highlightCounter > 0;
    
    private Rigidbody2D _rigidBody;
    private float _massForUnitScale;
    private int _highlightCounter;

    public void Init(float minScale, float maxScale)
    {
        _minScale = minScale;
        _maxScale = maxScale;
    }
    
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        float scaleSquared = CurrentScale * CurrentScale;
        _massForUnitScale = _rigidBody.mass / scaleSquared;
        StartScale = transform.localScale.x;

        // It's a gamejam, I'll use Resources.Load if I want to!
        if (_showScaleUI)
        {
            var scaleUI = Resources.Load<ScaleItemUI>("ScaleUI");
            var instantiatedUI = W2C.InstantiateAs<ScaleItemUI>(scaleUI.gameObject);
            instantiatedUI.Init(this);
        }

        if (_showMass)
        {
            var massUi = Resources.Load<DebugMassUI>("DebugMassUI");
            var createdMassUi = W2C.InstantiateAs<DebugMassUI>(massUi.gameObject);
            createdMassUi.Init(_rigidBody);
        }
    }

    private void Update()
    {
        if (_highlightCounter > 0)
            _highlightCounter--;
    }

    public override void HitWithRay(Vector2 point, Vector2 direction, Vector2 normal, PlayerBeam beam, int depth=0)
    {
        _highlightCounter = 2;
        
        if (Player.PressingGrow && beam.ScaleMeter > 0)
        {
            ScaleObject(point, 1, beam);
        }
        else if (Player.PressingShrink && beam.ScaleMeter < beam.MaxScale)
        {
            ScaleObject(point, -1, beam);
        }
        else if (Player.PressingShrink && beam.IsSpaceMode)
        {
            ScaleObject(point, -1, beam);

        }
    }

    private void ScaleObject(Vector3 anchorPoint, int scaleMultiplier, PlayerBeam beam)
    {
        float scaleChange = properties.scaleSpeed * scaleMultiplier * Time.fixedDeltaTime * _scaleSpeedMultiplier;

        // Clamp the scale by what is available
        scaleChange = beam.ClampAmountCanUse(scaleChange);

        if (transform.localScale.x + scaleChange >= _maxScale)
        {
            scaleChange = _maxScale - transform.localScale.x;
        }

        if (transform.localScale.x + scaleChange <= _minScale)
        {
            scaleChange = -(transform.localScale.x - _minScale);
        }
        
        if (scaleChange == 0)
            return;
        
        Vector3 originalScale = transform.localScale;
        Vector3 newScale = originalScale * (1 + scaleChange);
        
        if (transform.localScale.x + scaleChange >= _maxScale)
        {
            newScale = Vector3.one * _maxScale;
        }
        else if (transform.localScale.x + scaleChange <= _minScale)
        {
            newScale = Vector3.one * _minScale;
        }

        Vector3 originalPointOffset = transform.position - anchorPoint;
        Vector3 newPointOffset = originalPointOffset * (1 + scaleChange);
        
        _cachedBeam = beam;
        _beamUseRequest = scaleChange;
        _scaleRequest = newScale;
        _positionRequest = anchorPoint + newPointOffset;
        _massRequest = CurrentScale * CurrentScale * _massForUnitScale;
        _changeRequestMade = true;
    }

    private PlayerBeam _cachedBeam;
    private float _beamUseRequest;
    private Vector3 _scaleRequest;
    private Vector3 _positionRequest;
    private float _massRequest;
    private bool _changeRequestMade;

    private void FixedUpdate()
    {
        if (_changeRequestMade)
        {
            float scaleChange = _scaleRequest.x - transform.localScale.x;
            transform.localScale = _scaleRequest;
            _rigidBody.MovePosition(_positionRequest);
            _rigidBody.mass = _massRequest;
            _changeRequestMade = false;
            _cachedBeam.UseUp(scaleChange);
        }
    }
}