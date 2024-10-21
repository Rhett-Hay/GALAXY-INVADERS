using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _Xpos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            float randomX = Random.Range(-_Xpos, _Xpos);
            Vector3 posToSpawn = new Vector3(randomX, transform.position.y, 0);
            Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
