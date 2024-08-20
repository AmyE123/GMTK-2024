using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpaceSceneMegaScript : MonoBehaviour
{
    private Player _player;
    private PlayerBeam _beam;

    [SerializeField] private CanvasGroup _endGroup;
    [SerializeField] private ScalableObject _planet;
    [SerializeField] private Transform _leftPlanetCollider;
    [SerializeField] private Transform _rightPlanetCollider;
    [SerializeField] private Transform _roofBlocker;
    [SerializeField] private Transform _playerFollow;
    [SerializeField] private float _yVelocity = 1;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _narration;
    private Rigidbody2D _playerRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _beam = FindObjectOfType<PlayerBeam>();
        
        _player.EnableSpaceMode();
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();
        _beam.ResetScaleMeter(0, 100);
        _beam.SetSpaceMode();
    }

    // Update is called once per frame
    void Update()
    {
        _roofBlocker.transform.position = _player.transform.position + new Vector3(0, 22, 0);

        
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
            _roofBlocker.gameObject.SetActive(false);
        }
    }

    private float _stageTwoTime = 0;
    private bool _musicPlayed;

    private void StageTwo()
    {
        _stageTwoTime += Time.deltaTime;
        float yvel = Mathf.Lerp(0, _yVelocity, _stageTwoTime / 8f);
        
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, yvel);

        if (_stageTwoTime > 1.5f && _musicPlayed == false)
        {
            _musicPlayed = true;
            _music.Play();
        }
        
        if (_stageTwoTime >= 8)
        {
            _narration.Play();
            _stage++;
        }
        
        _playerFollow.transform.position = Vector3.Lerp(_playerFollow.transform.position, _player.transform.position, _stageTwoTime / 8f);
    }

    private void StageThree()
    {
        float xVel = _playerRigidbody.velocity.x;
        float xpos = _player.transform.position.x;

        if (xpos < -24)
        {
            xpos = -24;
        }
        if (xpos > 24)
        {
            xpos = 24;
        }

        _player.transform.position = new Vector3(xpos, _player.transform.position.y, _player.transform.position.z);
        
        _playerRigidbody.velocity = new Vector2(xVel, _yVelocity);
        
        Vector3 pos = _player.transform.position;
        pos.y = Mathf.Clamp(pos.y, -100, 220);
        _playerFollow.transform.position = pos;


        if (_player.transform.position.y > 240)
        {
            _endGroup.DOFade(1, 1).SetEase(Ease.Linear);
            _stage++;
        }
    }
}
