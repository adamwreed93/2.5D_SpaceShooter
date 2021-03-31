using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private  GameObject _enemy1Prefab;
    [SerializeField] private GameObject _enemyContainer;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    //[MEDIUM ARTICLE NOTE!!!]
    //Do a version of this with Void instead of IEnmumerator for the GIFs!
    public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //Creates/Declares the Vector3 "posToSpawn".
            GameObject newEnemy = Instantiate(_enemy1Prefab, posToSpawn, Quaternion.identity); //Creates/Declares the "newEnemy" GameObject variable and instantiates the "_enemy1Prefab" at the transform position of "posToSpawn".
            newEnemy.transform.parent = _enemyContainer.transform; //sets the transform of the "newEnemy" GameObject to be a child of "_enemyContainer'.
            yield return new WaitForSeconds(5.0f);
        }
    }
}
