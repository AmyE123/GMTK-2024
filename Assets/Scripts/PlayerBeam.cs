using UnityEngine;

public class PlayerBeam : MonoBehaviour {

    public GameObject beamIndicatorPrefab;
    private GameObject currentBeamIndicator;
    private Collider2D playerCollider;

    void Start() {
        playerCollider = GetComponent<Collider2D>();
    }

    void Update() {
        BeamIndicator();
    }

    void BeamIndicator() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 currentOrigin = transform.position;

        bool hitSomething = false;
        int maxReflections = 5;
        int reflectionsRemaining = maxReflections;

        while (reflectionsRemaining > 0) {
            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, direction, Mathf.Infinity, ~LayerMask.GetMask("Player"));

            if (hit.collider != null) {
                hitSomething = true;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Reflective")) {
                    direction = Vector2.Reflect(direction, hit.normal);
                    currentOrigin = hit.point;
                    reflectionsRemaining--;

                    Gizmos.DrawLine(currentOrigin, hit.point);
                } else {
                    if (currentBeamIndicator != null) {
                        currentBeamIndicator.transform.position = hit.point;
                    } else {
                        currentBeamIndicator = Instantiate(beamIndicatorPrefab, hit.point, Quaternion.identity);
                    }
                    break;
                }
            } else {
                break;
            }
        }

        if (!hitSomething) {
            if (currentBeamIndicator != null) {
                currentBeamIndicator.transform.position = mousePosition;
            } else {
                currentBeamIndicator = Instantiate(beamIndicatorPrefab, mousePosition, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmos() {
        if (Camera.main == null) return;

        Gizmos.color = Color.red;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 currentOrigin = transform.position;

        int maxReflections = 5;
        int reflectionsRemaining = maxReflections;

        while (reflectionsRemaining > 0) {
            RaycastHit2D hit = Physics2D.Raycast(currentOrigin, direction, Mathf.Infinity, ~LayerMask.GetMask("Player"));

            if (hit.collider != null) {
                Gizmos.DrawLine(currentOrigin, hit.point);

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Reflective")) {
                    direction = Vector2.Reflect(direction, hit.normal);
                    currentOrigin = hit.point;
                    reflectionsRemaining--;
                } else {
                    break;
                }
            } else {
                Gizmos.DrawLine(currentOrigin, mousePosition);
                break;
            }
        }
    }
}