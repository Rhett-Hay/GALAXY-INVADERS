using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _Xpos;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _tripleShotPowerupPrefab;

    private bool _stopSpawning;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-_Xpos, _Xpos);
            Vector3 posToSpawn = new Vector3(randomX, transform.position.y, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-_Xpos, _Xpos);
            Vector3 posToSpawn = new Vector3(randomX, transform.position.y, 0);
            Instantiate(_tripleShotPowerupPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
