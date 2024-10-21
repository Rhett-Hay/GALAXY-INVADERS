using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _minX = -9.5f;
    [SerializeField] private float _maxX = 9.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // IF other equals the Player's tag
        // Damage the player,
        // AND destroy ourselves

        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }

        // IF other equals Laser tag
        // Destroy Laser and ourselves

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
