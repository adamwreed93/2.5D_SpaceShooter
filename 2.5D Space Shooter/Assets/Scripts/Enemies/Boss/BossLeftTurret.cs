using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLeftTurret : MonoBehaviour
{
    private Player _player;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _tinyExplosionPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private GameObject _enemyLaserPrefab;

    private int _leftTurretLife = 10;
    private int _smallPointValue = 5;
    private int _bigPointValue = 50;
    private float _canFire;
    private float _movementSpeed;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = _player.transform;
    }

    private void Update()
    {
        if (_leftTurretLife <= 0)
        {
            if (_player != null)
            {
                _player.AddScore(_bigPointValue);
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        TurretMovement();
        FireTurretLaser();
    }

    private void TurretMovement()
    {
        transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);

        if (target != null)
        {
            transform.right = target.position - transform.position;
        }
    }

    private void FireTurretLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 5f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, (transform.localEulerAngles.z + 90))));
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
            _leftTurretLife--;

            if (_player != null)
            {
                _player.Damage();
            }
        }

        if (other.tag == "Laser")
        {
            Laser lasers = other.transform.GetComponentInChildren<Laser>();

            if (!lasers._isEnemyLaser)
            {
                _leftTurretLife--;

                if (_player != null)
                {
                    _player.AddScore(_smallPointValue);
                }
                Instantiate(_tinyExplosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }

        if (other.tag == "SuperBeam")
        {
            _leftTurretLife--;

            if (_player != null)
            {
                _player.AddScore(_smallPointValue);
            }
        }

        if (other.tag == "SuperMissile")
        {
            _leftTurretLife--;

            if (_player != null)
            {
                _player.AddScore(_smallPointValue);
            }
        }
    }
}