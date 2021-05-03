using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 0;

    public bool _isEnemyLaser = false;
    public bool _isStealthEnemyLaser = false;


    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else if (_isEnemyLaser && !_isStealthEnemyLaser)
        {
            MoveDown();
        }
        else if (_isStealthEnemyLaser && _isEnemyLaser)
        {
            MoveUp();
        }

        Destroy(gameObject, 2);
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void AssignStealthEnemyLaser()
    {
        _isEnemyLaser = true;
        _isStealthEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
