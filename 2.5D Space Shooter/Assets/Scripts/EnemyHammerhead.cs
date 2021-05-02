using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHammerhead : MonoBehaviour
{
    public bool _isWithinRange = false;

    public void WithinRange()
    {
        _isWithinRange = true;
        transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
    }

    public void NotWithinRange()
    {
        _isWithinRange = false;
        transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
    }
}