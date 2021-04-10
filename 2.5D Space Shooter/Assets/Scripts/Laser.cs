using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 0;


    void Update()
    {
        Destroy(gameObject, 2);

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
}
