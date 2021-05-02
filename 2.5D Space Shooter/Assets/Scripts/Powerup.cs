using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 0;
    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _audioClip;

    private int _ammoRefill = 15;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                switch(_powerupID)    
                {
                    case 0:
                        player.TripleShotActive(); //Common Spawn
                        break;
                    case 1:
                        player.SpeedBoostActive(); //Common Spawn
                        break;
                    case 2:
                        player.RefillAmmo(_ammoRefill); //Common Spawn
                        break;
                 /////////////////////////////////////////////////////////////////////////
                    case 3:
                        player.ShieldActive(); //Uncommon Spawn
                        break; 
                    case 4:
                        player.RestoreHealth(); //Uncommon Spawn
                        break;
                    case 5:
                        player.NegaShroomActive(); //Uncommon Spawn
                        break;
                 ///////////////////////////////////////////////////////////////////////
                    case 6:
                        player.SuperBeamActive(); //Rare Spawn
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
