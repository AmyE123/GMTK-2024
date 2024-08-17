#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class ScalableObject : BeamObject
{
    [SerializeField] private ScalableObjectProperties properties;

    private Player _player;
    private float _currentMass;
    private Rigidbody2D _rigidBody;

    // Initial Values
    private float _initialMass;
    private Vector3 _initialScale;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _initialMass = _rigidBody.mass;
        _initialScale = transform.localScale;
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        Vector3 anchorWorldPosition = CalculateClosestEdgeAnchor();
    }

    public override void HitWithRay(Vector2 point, Vector2 direction) {

    }

    private void OnMouseOver()
    {
        float initialScale = transform.localScale.x;

        if (Input.GetMouseButton(0))
        {
            if (initialScale <= properties.maxScale.x)
            {
                float newScale = initialScale + Time.deltaTime * properties.scaleSpeed;
                ScaleObject(newScale);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (initialScale >= properties.minScale.x)
            {
                float newScale = initialScale - Time.deltaTime * properties.scaleSpeed;
                ScaleObject(newScale);
            }
        }
    }

    private void ScaleObject(float newScale)
    {
        Vector3 anchorWorldPositionBeforeScaling = CalculateClosestEdgeAnchor();
        transform.localScale = new Vector3(newScale, newScale, newScale);
        Vector3 anchorWorldPositionAfterScaling = CalculateClosestEdgeAnchor();
        Vector3 positionDelta = anchorWorldPositionBeforeScaling - anchorWorldPositionAfterScaling;
        _rigidBody.MovePosition(_rigidBody.position + new Vector2(positionDelta.x, positionDelta.y));
        _currentMass = _initialMass * Mathf.Pow(newScale, 3);
    }

    private Vector3 CalculateClosestEdgeAnchor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Bounds bounds = spriteRenderer.bounds;
        Vector3 playerPosition = _player.transform.position;

        Vector3 topEdge = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
        Vector3 bottomEdge = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        Vector3 leftEdge = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
        Vector3 rightEdge = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);

        float distanceToTop = Vector3.Distance(playerPosition, topEdge);
        float distanceToBottom = Vector3.Distance(playerPosition, bottomEdge);
        float distanceToLeft = Vector3.Distance(playerPosition, leftEdge);
        float distanceToRight = Vector3.Distance(playerPosition, rightEdge);

        float minDistance = Mathf.Min(distanceToTop, distanceToBottom, distanceToLeft, distanceToRight);

        if (minDistance == distanceToTop)
        {
            return topEdge;
        }
        else if (minDistance == distanceToBottom)
        {
            return bottomEdge;
        }
        else if (minDistance == distanceToLeft)
        {
            return leftEdge;
        }        
        else
        {
            return rightEdge;
        }            
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_player == null)
        {         
            _player = (Player)FindAnyObjectByType(typeof(Player));
        }

        if (_player != null)
        {
            Vector3 playerPosition = _player.transform.position;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Bounds bounds = spriteRenderer.bounds;

            Vector3 topEdge = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            Vector3 bottomEdge = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            Vector3 leftEdge = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
            Vector3 rightEdge = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);

            Handles.color = Color.green;
            Handles.DrawLine(playerPosition, topEdge);
            Handles.DrawLine(playerPosition, bottomEdge);
            Handles.DrawLine(playerPosition, leftEdge);
            Handles.DrawLine(playerPosition, rightEdge);

            Vector3 anchorWorldPosition = CalculateClosestEdgeAnchor();
            Handles.color = Color.red;
            Handles.DrawSolidDisc(anchorWorldPosition, Vector3.forward, 0.1f);
        }
    }
#endif
}