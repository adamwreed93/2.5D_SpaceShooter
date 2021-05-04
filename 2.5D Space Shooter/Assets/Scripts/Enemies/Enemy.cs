using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _pointValue = 10;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private GameObject _enemyShieldVisualizer;
    [SerializeField] private GameObject _enemyBasicPrefab;

    private bool _isDead = false;
    private float _canFire;
    private bool _enemyShieldActive = false;

    private Animator _anim;
    private Player _player;
    private Enemy_Movement _enemy_Movement;
    private AudioSource _audioSource;


    private void Start()
    {
        _enemy_Movement = _enemyBasicPrefab.GetComponent<Enemy_Movement>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _isDead = false;
        CheckForShield();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");

        }
    }

    void Update()
    {
        FireLaser();
    }

    private void CheckForShield()
    {
        int randomNumber = Random.Range(0, 6);

        if (randomNumber == 1)
        {
            ActivateEnemyShields();
        }
    }

    public void ActivateEnemyShields()
    {
        _enemyShieldActive = true;
        _enemyShieldVisualizer.SetActive(true);
    }


    public void FireLaser()
    {
        if (Time.time > _canFire && !_isDead)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (_enemyShieldActive)
            {
                if (player != null)
                {
                    other.transform.GetComponent<Player>().Damage();
                }

                _enemyShieldActive = false;
                _enemyShieldVisualizer.SetActive(false);
                return;
            }

            if (player != null)
            {
                other.transform.GetComponent<Player>().Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemy_Movement._moveSpeed = 0;
            _audioSource.Play();
            _isDead = true;
            Destroy(transform.parent.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Laser lasers = other.transform.GetComponentInChildren<Laser>();

            if (!lasers._isEnemyLaser)
            {
                if (_enemyShieldActive)
                {
                    Destroy(other.gameObject);
                    _enemyShieldActive = false;
                    _enemyShieldVisualizer.SetActive(false);
                    return;
                }
            }

            if (!lasers._isEnemyLaser)
            {
                if (_player != null)
                {
                    _player.AddScore(_pointValue);
                }

                _anim.SetTrigger("OnEnemyDeath");
                Destroy(other.gameObject);
                _enemy_Movement._moveSpeed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                _isDead = true;
                Destroy(transform.parent.gameObject, 2.8f);
            }
        }

        if (other.tag == "SuperBeam")
        {
            if (_player != null)
            {
                _player.AddScore(_pointValue);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _enemy_Movement._moveSpeed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            _isDead = true;
            Destroy(transform.parent.gameObject, 2.8f);
        }
    }
}
