using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthInterceptorShot : MonoBehaviour
{
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private GameObject _enemyStealthShip;
    [SerializeField] private float _moveSpeed;
    private float _canFire;

    public float _movementType = 0;
    private bool _canSwitchMovement = true;
    private bool _movingHorizontal = false;
    private bool _canDoThis = false;
    private int _randomDirection;


    private void Start()
    {
        StartCoroutine(CalculateDirectionChange());
    }

    void Update()
    {
        CalculateMovement();

        if (_movingHorizontal)
        {
            HorizontalMovement();
            StartCoroutine(HorizontalMovementDirectionChanger());
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _movingHorizontal)
        {
            FireStealthLaser();
        }

        if (other.tag == "Powerup")
        {
            int randomChanceToFire = Random.Range(0, 2);
            if (randomChanceToFire == 0)
            {
                Debug.Log("Trying To Fire");
                FireStealthLaser();
            }
        }
    }



    void CalculateMovement()
    {
        if (_movementType == 0)
        {
            transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
        }

        if (_movementType == 1)
        {
            if (_canSwitchMovement)
            {
                StartCoroutine(StealthInterceptionMovement());
            }
        }

        if (transform.position.y <= -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8, 0);
        }
        else if (transform.position.x <= -12f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
        else if (transform.position.x >= 12f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
    }


    private IEnumerator CalculateDirectionChange()
    {
        int randomAmountOfTime = Random.Range(1, 10);

        for (int i = 0; i < randomAmountOfTime; i++)
        {
            yield return new WaitForSeconds(1.0f);
        }

        _movementType = 1;
    }

    private IEnumerator StealthInterceptionMovement()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y <= -3.6f && transform.position.y >= -3.9f && _canSwitchMovement)
        {
            _canSwitchMovement = false;
            _movingHorizontal = true;
            _canDoThis = true;
            transform.Rotate(0.0f, 0.0f, 180, Space.World);

            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1);
            }
            _movingHorizontal = false;
            transform.Rotate(0.0f, 0.0f, -180, Space.World);
            _movementType = 0;
            CalculateDirectionChange();
            _canSwitchMovement = true;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator HorizontalMovementDirectionChanger()
    {
        if (_canDoThis)
        {
            _canDoThis = false;
            int randomAmountOfTime = Random.Range(2, 7);

            for (int i = 0; i < randomAmountOfTime; i++)
            {
                yield return new WaitForSeconds(1);
            }
            _randomDirection = Random.Range(0, 2);
            _canDoThis = true;
        }
    }

    void HorizontalMovement()
    {
        if (_randomDirection == 0)
        {
            transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
        }

        if (_randomDirection == 1)
        {
            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        }
    }

    void FireStealthLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, new Vector3((transform.position.x - 1), (transform.position.y + 2), transform.position.z) , Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignStealthEnemyLaser();
            }
        }
    }
    
    public void StopMovement()
    {
        _moveSpeed = 0;
    }
}
