using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeker : MonoBehaviour
{
    Player player;

    [SerializeField] private Transform target;
    [SerializeField] private float _movementSpeed;
    public GameObject miniExplosionPrefab;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.transform;
        StartCoroutine(TimedDetonation());
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        transform.right = target.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.Damage();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (other.tag == "SuperBeam")
        {
            Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator TimedDetonation()
    {
        yield return new WaitForSeconds(8.0f);
        Destroy(this.gameObject);
    }
}