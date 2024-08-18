using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;

    public float CurrentScale => transform.localScale.x;
    public float MaxScale => properties.maxScale.x;
    public float MinScale => properties.minScale.x;
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
    }

    private void Update()
    {
        if (_highlightCounter > 0)
            _highlightCounter--;
    }

    public override void HitWithRay(Vector2 point, Vector2 direction, Vector2 normal, int depth=0)
    {
        _highlightCounter = 2;
        
        if (Input.GetMouseButton(0))
        {
            ScaleObject(point, 1);
        }
        else if (Input.GetMouseButton(1))
        {
            ScaleObject(point, -1);
        }
    }

    private void ScaleObject(Vector3 anchorPoint, int scaleMultiplier)
    {
        float scaleChange = properties.scaleSpeed * scaleMultiplier * Time.deltaTime;
        
        Vector3 originalScale = transform.localScale;
        Vector3 newScale = originalScale + new Vector3(scaleChange, scaleChange, scaleChange);

        newScale.x = Mathf.Clamp(newScale.x, properties.minScale.x, properties.maxScale.x);
        newScale.y = Mathf.Clamp(newScale.y, properties.minScale.y, properties.maxScale.y);
        newScale.z = Mathf.Clamp(newScale.z, properties.minScale.z, properties.maxScale.z);

        Vector3 scaleRatio = new Vector3(newScale.x / originalScale.x, newScale.y / originalScale.y, newScale.z / originalScale.z);
        Vector3 anchorOffset = transform.position - anchorPoint;
        anchorOffset.Scale(scaleRatio);
        Vector3 newPosition = anchorPoint + anchorOffset;
        
        transform.localScale = newScale;
        _rigidBody.MovePosition(newPosition);
        _rigidBody.mass = CurrentScale * CurrentScale * _massForUnitScale;
    }
}