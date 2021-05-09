using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _ammoCountText;
    [SerializeField] private Text _thrusterFuelText;
    [SerializeField] private Text _waveCountText;

    private GameManager _gameManager;

    void Start()
    {
        _ammoCountText.text = "Ammo Count: " + 15;
        _thrusterFuelText.text = "Fuel: " + 100 + "%";
        _scoreText.text = "Score: " + 0;
        _waveCountText.text = "Wave: " + 1;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null!");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateWaveCount(int waveCount)
    {
        _waveCountText.text = "Wave: " + waveCount.ToString();
    }

    public void UpdateAmmoCount(int playerAmmo)
    {
        _ammoCountText.text = "Ammo Count: " + playerAmmo.ToString();
    }

    public void UpdateFuelGuage(int thrusterFuel)
    {
        _thrusterFuelText.text = "Fuel: " + thrusterFuel.ToString() + "%";
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
