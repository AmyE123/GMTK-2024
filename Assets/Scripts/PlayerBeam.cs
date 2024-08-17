using UnityEngine;

public class PlayerBeam : MonoBehaviour {

    public Transform firingPoint;
    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        UpdateBeam();
    }

    void UpdateBeam() {
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, firingPoint.position);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 differenceVector = targetPosition - firingPoint.position;
        differenceVector.Normalize();

        Ray2D ray = new Ray2D(firingPoint.position, differenceVector);

        RaycastHit2D hit = Physics2D.Raycast(firingPoint.position, differenceVector);

        Debug.Log(hit.collider);

        if (hit.collider != null) {
            lineRenderer.SetPosition(1, hit.point);
        } else {
            lineRenderer.SetPosition(1, firingPoint.position + (differenceVector*999));
        }
    }
}