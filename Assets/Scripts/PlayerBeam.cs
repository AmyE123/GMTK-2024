using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private float _aimStartDistance = 0.5f;
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private float maxRayDistance = 100f;

    private LineRenderer lineRenderer;
    private Vector3 _aimDirection = Vector3.right;
    private ScalableObject lastScalableObjectHit = null;

    void Start()
    {
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
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, firingPoint);

        Vector3 currentDirection = _aimDirection;
        Vector3 currentOrigin = firingPoint;
        int reflectionsRemaining = maxReflectionCount;

        ScalableObject currentScalableObjectHit = null;

        while (reflectionsRemaining > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, currentDirection, maxRayDistance);

            if (hit.collider != null)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
                if (hitObject != null)
                {
                    hitObject.HitWithRay(hit.point, currentDirection, hit.normal);
                }

                if (hit.collider.CompareTag("Mirror"))
                {
                    currentDirection = Vector2.Reflect(currentDirection, hit.normal);
                    currentOrigin = hit.point;
                    reflectionsRemaining--;
                }
                else
                {
                    if (hit.collider.CompareTag("Scalable"))
                    {
                        currentScalableObjectHit = hit.collider.GetComponent<ScalableObject>();
                        if (currentScalableObjectHit != null)
                        {
                            currentScalableObjectHit.SetScaleAnchor(hit.point);
                        }
                    }
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentOrigin + currentDirection * maxRayDistance);
                break;
            }
        }

        if (lastScalableObjectHit != null && lastScalableObjectHit != currentScalableObjectHit)
        {
            lastScalableObjectHit.ResetScaleAnchor();
        }

        lastScalableObjectHit = currentScalableObjectHit;
    }
}