using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMissileMovement : MonoBehaviour
{
    Player player;
    Enemy enemy;

    [SerializeField] private Transform target;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private bool _isPlayerMissile;

    public List<Enemy> enemiesWithinRange = new List<Enemy>();



    void Start()
    {
        if (!_isPlayerMissile)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            target = player.transform;
        }
        StartCoroutine(TimedDetonation());
    }

    void Update()
    {
        if (!_isPlayerMissile)
        {
            EnemyMissileMovement();
        }
        else
        {
            PlayerMissileMovement();
        }
    }

    private void EnemyMissileMovement()
    {
        transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
        transform.up = target.position - transform.position;
    }

    private void PlayerMissileMovement()
    {
        if (target != null)
        {
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
            transform.right = target.position - transform.position;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 90);
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && _isPlayerMissile)
        {
            enemy = other.GetComponent<Enemy>();
            Debug.Log("Checkpoint 1");
            if (other != null)
            {
                Debug.Log("Checkpoint 2");
                enemiesWithinRange.Add(enemy);

                if (target == null)
                {
                    Debug.Log("Checkpoint 3");
                    target = enemiesWithinRange[0].transform;
                }
            }
        }
    }

    private IEnumerator TimedDetonation()
    {
        yield return new WaitForSeconds(8.0f);
        Destroy(this.gameObject);
    }
}
