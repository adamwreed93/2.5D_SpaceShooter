using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRangeScript : MonoBehaviour
{
    private int _randomNumber;

    public void RandomNumberGenerator()
    {
        _randomNumber = Random.Range(0, 100);

        switch (_randomNumber)
        {
            case int n when (n <= 25):
                Debug.Log("You rolled " + _randomNumber);
                break;
            case int n when (n >= 26 && n <= 50):
                Debug.Log("You rolled " + _randomNumber);
                break;
            case int n when (n >= 51 && n <= 75):
                Debug.Log("You rolled " + _randomNumber);
                break;
            case int n when (n >= 51 && n <= 100):
                Debug.Log("You rolled " + _randomNumber);
                break;
        }
    }
}
