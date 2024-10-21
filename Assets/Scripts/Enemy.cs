using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _minX = -9.5f;
    [SerializeField] private float _maxX = 9.5f;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        /*if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, 8f, 0);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                Destroy(this.gameObject);
                _player.Damage();
            }
        }

        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
