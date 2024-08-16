using UnityEngine;

public class PlayerBeam : MonoBehaviour {

    public GameObject beamIndicatorPrefab;
    private GameObject currentBeamIndicator;
    private Collider2D playerCollider;

    void Start() {
        playerCollider = GetComponent<Collider2D>();
        currentBeamIndicator = Instantiate(beamIndicatorPrefab);
        currentBeamIndicator.SetActive(false);
    }

    void Update() {
        BeamIndicator();
    }

    void BeamIndicator() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 currentOrigin = transform.position;

        int maxReflections = 5;
        int reflectionsRemaining = maxReflections;

        RaycastHit2D hit = new RaycastHit2D();
        bool hitSomething = false;

        while (reflectionsRemaining > 0) {
            hit = Physics2D.Raycast(currentOrigin, direction, Mathf.Infinity, ~LayerMask.GetMask("Player"));

            if (hit.collider != null) {
                hitSomething = true;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Reflective")) {
                    direction = Vector2.Reflect(direction, hit.normal);
                    currentOrigin = hit.point;
                    reflectionsRemaining--;
                    continue;
                } else {
                    currentBeamIndicator.transform.position = hit.point;
                    currentBeamIndicator.SetActive(true);
                    break;
                }
            } else {
                break;
            }
        }

        if (!hitSomething || reflectionsRemaining == 0) {
            currentBeamIndicator.transform.position = mousePosition;
            currentBeamIndicator.SetActive(true);
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