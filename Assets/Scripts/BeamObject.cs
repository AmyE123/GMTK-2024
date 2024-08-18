using UnityEngine;

public class BeamObject : MonoBehaviour
{

    protected const int MAX_RECURSION_DEPTH = 32;
    
    public virtual void HitWithRay(Vector2 point, Vector2 direction, Vector2 normal, PlayerBeam beam, int depth=0)
    {
    
    }
}
