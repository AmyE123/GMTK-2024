using UnityEngine;

public class LevelContainer : MonoBehaviour {

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _maximumScaleMeter = 2;
    [SerializeField] private float _startingScaleMeter = 0;
    [SerializeField] private float _minimumPlayerScale = 0.5f;
    [SerializeField] private float _maximumPlayerScale = 2f;
    
    public Vector3 SpawnPosition => _spawnPoint.position;
    public float MaximumScaleMeter => _maximumScaleMeter;
    public float StartingScaleMeter => _startingScaleMeter;
    public float MinimumPlayerScale => _minimumPlayerScale;
    public float MaximumPlayerScale => _maximumPlayerScale;
}