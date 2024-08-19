using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _neutral;
    [SerializeField] private Sprite[] _growAnim;
    [SerializeField] private Sprite[] _shrinkAnim;
    
    public void SetPosition(Vector3 pos, Vector3 angleVector)
    {
        transform.position = pos;
        float angle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    [SerializeField] private float _animSpeed = 20f;
    private float _currentValue;
    private int _targetNumber;

    public void SetGrowing()
    {
        _targetNumber = 1;
    }

    public void SetShrinking()
    {
        _targetNumber = -1;
    }
    
    private void LateUpdate()
    {
        _currentValue = Mathf.MoveTowards(_currentValue, _targetNumber, Time.deltaTime * _animSpeed);
        _targetNumber = 0;

        if (_currentValue == 0)
        {
            _renderer.sprite = _neutral;
        }
        else if (_currentValue < 0)
        {
            int i = (int) ((-(_shrinkAnim.Length - 1) * _currentValue) + 0.01f);
            _renderer.sprite = _shrinkAnim[i];
        }
        else if (_currentValue > 0)
        {
            int i = (int) (((_growAnim.Length - 1) * _currentValue) + 0.01f);
            _renderer.sprite = _growAnim[i];
        }
    }
}
