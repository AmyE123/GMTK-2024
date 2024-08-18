using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform _cameraTransform;
    public float _parallaxFactor;

    private Vector3 _previousCameraPosition;
    private float _startPosition;
    private float _length;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
        _startPosition = transform.position.x;

        if (_cameraTransform != null)
        {
            _previousCameraPosition = _cameraTransform.position;
        }
        
        _length = GetComponent<SpriteRenderer>().bounds.size.x;      
    }

    void Update()
    {
        if (_cameraTransform == null)
        {
            
        }
        else
        {
            float delta = _cameraTransform.position.x - _previousCameraPosition.x;
            float parallax = delta * _parallaxFactor;
            float backgroundTargetPosX = transform.position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, 1f);

            _previousCameraPosition = _cameraTransform.position;

            if (Mathf.Abs(_cameraTransform.position.x - _startPosition) >= _length)
            {
                _startPosition += _length * Mathf.Sign(_cameraTransform.position.x - _startPosition);
            }
        }
    }
}