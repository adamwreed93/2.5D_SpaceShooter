using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerheadMovement : MonoBehaviour
{
    private EnemyHammerhead _enemyHammerhead;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _pointValue = 10;

    private AudioSource _audioSource;
    private Player _player;
    private Transform target;

    public GameObject explosionPrefab;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _enemyHammerhead = GameObject.FindGameObjectWithTag("Enemy_Hammerhead").GetComponent<EnemyHammerhead>();
        _audioSource = GetComponent<AudioSource>();
        target = _player.transform;


        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        if (_enemyHammerhead == null)
        {
            Debug.LogError("The EnemyHammerhead is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (_enemyHammerhead != null && _enemyHammerhead._isWithinRange)
        {
            transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
            transform.right = target.position - transform.position;
        }
        else
        {
            transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

            if (transform.position.y <= -5f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 9, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _enemyHammerhead.WithinRange();
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
                _moveSpeed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(transform.GetChild(0).gameObject);
                Destroy(gameObject, 2.8f);
            }
        }

        if (other.tag == "SuperBeam")
        {
            if (_player != null)
            {
                _player.AddScore(_pointValue);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            _moveSpeed = 0;
            _audioSource.Play();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            _enemyHammerhead.NotWithinRange();
        }
    }

    public void KilledByPlayer()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        _moveSpeed = 0;
        _audioSource.Play();
        Destroy(transform.GetChild(0).gameObject);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 2.8f);
    }
}