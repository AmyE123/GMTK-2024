using UnityEngine;

public class PlayerRespawn : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Respawn")) {
            GameObject respawnPointObject = GameObject.Find("RespawnPoint");

            if (respawnPointObject != null) {
                transform.position = respawnPointObject.transform.position;
            } else {
                Debug.LogWarning("RespawnPoint not found in the scene!");
            }
        }
    }
}