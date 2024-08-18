using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScaler : MonoBehaviour
{
    private float _defaultGravity;
    private float _defaultSize;
    
    [SerializeField]
    private Rigidbody2D _player;

    [SerializeField] private Vector2 _initialForce;

    [SerializeField] private float _initialRotiation;
    
    private bool _isStopped;
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultGravity = Physics2D.gravity.y;
        _defaultSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStopped)
            return;
        
        float gravMultiplier = transform.localScale.x / _defaultSize;
        Physics2D.gravity = new Vector2(0, _defaultGravity * gravMultiplier);
        
        // TODO get proper jump button
        if (Input.GetKeyDown(KeyCode.Space) && gravMultiplier < 0.02f)
        {
            Physics2D.gravity = Vector2.zero;
            _player.velocity = _initialForce;
            _player.angularVelocity = _initialRotiation;
            _player.freezeRotation = false;
            
            _isStopped = true;
        }
    }
}
