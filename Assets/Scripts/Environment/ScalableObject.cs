using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;

    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
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
    }
}