using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    [SerializeField] private ScalableObjectProperties properties;

    private float _startingMass;
    private float _currentMass;
    private Rigidbody2D _rigidBody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _startingMass = _rigidBody.mass;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            float scale = transform.localScale.x;
            
            if (scale <= properties.maxScale.x)
            {
                scale += Time.deltaTime * properties.scaleSpeed;
                transform.localScale = new Vector3(scale, scale, scale);
                _currentMass = _startingMass * Mathf.Pow(scale, 3);
            }
        }
        if (Input.GetMouseButton(1))
        {
            float scale = transform.localScale.x;

            if(scale >= properties.minScale.x)
            {
                scale -= Time.deltaTime * properties.scaleSpeed;
                transform.localScale = new Vector3(scale, scale, scale);
                _currentMass = _startingMass * Mathf.Pow(scale, 3);
            }
        }
    }
}
