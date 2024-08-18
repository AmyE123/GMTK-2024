using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class LevelManager : MonoBehaviour {

    [SerializeField] private List<LevelContainer> _levelPrefabs;

    private LevelContainer _currentLevel;

    void Start() {
        OnNewLevelReached(0);
    }

    private int _currentLevelIdx;

    public void OnNewLevelReached(int levelNumber) {
        _currentLevelIdx = levelNumber;

        Destroy(_currentLevel.gameObject);


        int levelNum = Mathf.Clamp(levelNumber, 0, _levelPrefabs.Count - 1);

        _currentLevel = Instantiate(_levelPrefabs[levelNum]);
    }

    public void RespawnSameLevel() {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        _currentLevel = Instantiate(_levelPrefabs[_currentLevelIdx]);
    }
}