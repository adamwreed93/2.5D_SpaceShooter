using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    public ColorChanger _colorChanger;
    public BoxCollider2D superBeamCollider;
    public CameraShake cameraShake;

    [SerializeField] private GameObject _thruster;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _superBeamPrefab;
    [SerializeField] private GameObject _superMissilePrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngine, _rightEngine; //Similar references like this can be written like this but you hjave to be careful. This is more ofr organizational purposes.
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private Animator _superBeamAnimator;

    private AudioSource _audioSource;

    [SerializeField] private int _score;
    [SerializeField] private int _lives = 3;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _fireRate = 0;
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private int _thrusterFuel = 100;


    private float _canFire = -1f; //The -1 starts _canFire below 0. So long as Time.time is higher than _canFire, you can shoot!  
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _isSuperBeamActive = false;
    private bool _isSuperMissileActive = false;
    private bool _isThrusterActive = false;
    private bool _isNegaShroomActive = false;
    private float _defaultSpeed;
    




    void Start()
    {
        _defaultSpeed = _speed;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _colorChanger = _shieldVisualizer.GetComponent<ColorChanger>();
        superBeamCollider = _superBeamPrefab.GetComponent<BoxCollider2D>();
        _superBeamAnimator = _superBeamPrefab.GetComponent<Animator>();


        if (_colorChanger == null)
        {
            Debug.LogError("The _colorChanger is NULL.");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    void Update()
    {
        Boundries();
        Movement();
        Thrusters();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0 && !_isSuperBeamActive && !_isSuperMissileActive)
        {
            FireLaser();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && !_isSuperBeamActive && _isSuperMissileActive)
        {
            FireMissile();
        }
    }

    void Boundries()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0); //Mathf.Clamp is used here to create a min and max value for Y as it prevents the value from going any higher or lower.

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);

        }
        else if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); //Creates the Vector3 "direction" for use within this method.

        transform.Translate(direction * _speed * Time.deltaTime);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        _ammoCount--;
        _uiManager.UpdateAmmoCount(_ammoCount);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    void FireMissile()
    {
        _canFire = Time.time + (_fireRate * 3);
        Instantiate(_superMissilePrefab, new Vector3(transform.position.x, (transform.position.y + 1.5f), transform.position.z), Quaternion.identity);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _colorChanger.DamageShield();
            return;
        }

        StartCoroutine(cameraShake.Shake(.15f, .4f));
        _lives--;

        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SuperBeamActive()
    {
        StartCoroutine(SuperBeamPowerDownRoutine());
    }

    public void SuperMissileActive()
    {
        _isSuperMissileActive = true;
        StartCoroutine(SuperMissilePowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = (_defaultSpeed * _speedMultiplier);
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

        if (_colorChanger != null)
        {
            _colorChanger.Restoreshield();
        }
    }

    public void DeactivateShield() //Called fromt the "ColorChange" script when shield is destroyed.
    {
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    
    private IEnumerator SuperBeamPowerDownRoutine()
    {
        _superBeamPrefab.SetActive(true);
        _superBeamAnimator.SetBool("SuperBeamActive", true);
        _isSuperBeamActive = true;
        superBeamCollider.offset = new Vector3(0, 2.5f, 0);
        yield return new WaitForSeconds(3.0f);
        _superBeamAnimator.SetBool("SuperBeamActive", false);
        yield return new WaitForSeconds(1.0f);
        superBeamCollider.offset = new Vector3(0, 0, 0);
        _isSuperBeamActive = false;
        _superBeamPrefab.SetActive(false);
    }

    private IEnumerator SuperMissilePowerDownRoutine()
    {
        yield return new WaitForSeconds(20.0f);
        _isSuperMissileActive = false;
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed = _defaultSpeed;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void RefillAmmo(int ammo)
    { 
        _ammoCount += ammo;
        if (_ammoCount > 15)
        {
            _ammoCount = 15;
        }
        ammo = _ammoCount;

        _uiManager.UpdateAmmoCount(ammo);
    }

    public void RestoreHealth()
    {
        _lives++;
        if (_lives > 3)
        {
            _lives = 3;
        }

        if (_lives == 2)
        {
            _rightEngine.SetActive(false);
        }
        else if (_lives == 3)
        {
            _leftEngine.SetActive(false);
        }

        _uiManager.UpdateLives(_lives);
    }

    public void NegaShroomActive()
    {
        _isNegaShroomActive = true;
        _speed = (_defaultSpeed / _speedMultiplier);
        StartCoroutine(NegaShroomActivePowerDownRoutine());
        
    }

    private IEnumerator NegaShroomActivePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isNegaShroomActive = false;
        _speed = _defaultSpeed;
    }

    private void Thrusters()
    {
        if (_thrusterFuel > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isSpeedBoostActive && !_isNegaShroomActive)
            {
                _isThrusterActive = true;
                _thruster.transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.z); //Adds a little bit of a visual prompt to the thruster.
                _speed = (_defaultSpeed * 1.75f);
                StartCoroutine(DrainThrusterFuel());
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && !_isSpeedBoostActive && !_isNegaShroomActive || _thrusterFuel == 0 && !_isSpeedBoostActive && !_isNegaShroomActive)
            {
                _isThrusterActive = false;
                _thruster.transform.localScale = new Vector3(0.4f, transform.localScale.y, transform.localScale.z); //Removes the visual prompt from the thruster.
                _speed = _defaultSpeed;
            }
        }
    }

    private IEnumerator DrainThrusterFuel()
    {
        while (_isThrusterActive && _thrusterFuel > 0)
        {
            _thrusterFuel--;
            _uiManager.UpdateFuelGuage(_thrusterFuel);
            yield return new WaitForSeconds(.05f);

            if (!_isThrusterActive)
            {
                break;
            }
        }
    }
}