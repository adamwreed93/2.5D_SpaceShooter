using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupDetector : MonoBehaviour
{
    private Powerup powerup;

    private bool _isWithinRange = false;
    private bool _isAttractingPowerup = false;
    [SerializeField] private bool _outOfRange = true;

    public List<Powerup> powerupsWithinRange = new List<Powerup>(); //This list will hold all powerups that are within the players detection range collider.

    private void Update()
    {
        if (Input.GetKey(KeyCode.C) && _isWithinRange)
        {
            _outOfRange = false;
            _isAttractingPowerup = true;
            if (powerup != null)
            {
                foreach (Powerup powerup in powerupsWithinRange)
                {
                    powerup.AttractPowerup();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.C) && _isAttractingPowerup && !_outOfRange || !_outOfRange && !_isWithinRange)
        {
            _isAttractingPowerup = false;
            _outOfRange = true;

            if (powerup != null)
            {
                powerup.StopAttractingPowerup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            powerup = other.GetComponent<Powerup>();

            if (other.GetComponent<Powerup>() != null)
            {
                powerupsWithinRange.Add(powerup);
            }

            _isWithinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            powerup = other.GetComponent<Powerup>();
            if (other.GetComponent<Powerup>() != null)
            {
                powerupsWithinRange.Remove(powerup);
            }
            _isWithinRange = false;
        }
    }
}
