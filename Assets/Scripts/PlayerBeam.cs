using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private float _aimStartDistance = 0.5f;
    [SerializeField] private float maxRayDistance = 100f;
    [SerializeField] private LineRenderer lineRenderer;
    
    public float ScaleMeter { get; private set; }
    public float MaxScale { get; private set; }
    
    private Vector3 _aimDirection = Vector3.right;

    [SerializeField] private LayerMask _hitMask;

    public void ResetScaleMeter(float starting, float maximum)
    {
        ScaleMeter = starting;
        MaxScale = maximum;
    }

    public void UseUp(float amount)
    {
        ScaleMeter -= amount;
    }

    public float ClampAmountCanUse(float amount)
    {
        // Because we're using this up, we need to reverse it
        amount = -amount;
        
        if (ScaleMeter + amount < 0)
        {
            amount = -ScaleMeter;
        }
        if (ScaleMeter + amount > MaxScale)
        {
            amount = MaxScale - ScaleMeter;
        }

        return -amount;
    }

    void Start()
    {
        ResetScaleMeter(10, 20f);
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
        
        RaycastHit2D firstHit = Physics2D.Raycast(transform.position, _aimDirection, _aimStartDistance, _hitMask);
        
        if (firstHit.collider != null)
        {
            // If there is a collision between the players body and their hand, hide the ray
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
    
        lineRenderer.SetPosition(0, firingPoint);

        Vector3 currentDirection = _aimDirection;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, maxRayDistance, _hitMask);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null)
            {
                hitObject.HitWithRay(hit.point, currentDirection, hit.normal, this);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, firingPoint + (_aimDirection * 999));
        }
    }
}