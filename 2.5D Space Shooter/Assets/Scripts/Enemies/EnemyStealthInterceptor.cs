using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStealthInterceptor : MonoBehaviour
{
    [SerializeField] private int _pointValue = 10;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private GameObject _enemyStealthInterceptorPrefab;
    [SerializeField] private GameObject explosionPrefab;

    private bool _isDead = false;
    private float _canFire;


    private Player _player;
    private StealthInterceptorShot stealthInterceptorShot;
    private AudioSource _audioSource;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        stealthInterceptorShot = _enemyStealthInterceptorPrefab.GetComponent<StealthInterceptorShot>();
        _audioSource = GetComponent<AudioSource>();
        _isDead = false;

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
    }

    private void Update()
    {
        if (stealthInterceptorShot._movementType == 0)
        {
            FireLaser();
        }
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

            if (player != null)
            {
                other.transform.GetComponent<Player>().Damage();
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            stealthInterceptorShot.StopMovement();
            _audioSource.Play();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.parent.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Laser lasers = other.transform.GetComponentInChildren<Laser>();

            if (!lasers._isEnemyLaser)
            {
                if (_player != null)
                {
                    _player.AddScore(_pointValue);
                }

                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                stealthInterceptorShot.StopMovement();
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(transform.GetChild(0).gameObject);
                Destroy(transform.parent.gameObject, 2.8f);
            }
        }

        if (other.tag == "SuperBeam")
        {
            if (_player != null)
            {
                _player.AddScore(_pointValue);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            stealthInterceptorShot.StopMovement();
            _audioSource.Play();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(transform.parent.gameObject, 2.8f);
        }

        if (other.tag == "SuperMissile")
        {
           

            if (_player != null)
            {
                _player.AddScore(_pointValue);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            stealthInterceptorShot.StopMovement();
            _audioSource.Play();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(transform.parent.gameObject, 2.8f);
        }
    }
}