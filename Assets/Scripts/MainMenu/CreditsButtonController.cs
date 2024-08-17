using UnityEngine;

public class CreditsButtonController : MonoBehaviour {

    public GameObject creditsPanel;

    public void ShowCreditsPanel() {
        if (creditsPanel != null) {
            creditsPanel.SetActive(true);  
        }
    }

    public void CloseCreditsPanel() {
        if (creditsPanel != null) {
            creditsPanel.SetActive(false);
        }
    }
}