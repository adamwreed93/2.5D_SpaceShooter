using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _pointValue = 10;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
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
            Destroy(gameObject);
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddScore(_pointValue);
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
        }  
    }
}
