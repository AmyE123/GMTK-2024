using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour {

    public Slider skipSlider; 
    public float holdTime = 2.0f; 
    private float holdDuration = 0f; 
    private bool isHolding = false;

    void Update() {

        if (Input.GetMouseButton(1)) {
            if (!isHolding) {
                isHolding = true;
                skipSlider.gameObject.SetActive(true);
            }

            holdDuration += Time.deltaTime;

            skipSlider.value = Mathf.Clamp01(holdDuration / holdTime);

            if (holdDuration >= holdTime) {
                SceneManager.LoadScene("GameScene");
            }
        } else {
            if (isHolding) {
                isHolding = false;
                holdDuration = 0f;
                skipSlider.gameObject.SetActive(false);
            }
        }
    }
}