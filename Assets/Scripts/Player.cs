using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    private bool _fireLaser = true;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField] GameObject _playerExplosionPreb;

    // Start is called before the first frame update
    void Start()
    {
        // starting position of the player when the game starts.
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

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.3f, 0), 0);

        if (transform.position.x >= 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
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
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

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
        _speed *= _speedMultiplier;
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
        _shieldPrefab.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
        _shieldPrefab.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
