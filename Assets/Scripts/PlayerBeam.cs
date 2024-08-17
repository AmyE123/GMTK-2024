using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private float _aimStartDistance = 0.5f;
    
    private LineRenderer lineRenderer;
    private Vector3 _aimDirection = Vector3.right;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
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
            // if we hit something before we've even reached out minimum distance, don't do the ray
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firingPoint);
        
        RaycastHit2D hit = Physics2D.Raycast(firingPoint, _aimDirection);

        if (hit.collider != null) 
        {
            lineRenderer.SetPosition(1, hit.point);
            
            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null)
            {
                hitObject.HitWithRay (hit.point, _aimDirection, hit.normal);
            }
        } 
        else 
        {
            lineRenderer.SetPosition(1, firingPoint + (_aimDirection * 999));
        }
    }
}