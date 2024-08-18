using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private float _aimStartDistance = 0.5f;
    [SerializeField] private float maxRayDistance = 100f;
    [SerializeField] private LineRenderer lineRenderer;
    
    private Vector3 _aimDirection = Vector3.right;

    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        GetAimDirectionFromMouse();
        UpdateBeam();
    }

    private void GetAimDirectionFromMouse()
    {
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
        Vector3 firingPoint = transform.position + (_aimDirection * _aimStartDistance);
        
        RaycastHit2D firstHit = Physics2D.Raycast(transform.position, _aimDirection, _aimStartDistance);
        
        if (firstHit.collider != null)
        {
            // If there is a collision between the players body and their hand, do not fire a ray, they're in a wall
            lineRenderer.enabled = false;
            return;
        }
    
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firingPoint);

        Vector3 currentDirection = _aimDirection;

        RaycastHit2D hit = Physics2D.Raycast(firingPoint, currentDirection, maxRayDistance);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null)
            {
                hitObject.HitWithRay(hit.point, currentDirection, hit.normal);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, firingPoint + (_aimDirection * 999));
        }
    }
}