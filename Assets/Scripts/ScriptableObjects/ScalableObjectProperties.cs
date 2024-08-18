using UnityEngine;

[CreateAssetMenu(fileName = "NewScalableObject", menuName = "ScriptableObjects/Game/ScalableObject", order = 1)]
public class ScalableObjectProperties : ScriptableObject
{
    /// <summary>
    /// The speed this object scales
    /// </summary>
    public float scaleSpeed;

    /// <summary>
    /// The minimum scale of this object
    /// </summary>
    public float minScale = 0.5f;

    /// <summary>
    /// The maximum scale of this object
    /// </summary>
    public float maxScale = 3f;
}