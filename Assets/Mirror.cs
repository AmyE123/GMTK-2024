using UnityEngine;
using System.Collections.Generic;

public class Mirror : BeamObject
{
    private class MirrorHit
    {
        public Vector2 start;
        public Vector2 end;

        public MirrorHit(Vector2 s, Vector2 e)
        {
            start = s;
            end = e;
        }
    }

    [SerializeField] private GameObject _beamPrefab;
    private List<LineRenderer> _lines = new();
    private List<MirrorHit> _hits = new();

    private void LateUpdate()
    {
        int maximum = Mathf.Max(_lines.Count, _hits.Count);

        for (int i = 0; i < maximum; i++)
        {
            bool hasHit = _hits.Count > i;
            bool hasLine = _lines.Count > i;

            if (hasHit && hasLine)
            {
                _lines[i].SetPosition(0, _hits[i].start);
                _lines[i].SetPosition(1, _hits[i].end);
                _lines[i].enabled = true;
            }
            else if (hasHit)
            {
                GameObject newObj = Instantiate(_beamPrefab, transform);
                LineRenderer line = newObj.GetComponent<LineRenderer>();
                _lines.Add(line);
                
                _lines[i].SetPosition(0, _hits[i].start);
                _lines[i].SetPosition(1, _hits[i].end);
                _lines[i].enabled = true;
            }
            else if (hasLine)
            {
                _lines[i].enabled = false;
            }
        }
        
        _hits.Clear();
    }

    public override void HitWithRay(Vector2 point, Vector2 direction) 
    {
        direction.x = -direction.x;

        RaycastHit2D hit = Physics2D.Raycast(point, direction);
        bool didHitSomething = hit.collider != null;
        
        if (didHitSomething) 
        {
            _hits.Add(new MirrorHit(point, hit.point));
            BeamObject hitObject = hit.collider.GetComponent<BeamObject>();
            
            if (hitObject != null) 
            {
                hitObject.HitWithRay(hit.point, direction);
            }
        } 
        else 
        {
            _hits.Add(new MirrorHit(point, point + (direction * 999)));
        }
    }
}