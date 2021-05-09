using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeker : MonoBehaviour
{
    Player player;

    public GameObject miniExplosionPrefab;

    [SerializeField] private bool _isPlayerMissile;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_isPlayerMissile)
        {
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.transform.parent.gameObject);
        }

        if (other.tag == "Laser")
        {
            Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.transform.parent.gameObject);
        }

        if (other.tag == "SuperBeam")
        {
            Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }

        if (other.tag == "Boss")
        {
            Instantiate(miniExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}