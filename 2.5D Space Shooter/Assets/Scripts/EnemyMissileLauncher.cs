using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _pointValue = 10;
    [SerializeField] private GameObject _HeatSeekingMissilePrefab;
    [SerializeField] private float _fireRate = 3.0f;

    private bool _isDead = false;
    private bool _stopMoving = false;
    private bool _alreadyFired = false;
    private int _firingRange;

    private Animator _anim;
    private Player _player;
    private AudioSource _audioSource;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _isDead = false;


        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");

        }

        _firingRange = Random.Range(-9, 9);
    }

    void Update()
    {
        if (!_stopMoving)
        {
            CalculateMovement();

            if (transform.position.x >= _firingRange && !_alreadyFired)
            {
                _stopMoving = true;
                _alreadyFired = true;
                StartCoroutine(FireMissiles());
            }
        }



    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);

        if (transform.position.x >= 11f)
        {
            float randomY = Random.Range(1.5f, 5.5f);
            transform.position = new Vector3(-11, randomY, 0);
            _alreadyFired = false;
        }
    }

    private IEnumerator FireMissiles()
    {
        int numberOfMissiles = Random.Range(1, 4);

        for (int i = 0; i < numberOfMissiles; i++) 
        {
            Instantiate(_HeatSeekingMissilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.5f);
        }
        _stopMoving = false;

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
            _isDead = true;
            Destroy(transform.GetChild(0).gameObject);
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
                _isDead = true;
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

            _anim.SetTrigger("OnEnemyDeath");
            _moveSpeed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            _isDead = true;
            Destroy(gameObject, 2.8f);
        }
    }
}
