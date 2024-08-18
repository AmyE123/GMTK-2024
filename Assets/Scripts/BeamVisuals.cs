using UnityEngine;

public class BeamVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    
    [Header("Width")] 
    [SerializeField] private AnimationCurve _inactiveWidth;
    [SerializeField] private AnimationCurve _activeWidth;

    [Header("Colors")]
    [SerializeField] private Gradient _inactiveColor;
    [SerializeField] private Gradient _growColor;
    [SerializeField] private Gradient _shrinkColor;

    [Header("Material")] 
    [SerializeField] private Material _beamMat;
    [SerializeField] private Texture2D _inactiveTexture;
    
    // Update is called once per frame
    void Update()
    {
        // TODO make this actually attach to the input system
        bool isGrowing = Input.GetMouseButton(0);
        bool isShrinking = Input.GetMouseButton(1);

        if (isGrowing)
        {
            _beamMat.mainTexture = null;
            _line.colorGradient = _growColor;
            _line.widthCurve = _activeWidth;
        }
        else if (isShrinking)
        {
            _beamMat.mainTexture = null;
            _line.colorGradient = _shrinkColor;
            _line.widthCurve = _activeWidth;
        }
        else
        {
            _line.colorGradient = _inactiveColor;
            _beamMat.mainTexture = _inactiveTexture;
            _line.widthCurve = _inactiveWidth;
        }
    }
}
