using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Hit: " + other.transform.name);
            other.transform.GetComponent<Player>().Damage(); 
            Destroy(gameObject);
        }
        else if (other.tag =="Laser")
        {
            Debug.Log("Hit: " + other.transform.name);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
            
    }
}
