using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSceneMegaScript : MonoBehaviour
{
    private Player _player;
    private PlayerBeam _beam;
    
    [SerializeField] private ScalableObject _planet;
    [SerializeField] private Transform _leftPlanetCollider;
    [SerializeField] private Transform _rightPlanetCollider;
    [SerializeField] private float _yVelocity = 1;
    
    private Rigidbody2D _playerRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _beam = FindObjectOfType<PlayerBeam>();
        
        _player.EnableSpaceMode();
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();
        _beam.ResetScaleMeter(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (_stage == 1)
        {
            StageOne();
        }
        else if (_stage == 2)
        {
            StageTwo();
        }
        else if (_stage == 3)
        {
            StageThree();
        }
    }

    private int _stage = 1;
    

    private void StageOne()
    {
        _leftPlanetCollider.transform.position = _planet.transform.position - new Vector3(_planet.transform.localScale.x + 4, -10, 0);
        _rightPlanetCollider.transform.position = _planet.transform.position + new Vector3(_planet.transform.localScale.x + 4, 10, 0);

        if (_planet.CurrentScale <= _planet.MinScale + 0.1f)
        {
            _stage++;
            _playerRigidbody.freezeRotation = false;
            Physics2D.gravity = Vector2.zero;
            _leftPlanetCollider.gameObject.SetActive(false);
            _rightPlanetCollider.gameObject.SetActive(false);
        }
    }

    private float _stageTwoTime = 0;

    private void StageTwo()
    {
        _stageTwoTime += Time.deltaTime;
        float yvel = Mathf.Lerp(0, _yVelocity, _stageTwoTime / 5f);
        
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, yvel);

        if (_stageTwoTime >= 5)
        {
            _stage++;
        }
    }

    private void StageThree()
    {
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, _yVelocity);

    }
}
