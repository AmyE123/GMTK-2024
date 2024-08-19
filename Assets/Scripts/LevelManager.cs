using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] private List<LevelContainer> _levelPrefabs;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineCamera _cmCamera;

    private LevelContainer _currentLevel;
    private GameObject _currentPlayer;
    private int _currentLevelIdx;

    public GameObject Player => _currentPlayer;

    void Start() {
        OnNewLevelReached(0);       
    }

    void Update() {
        if(_currentLevel != null && _cmCamera.Target.TrackingTarget == null)
        {
            _cmCamera.Target.TrackingTarget = _currentPlayer.transform;
        }

        if (Input.GetKeyDown(KeyCode.Equals)) 
        {
            OnNewLevelReached(_currentLevelIdx + 1);
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            RespawnSameLevel();
        }
    }

    public void OnNewLevelReached(int levelNumber) {
        _currentLevelIdx = levelNumber;

        if (_currentLevel != null) {
            Destroy(_currentLevel.gameObject);
        }

        int levelNum = Mathf.Clamp(levelNumber, 0, _levelPrefabs.Count - 1);

        _currentLevel = Instantiate(_levelPrefabs[levelNum], Vector3.zero, Quaternion.identity);

        SpawnPlayer();
    }

    public void RespawnSameLevel() {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        _currentLevel = Instantiate(_levelPrefabs[_currentLevelIdx], Vector3.zero, Quaternion.identity);

        SpawnPlayer();
    }

    private void SpawnPlayer() {
        if (_currentPlayer != null) {
            Destroy(_currentPlayer);
        }

        _currentPlayer = Instantiate(_playerPrefab, _currentLevel.SpawnPosition, Quaternion.identity);
        _currentPlayer.GetComponent<Player>().InitLevel(_currentLevel);
    }
}