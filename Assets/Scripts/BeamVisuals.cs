using UnityEngine;

public class BeamVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;

    [Header("Width")]
    [SerializeField] private AnimationCurve _inactiveWidth;
    [SerializeField] private AnimationCurve _activeWidth;

    [Header("Texture Stretch")] 
    [SerializeField] private Vector2 _inactiveStretch;
    [SerializeField] private Vector2 _activeStretch;

    [Header("Colors")]
    [SerializeField] private Gradient _inactiveColor;
    [SerializeField] private Gradient _growColor;
    [SerializeField] private Gradient _shrinkColor;

    [Header("Material")]
    [SerializeField] private Material _beamMat;
    [SerializeField] private Texture2D _inactiveTexture;
    [SerializeField] private Texture2D _activeTexture;

    private void Update()
    {
        // Handle beam visual states
        if (Player.PressingGrow)
        {
            _beamMat.mainTexture = _activeTexture;
            _line.colorGradient = _growColor;
            _line.widthCurve = _activeWidth;
            _line.textureScale = _activeStretch;
        }
        else if (Player.PressingShrink)
        {
            _beamMat.mainTexture = _activeTexture;
            _line.colorGradient = _shrinkColor;
            _line.widthCurve = _activeWidth;
            _line.textureScale = _activeStretch;

        }
        else
        {
            _line.colorGradient = _inactiveColor;
            _beamMat.mainTexture = _inactiveTexture;
            _line.widthCurve = _inactiveWidth;
            _line.textureScale = _inactiveStretch;

        }
    }
}