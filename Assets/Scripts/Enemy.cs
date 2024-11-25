using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _minX = -9.5f;
    [SerializeField] private float _maxX = 9.5f;
    [SerializeField] private int _enemyPoints;

    private Player _player;
    private Animator _anim;
    private Collider2D _collider;
    private Enemy _enemy;
    private AudioSource _audioSource;

    private float _fireRate;
    private float _canFire = -1f;
    [SerializeField] private float _minFireRate;
    [SerializeField] private float _maxFireRate;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private GameObject _enemyLaser;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }

        _enemy = GetComponent<Enemy>();
        if (_enemy == null)
        {
            Debug.LogError("Enemy script component is NULL!");
        }

        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("Collider is NULL!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyMovement();
        EnemyFireLaser();
    }

    private void EnemyFireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(_minFireRate, _maxFireRate);
            _canFire = Time.time + _fireRate;

            _enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser(true);
            }

        }
    }

    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7f)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(_enemy);
            _audioSource.Play();
            Destroy(this.gameObject, 2.35f);
            _player.AddScore(_enemyPoints);
        }

        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(_enemyPoints);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(_enemy);
            Destroy(_collider);
            Destroy(this.gameObject, 2.35f);
        }
    }
}
