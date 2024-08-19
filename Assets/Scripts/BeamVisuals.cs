using UnityEngine;

public class BeamVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handSprite;
    [SerializeField] private HandAnimation _handAnimation;

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
    [SerializeField] private Texture2D _inactiveTexture;

    private bool _handIsFlipped = false;

    {
        // Handle beam visual states
        bool isGrowing = Input.GetMouseButton(0);
        bool isShrinking = Input.GetMouseButton(1);

        if (isGrowing)
        {
            _beamMat.mainTexture = _activeTexture;
            _handAnimation.PlayHandMakeLarge();
            _beamMat.mainTexture = null;
            _line.colorGradient = _growColor;
            _line.widthCurve = _activeWidth;
            _line.textureScale = _activeStretch;
        }
        else if (isShrinking)
        {
            _handAnimation.PlayHandMakeSmall();
            _beamMat.mainTexture = _activeTexture;
            _line.colorGradient = _shrinkColor;
            _line.widthCurve = _activeWidth;
            _line.textureScale = _activeStretch;

        }
        else
        {
            _handAnimation.PlayStaticHand();
            _line.colorGradient = _inactiveColor;
            _beamMat.mainTexture = _inactiveTexture;
            _line.widthCurve = _inactiveWidth;
            _line.textureScale = _inactiveStretch;

        }

        RotateHandTowardsMouse();
    }

    private void RotateHandTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Assuming 2D or using a fixed Z value for 3D
        Vector3 direction = mousePosition - _handTransform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = _handTransform.eulerAngles.z;
        float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * 50f);
        _handTransform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothedAngle));
        // Check if the rotation angle is more than 90 degrees or less than -90 degrees
        if (smoothedAngle > 90f && smoothedAngle < 270f)
        {
            if (!_handIsFlipped)
            {
                // Rotate the hand by 180 degrees around the Y-axis to flip it
                _handSprite.rotation *= Quaternion.Euler(180f, 0, 0);
                _handIsFlipped = true; // Set flipped state to true
            }
        }
        else
        {
            if (_handIsFlipped)
            {
                // Ensure the hand is not flipped by resetting the Y rotation
                _handSprite.rotation = Quaternion.Euler(0, 0, smoothedAngle);
                _handIsFlipped = false; // Set flipped state to false
            }
        }
    }
}