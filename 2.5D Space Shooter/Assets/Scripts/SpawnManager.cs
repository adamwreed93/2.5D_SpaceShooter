using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _basicEnemyPrefab;
    [SerializeField] private GameObject _missileLauncherEnemyPrefab;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float spawnRate;

    [SerializeField] private GameObject _newWaveText;
    [SerializeField] private Text _newWaveCountdownText;
    [SerializeField] private GameObject _newWaveCountdownObject;

    private int _basicEnemiesSpawnCount = 5; //The first wave starts with 2 enemies.
    private int _missileEnemiesSpawnCount = 1;
    private bool _stopSpawning = false;
    private int _waveNumber = 1; //This will eventually need to be put into the UI as an image.
    private bool _spawnedMissileEnemies = false; 


    public void StartSpawning() //Called when you shoot the asteroid.
    {
        StartCoroutine(SpawnBasicEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(NewWaveManager());

        if (_waveNumber > 2 && !_spawnedMissileEnemies)
        {
            int random = Random.Range(0, 4);
            if (random >= 2)
            {
                _spawnedMissileEnemies = true;
                StartCoroutine(SpawnMissileEnemyRoutine());
            }
        }
    }

    public IEnumerator SpawnBasicEnemyRoutine()
    {
        yield return new WaitForSeconds(1.0f); //Gives an inital delay before spawning waves.

        while (!_stopSpawning)
        {
            for (int i = 0; i < _basicEnemiesSpawnCount; i++)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                GameObject newEnemy = Instantiate(_basicEnemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(spawnRate);
            }
            break;
        }
        
    }

    public IEnumerator SpawnMissileEnemyRoutine()
    {
        yield return new WaitForSeconds(1.0f); //Gives an inital delay before spawning waves.

        while (!_stopSpawning)
        {
            for (int i = 0; i < _missileEnemiesSpawnCount; i++)
            {
                float randomY = Random.Range(1.5f, 5.5f);
                Vector3 posToSpawn = new Vector3(randomY, -11, 0);
                GameObject newEnemy = Instantiate(_missileLauncherEnemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(spawnRate);
            }
            _missileEnemiesSpawnCount++;
            break;
        }
    }

    private IEnumerator NewWaveManager()
    {
        yield return new WaitForSeconds(6f);
        while (_enemyContainer.transform.childCount > 0) //Continues checking for the enemies to all be dead before starting the next wave.
        {
            yield return new WaitForSeconds(.1f);
        }
        _stopSpawning = true;
        StartCoroutine(StartNewWave());
    }


    private IEnumerator StartNewWave()
    {
        _basicEnemiesSpawnCount += 2;
        _waveNumber++;
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
        _spawnedMissileEnemies = false;
        StartSpawning();
    }

    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //Creates/Declares the Vector3 "posToSpawn".
            int randomPowerUp = Random.Range(0, 7); //Created the int "randomPowerUp" to be used to represent a random PowerupID. [IMPORTANT!] This random range must be changed to match the number of different powerups.
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