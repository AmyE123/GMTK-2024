using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] JumpInfo _jump;
    [SerializeField] GroundInfo _ground;
    [SerializeField] MoveInfo _move;

    Rigidbody2D _rb;
    bool _isLocked;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        OnValidate();
    }

    private void OnValidate()
    {
        _ground.OnValidate();
    }

    public void SetDesiredMove(float move)
    {
        _move.desiredVelocity = new Vector2(move * _move.maxSpeed, 0);

    }

    private void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        _rb.velocity = _move.velocity;
        ClearState();
    }

    private void UpdateState()
    {
        _move.velocity = _rb.velocity;

        if (_ground.IsGrounded())
        {
            _ground.contactNormal.Normalize();
        }
        else
        {
            _ground.contactNormal = Vector2.up;
        }

        if (_jump.isRequested)
        {
            _jump.isRequested = false;
            Jump();
        }
    }

    private void ClearState()
    {
        _ground.groundContactCount = 0;
        _ground.contactNormal = Vector2.zero;
    }

    public void SetJumpRequested(bool jumpPressed)
    {
        _jump.isRequested |= jumpPressed;
    }

    private void Jump()
    {
        if (_ground.IsGrounded())
        {
            float jumpSpeed = _jump.Speed(transform.localScale.x);
            float alignedSpeed = Vector2.Dot(_move.velocity, _ground.contactNormal);

            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);
            }

            _move.velocity.y = 0;
            _move.velocity += _ground.contactNormal * jumpSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private Vector2 ProjectOnContactPlane(Vector2 vector)
    {
        return vector - _ground.contactNormal * Vector2.Dot(vector, _ground.contactNormal);
    }

    private void AdjustVelocity()
    {
        Vector2 xAxis = ProjectOnContactPlane(Vector2.right).normalized;
        float currentX = Vector3.Dot(_move.velocity, xAxis);

        float acceleration = _ground.IsGrounded() ? _move.maxAcceleration : _move.maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, _move.desiredVelocity.x, maxSpeedChange);
        _move.velocity += xAxis * (newX - currentX);
    }


    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            
            if (normal.y >= _ground.minGroundDotProduct)
            {
                Debug.DrawLine(collision.GetContact(i).point, collision.GetContact(i).point + Vector2.up, Color.green);
                _ground.groundContactCount++;
                _ground.contactNormal += normal;
            }
            else
            {
                Debug.DrawLine(collision.GetContact(i).point, collision.GetContact(i).point + Vector2.up, Color.red);
            }

        }
    }
}
