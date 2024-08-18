using UnityEngine;

public class LevelContainer : MonoBehaviour {

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _bottomRight;
    [SerializeField] private Transform _topLeft;

    public Vector3 SpawnPosition => _spawnPoint.position;

    public Vector3 TopLeft => _topLeft.position;
    public Vector3 BottomRight => _bottomRight.position;
}