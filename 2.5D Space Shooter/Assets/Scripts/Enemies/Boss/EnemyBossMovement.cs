using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _shipCorePrefab;

    private bool _stopMoving = false;

    void Update()
    {
        if (!_stopMoving)
        {
            CalculateMovement();
        }

        if (_shipCorePrefab == null)
        {
            Destroy(gameObject);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y <= 1)
        {
            _stopMoving = true;
        }
    }
}
