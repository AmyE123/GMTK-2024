using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;
    [SerializeField] private bool _showMass;
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

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        float scaleSquared = CurrentScale * CurrentScale;
        _massForUnitScale = _rigidBody.mass / scaleSquared;
        StartScale = transform.localScale.x;

        // It's a gamejam, I'll use Resources.Load if I want to!
        var scaleUI = Resources.Load<ScaleItemUI>("ScaleUI");
        var instantiatedUI = W2C.InstantiateAs<ScaleItemUI>(scaleUI.gameObject);
        instantiatedUI.Init(this);

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
        
        if (Input.GetMouseButton(0) && beam.ScaleMeter > 0)
        {
            ScaleObject(point, 1, beam);
        }
        else if (Input.GetMouseButton(1) && beam.ScaleMeter < beam.MaxScale)
        {
            ScaleObject(point, -1, beam);
        }
    }

    private void ScaleObject(Vector3 anchorPoint, int scaleMultiplier, PlayerBeam beam)
    {
        float scaleChange = properties.scaleSpeed * scaleMultiplier * Time.deltaTime * _scaleSpeedMultiplier;

        // Clamp the scale by what is available
        scaleChange = beam.ClampAmountCanUse(scaleChange);

        if (transform.localScale.x + scaleChange > _maxScale)
        {
            scaleChange = _maxScale - transform.localScale.x;
        }

        if (transform.localScale.x + scaleChange < _minScale)
        {
            scaleChange = -(transform.localScale.x - _minScale);
        }
        
        beam.UseUp(scaleChange);

        if (scaleChange == 0)
            return;
        
        Vector3 originalScale = transform.localScale;
        Vector3 newScale = originalScale + new Vector3(scaleChange, scaleChange, scaleChange);

        Vector3 scaleRatio = new Vector3(newScale.x / originalScale.x, newScale.y / originalScale.y, newScale.z / originalScale.z);
        Vector3 anchorOffset = transform.position - anchorPoint;
        anchorOffset.Scale(scaleRatio);
        Vector3 newPosition = anchorPoint + anchorOffset;
        
        transform.localScale = newScale;
        _rigidBody.MovePosition(newPosition);
        _rigidBody.mass = CurrentScale * CurrentScale * _massForUnitScale;
    }
}