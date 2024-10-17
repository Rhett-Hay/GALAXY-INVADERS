using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        // starting position of the player when the game starts.
        transform.position = new Vector3(0, -3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(_speed * Time.deltaTime, 0, 0));
    }
}
