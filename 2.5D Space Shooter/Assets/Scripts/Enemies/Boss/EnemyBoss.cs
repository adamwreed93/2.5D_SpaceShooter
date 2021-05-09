using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private Player _player;
    private Collider2D _shipCoreCollider;

    [SerializeField] private GameObject _leftTurretPrefab;
    [SerializeField] private GameObject _rightTurretPrefab;
    [SerializeField] private GameObject _giantExplosionPrefab;
    [SerializeField] private GameObject _tinyExplosionPrefab;

    private int _shipCoreLife = 15;
    private int _smallPointValue = 5;
    private int _grandPointValue = 100;



    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _shipCoreCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_leftTurretPrefab == null && _rightTurretPrefab == null)
        {
            _shipCoreCollider.enabled = true;
        }

 
        if (_shipCoreLife <= 0)
        {
            if (_player != null)
            {
                _player.AddScore(_grandPointValue);
            }

            Instantiate(_giantExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _shipCoreLife--;

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
                _shipCoreLife--;

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
            _shipCoreLife--;

            if (_player != null)
            {
                _player.AddScore(_smallPointValue);
            }
        }

        if (other.tag == "SuperMissile")
        {
            _shipCoreLife--;

            if (_player != null)
            {
                _player.AddScore(_smallPointValue);
            }
        }
    }
}
