using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] private List<LevelContainer> _levelPrefabs;

    private LevelContainer _currentLevel;
    private int _currentLevelIdx;

    void Start() {
        OnNewLevelReached(0);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Equals)) {
            OnNewLevelReached(_currentLevelIdx + 1);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
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
    }

    public void RespawnSameLevel() {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        _currentLevel = Instantiate(_levelPrefabs[_currentLevelIdx], Vector3.zero, Quaternion.identity);
    }
}