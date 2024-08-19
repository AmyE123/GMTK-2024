using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    public void SetPosition(Vector3 pos, Vector3 angleVector)
    {
        transform.position = pos;
        float angle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
    public void PlayStaticHand()
    {
        
    }

    public void PlayHandMakeSmall()
    {
        
    }

    public void PlayHandMakeLarge()
    {
        
    }

    public void PlayHandOut()
    {
        
    }
}
