using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageLink : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    private string url;

    public void OnPointerClick(PointerEventData eventData) {
        OpenURL();
    }

    private void OpenURL() {
        if (!string.IsNullOrEmpty(url)) {
            Application.OpenURL(url);
        } else {
            Debug.LogWarning("URL is not set in the ImageLink script attached to " + gameObject.name);
        }
    }
}