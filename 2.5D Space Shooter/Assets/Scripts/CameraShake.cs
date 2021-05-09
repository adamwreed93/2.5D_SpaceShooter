using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f; //Sets a perameter for how long the shaking has been going on, starting at 0.0.

        while (elapsed < duration) //Checks to see if the ammount of time "elapsed" so far is greater than the duration set from the "Player" script.
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime; //Adds the ammount of time that's passed since the camera shake began to "elapsed".

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
