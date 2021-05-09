using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotParentDestroyer : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(TimeToDie());
    }

    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(7);
        Destroy(this.gameObject);
    }
}
