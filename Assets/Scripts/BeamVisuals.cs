using UnityEngine;

public class BeamVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handSprite;

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

    private bool _handIsFlipped = false;

    // Update is called once per frame
    void Update()
    {
        // Handle beam visual states
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