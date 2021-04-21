﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _pointValue = 10;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3.0f;

    private float _canFire;

    private Animator _anim;
    private Player _player;
    private AudioSource _audioSource;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

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
        CalculateMovement();
        FireLaser();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
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
            _anim.SetTrigger("OnEnemyDeath");
            _moveSpeed = 0;
            _audioSource.Play();
            Destroy(gameObject, 2.8f);
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

                _anim.SetTrigger("OnEnemyDeath");
                Destroy(other.gameObject);
                _moveSpeed = 0;
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject, 2.8f);
            }
        }  
    }
}