using UnityEngine;

[CreateAssetMenu(fileName = "ParallaxImage", menuName = "ScriptableObjects/Cutscene/ParallaxImage")]
public class ParallaxImage : ScriptableObject
{
    [System.Serializable]
    public class Layer
    {
        public Sprite image;
        public float parallaxFactor = 1f;
    }

    public Layer[] layers;

    public Vector3 panAmount = new Vector3(5f, 0f, 0f);
    public float displayDuration = 3f;
    public float initialZoom = 0.9f;
    public float finalZoom = 1.0f;

    // Audio and subtitles
    public AudioClip audioClip;
    public string subtitleText;

    public Vector3 GetParallaxOffset(Vector3 cameraPosition, int layerIndex)
    {
        if (layerIndex >= 0 && layerIndex < layers.Length)
        {
            return new Vector3(cameraPosition.x * layers[layerIndex].parallaxFactor, cameraPosition.y * layers[layerIndex].parallaxFactor, 0);
        }
        return Vector3.zero;
    }
}