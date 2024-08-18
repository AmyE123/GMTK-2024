using UnityEngine;

[System.Serializable]
public class MoveInfo
{
    [Range(0f, 100f)] public float maxAcceleration = 10f;
    [Range(0f, 100f)] public float maxAirAcceleration = 1f;
    [Range(0f, 100f)] public float maxSpeed = 10f;

    public float tempMaxSpeed;
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public Vector2 desiredVelocity;
}
