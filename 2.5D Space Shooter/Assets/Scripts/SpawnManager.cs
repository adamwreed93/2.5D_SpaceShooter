using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private  GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _powerups;

    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float spawnRate;

    private bool _stopSpawning = false;

   void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //Creates/Declares the Vector3 "posToSpawn".
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); //Creates/Declares the "newEnemy" GameObject variable and instantiates the "_enemy1Prefab" at the transform position of "posToSpawn".
            newEnemy.transform.parent = _enemyContainer.transform; //sets the transform of the "newEnemy" GameObject to be a child of "_enemyContainer'.
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //Creates/Declares the Vector3 "posToSpawn".
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}