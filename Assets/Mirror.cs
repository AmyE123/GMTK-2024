using UnityEngine;

public class Mirror : BeamObject
{
    private LineRenderer lineRenderer;
    private bool hitWithBeam;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void LateUpdate() {
        if (hitWithBeam) {
            hitWithBeam = false;
            return;
        }

        if (lineRenderer.enabled) {
        lineRenderer.enabled = false;
        }
    }

    public override void HitWithRay(Vector2 point, Vector2 direction) {
        lineRenderer.SetPosition(0, point);

        direction.x = -direction.x;

        RaycastHit2D hit = Physics2D.Raycast(point, direction);

        if (hit.collider != null) {
            lineRenderer.SetPosition(1, hit.point);
            Debug.DrawLine(point, hit.point, Color.red);
            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            if (hitObject != null) {
                hitObject.HitWithRay(hit.point, direction);
            }
        } else {
            lineRenderer.SetPosition(1, point + (direction * 999));
        }

        hitWithBeam = true;
        lineRenderer.enabled = true;
    }
}