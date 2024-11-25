using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private float _maxBoundary;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField] GameObject _tripleShotPrefab;
    private bool _fireLaser = true;   
    [SerializeField] AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField] GameObject _playerExplosionPreb;
    

    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private float _thrusterDuration = 5.0f;
    [SerializeField] private float _thrusterCoolDown = 5f;
    [SerializeField] private Slider _thrusterSlider;
    private float _thrusterTimer;
    private float _coolDownTimer;
    private bool _isThrusterActive = false;
    private bool _onCoolDown = false;
    [SerializeField] private float _currentSpeed;
    private bool _shieldHit;

    [SerializeField] private GameObject _shieldPrefab;
    private bool _isShieldActive = false;
    private SpriteRenderer _shieldRenderer;
    private int _shieldStrength = 3;
    private Color[] _shieldColors = { Color.red, Color.yellow, Color.white };

    

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _speed;
        _thrusterTimer = _thrusterDuration;
        _thrusterSlider.maxValue = _thrusterDuration;
        _thrusterSlider.value = _thrusterDuration;

        _shieldRenderer = _shieldPrefab.GetComponent<SpriteRenderer>();
        _shieldPrefab.SetActive(false);

        transform.position = new Vector3(0, -3, 0);

        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL!");
        }

        _uiManager = GameObject.FindObjectOfType<UIManager>().GetComponent<UIManager>();
        
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CalculateBoost();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }      
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _currentSpeed * Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _maxBoundary, 0), 0);

        if (transform.position.x >= 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
    }

    private void CalculateBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) &&  _thrusterTimer > 0f && !_onCoolDown)
        {
            _isThrusterActive = true;
            _currentSpeed = _speed * _speedMultiplier;

            _thrusterTimer -= Time.deltaTime;
            _thrusterTimer = Mathf.Clamp(_thrusterTimer, 0f, _thrusterDuration);
            _thrusterSlider.value = _thrusterTimer;

            if (_thrusterTimer <= 0f)
            {
                StartCoolDown();
            }
        }
        else
        {
            _isThrusterActive = false;
            _currentSpeed = _speed;

            if (!_onCoolDown && _thrusterTimer < _thrusterDuration)
            {
                _thrusterTimer += Time.deltaTime;
                _thrusterTimer = Mathf.Clamp(_thrusterTimer, 0f, _thrusterDuration);
                _thrusterSlider.value = _thrusterTimer;
            }
        }

        if (_onCoolDown)
        {
            _coolDownTimer -= Time.deltaTime;
            if (_coolDownTimer <= 0f)
            {
                _onCoolDown = false;
            }
        }
    }

    private void StartCoolDown()
    {
        _onCoolDown = true;
        _coolDownTimer = _thrusterCoolDown;
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_canFire > Time.time && _fireLaser)
        {
            if (_isTripleShotActive)
            {
                TripleShotActive();
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            }

            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }        
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_playerExplosionPreb, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        // Set Triple shot to True
        // Start the power down coroutine for 5 seconds
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        // Wait 5 seconds, then Triple shot to False
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier * 1.5f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldStrength = 3;
        UpdateShieldColor();
        _shieldPrefab.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    public void ShieldHit()
    {
        _shieldStrength--;
        UpdateShieldColor();

        if (_shieldStrength <= 0)
        {
            _isShieldActive = false;
            _shieldPrefab.SetActive(false);
            _shieldStrength = 0;
        }
    }

    private void UpdateShieldColor()
    {
        if (_shieldStrength > 0 && _shieldStrength <= _shieldColors.Length)
        {
            _shieldRenderer.color = _shieldColors[_shieldStrength - 1];
        }
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
        _shieldPrefab.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser") || other.CompareTag("Enemy"))
        {
            if (_isShieldActive)
            {
                ShieldHit();
            }
            
            Destroy(other.gameObject);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
