using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;

    public float CurrentScale => transform.localScale.x;
    public float MaxScale => properties.maxScale.x;
    public float MinScale => properties.minScale.x;
    public float StartScale { get; private set; }
    
    private Rigidbody2D _rigidBody;
    private float _massForUnitScale;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _massForUnitScale = _rigidBody.mass / CurrentScale;
        StartScale = transform.localScale.x;
    }

    public override void HitWithRay(Vector2 point, Vector2 direction, Vector2 normal, int depth=0)
    {
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
        _rigidBody.mass = transform.localScale.x * _massForUnitScale;
    }
}