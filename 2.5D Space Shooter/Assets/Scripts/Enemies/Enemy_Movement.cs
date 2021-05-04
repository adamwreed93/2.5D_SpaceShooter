using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float _moveSpeed;

    private float _movementType = 0;
    private bool _canStartCoroutine = true;
    private float _movementDirection;

    [SerializeField] private GameObject _enemyChildPrefab;

    private Enemy _enemy;
    private Player _player;



    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemy = _enemyChildPrefab.GetComponent<Enemy>();
        StartCoroutine(ChangeDirection());

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (_movementType == 0)
        {
            transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

            if (transform.position.y <= -5f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 8, 0);
            }
        }

        if (_movementType == 1)
        {
            if (_canStartCoroutine)
            {
                _canStartCoroutine = false;
                StartCoroutine(DirectHorizontalMovement());
            }

            if (_movementDirection == 0)
            {
                transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
            }
            else if (_movementDirection == 1)
            {
                transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
            }

            if (transform.position.x >= 11)
            {
                transform.position = new Vector3(-11, transform.position.y, 0);
            }
            else if (transform.position.x <= -11)
            {
                transform.position = new Vector3(11, transform.position.y, 0);
            }
        }
    }

    private IEnumerator DirectHorizontalMovement()
    {
        _movementDirection = Random.Range(0, 2);

        while (_movementType == 1)
        {
            yield return new WaitForSeconds(3.0f);
            _movementDirection = Random.Range(0, 2);
        }
        _canStartCoroutine = true;
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            if (transform.position.y >= -0.75f && transform.position.y <= 5.5f)
            {
                _movementType = Random.Range(0, 2);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            int randomChanceToFire = Random.Range(0, 2);
            if (randomChanceToFire == 0)
            {
                Debug.Log("Trying To Fire");
                _enemy.FireLaser();
            }
        }
    }
}
