using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHammerhead : MonoBehaviour
{
    private Player _player;
    public bool _isWithinRange = false;

    [SerializeField] private GameObject _enemyHammerheadPrefab;
    HammerheadMovement hammerheadMovement;


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
    }
}