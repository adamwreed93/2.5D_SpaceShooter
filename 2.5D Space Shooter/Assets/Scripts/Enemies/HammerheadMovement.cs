using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerheadMovement : MonoBehaviour
{
    [SerializeField] private GameObject _EnemyHammerheadGFX;
    [SerializeField] private float _moveSpeed;

    private EnemyHammerhead _enemyHammerhead;
    private AudioSource _audioSource;
    private Player _player;
    private Transform target;

    public bool _isWithinRange = false;

    [SerializeField] private GameObject explosionPrefab;


    private void Start()
    {
        _enemyHammerhead = _EnemyHammerheadGFX.GetComponent<EnemyHammerhead>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        if (_enemyHammerhead != null && _isWithinRange)
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
            _enemyHammerhead.RotateThisEnemy();
            _isWithinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            _isWithinRange = false;
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

    public void TouchedLaser()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        _moveSpeed = 0;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(transform.GetChild(0).gameObject);
        Destroy(gameObject, 2.8f);
    }

    public void TouchedSuperBeam()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        _moveSpeed = 0;
        _audioSource.Play();
        Destroy(transform.GetChild(0).gameObject);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 2.8f);
    }
}