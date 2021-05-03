using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHammerhead : MonoBehaviour
{
    HammerheadMovement hammerheadMovement;

    [SerializeField] private GameObject _enemyHammerheadPrefab;
    [SerializeField] private int _pointValue = 10;

    private Player _player;

    public bool _isWithinRange = false;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hammerheadMovement = _enemyHammerheadPrefab.GetComponent<HammerheadMovement>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
    }
    public void WithinRange()
    {
        _isWithinRange = true;
        transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
    }

    public void NotWithinRange()
    {
        _isWithinRange = false;
        transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
            hammerheadMovement.KilledByPlayer();
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
                Destroy(other.gameObject);
                hammerheadMovement.TouchedLaser();
            }


            if (other.tag == "SuperBeam")
            {
                if (_player != null)
                {
                    _player.AddScore(_pointValue);
                }
                hammerheadMovement.TouchedSuperBeam();
            }
        }
    }
}