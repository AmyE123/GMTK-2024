using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;

    private Rigidbody2D _rigidBody;
    private Vector3 _scaleAnchorPoint;
    private bool _anchorSet = false;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetScaleAnchor(Vector2 hitPoint)
    {
        _scaleAnchorPoint = hitPoint;
        _anchorSet = true;
        Debug.Log($"Anchor set at: {_scaleAnchorPoint}");
    }

    private void Update()
    {
        if (_anchorSet)
        {
            if (Input.GetMouseButton(0))
            {
                ScaleObject(properties.scaleSpeed * Time.deltaTime);
            }
            else if (Input.GetMouseButton(1))
            {
                ScaleObject(-properties.scaleSpeed * Time.deltaTime);
            }
        }
    }

    private void ScaleObject(float scaleChange)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 newScale = originalScale + new Vector3(scaleChange, scaleChange, scaleChange);

        // Limit scale to within min and max bounds
        if (newScale.x < properties.minScale.x || newScale.x > properties.maxScale.x)
            return;

        // Calculate the scale ratio
        Vector3 scaleRatio = new Vector3(newScale.x / originalScale.x, newScale.y / originalScale.y, newScale.z / originalScale.z);

        // Adjust the position so that the anchor point remains fixed
        Vector3 anchorOffset = transform.position - _scaleAnchorPoint;
        anchorOffset.Scale(scaleRatio);
        Vector3 newPosition = _scaleAnchorPoint + anchorOffset;

        // Apply the new scale and position
        transform.localScale = newScale;
        transform.position = newPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_anchorSet)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_scaleAnchorPoint, 0.1f);
            Debug.Log($"Drawing Gizmo at anchor: {_scaleAnchorPoint}");
        }
    }
#endif
}