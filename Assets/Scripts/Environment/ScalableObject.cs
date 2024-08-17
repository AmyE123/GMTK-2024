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

    /// <summary>
    /// Set the scaling anchor to the hit point of the beam
    /// </summary>
    /// <param name="hitPoint">The hitpoint of the beam</param>
    public void SetScaleAnchor(Vector2 hitPoint)
    {
        _scaleAnchorPoint = hitPoint;
        _anchorSet = true;
    }

    /// <summary>
    /// Reset the scaling anchor when it isn't hitting a scalable object
    /// </summary>
    public void ResetScaleAnchor()
    {
        _scaleAnchorPoint = Vector3.zero;
        _anchorSet = false;
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

        if (newScale.x < properties.minScale.x || newScale.x > properties.maxScale.x)
        {
            return;
        }
            
        Vector3 scaleRatio = new Vector3(newScale.x / originalScale.x, newScale.y / originalScale.y, newScale.z / originalScale.z);
        Vector3 anchorOffset = transform.position - _scaleAnchorPoint;
        anchorOffset.Scale(scaleRatio);
        Vector3 newPosition = _scaleAnchorPoint + anchorOffset;

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
        }
    }
#endif
}