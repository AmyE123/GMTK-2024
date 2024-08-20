using UnityEngine;

public class LevelEndTrigger : MonoBehaviour {

    [SerializeField] private string targetTag = "Player"; 
    
    private LevelManager _levelManager;

    void Start() {

        _levelManager = FindObjectOfType<LevelManager>();
        if (_levelManager == null) {
            Debug.LogError("LevelManager not found in the scene.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag(targetTag)) {
            collision.gameObject.GetComponent<Player>().OnWin(GetComponent<SpriteRenderer>().sprite);
            gameObject.SetActive(false);
        }
    }
}