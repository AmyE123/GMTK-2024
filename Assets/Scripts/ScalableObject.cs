using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    private float _startingMass;
    private Rigidbody2D _rigidBody;

    [SerializeField] private float _scaleSpeed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _startingMass = _rigidBody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO limits on how big/small it can be scaled
        // TODO only if mouse is over this object
        if (Input.GetMouseButton(0))
        {
            float scale = transform.localScale.x;
            scale += Time.deltaTime * _scaleSpeed;
            transform.localScale = new Vector3(scale, scale, scale);
            // TODO calculate the mass based on the scale and starting mass
        }
        if (Input.GetMouseButton(1))
        {
            float scale = transform.localScale.x;
            scale -= Time.deltaTime * _scaleSpeed;
            transform.localScale = new Vector3(scale, scale, scale);
            // TODO calculate the mass based on the scale and starting mass
        }
    }
}
