using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float spawnRate;

    [SerializeField] private GameObject _newWaveText;
    [SerializeField] private Text _newWaveCountdownText;
    [SerializeField] private GameObject _newWaveCountdownObject;

    private int _enemiesSpawnCount = 5; //The first wave starts with 2 enemies.
    private bool _stopSpawning = false;


    public void StartSpawning() //Called when you shoot the asteroid.
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.0f); //Gives an inital delay after shooting the asteroid before enemies begin spawning.

        while (!_stopSpawning)
        {
            for (int i = 0; i < _enemiesSpawnCount; i++)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(spawnRate);
            }
            
            while (_enemyContainer.transform.childCount > 0) //Continues checking for the enemies to all be dead before starting the next wave.
            {
                yield return new WaitForSeconds(.1f);
            }
            _stopSpawning = true;
            break;
        }
        StartCoroutine(StartNewWave());
    }

    private IEnumerator StartNewWave()
    {
        _enemiesSpawnCount += 2;
        int waitTime = 4;

        for (int i = 0; i < waitTime; i++)
        {
            switch (i)
            {
                case 0:
                    StartCoroutine(TextFlicker());
                    break;
                case 1:
                    _newWaveCountdownText.text = "3";
                    _newWaveCountdownObject.SetActive(true);
                    break;
                case 2:
                    _newWaveCountdownText.text = "2";
                    break;
                case 3:
                    _newWaveCountdownText.text = "1";
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        _newWaveCountdownObject.SetActive(false);
        _stopSpawning = false;
        StartSpawning();
    }

    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //Creates/Declares the Vector3 "posToSpawn".
            int randomPowerUp = Random.Range(0, 6); //Created the int "randomPowerUp" to be used to represent a random PowerupID. [IMPORTANT!] This random range must be changed to match the number of different powerups.
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    private IEnumerator TextFlicker()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(.25f);
            _newWaveText.SetActive(false);
            yield return new WaitForSeconds(.25f);
            _newWaveText.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _newWaveText.SetActive(false);
            yield return new WaitForSeconds(.25f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}