using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _Xpos;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerupPrefabs;

    private bool _stopSpawning;

    [SerializeField] private GameObject[] _specialPowerupPrefabs;
    [SerializeField] private float _minRandomTime;
    [SerializeField] private float _maxRandomTime;
    [SerializeField] private float _fixedSpawnTime;
    private float _randomX;
    private Vector3 _posToSpawn;

    private void SpawnRange()
    {
        _randomX = Random.Range(-_Xpos, _Xpos);
        _posToSpawn = new Vector3(_randomX, transform.position.y, 0);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            SpawnRange();
            GameObject newEnemy = Instantiate(_enemyPrefab, _posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            SpawnRange();
            float waitTime = Random.Range(_minRandomTime, _maxRandomTime);
            yield return new WaitForSeconds(waitTime);
            SpawnPowerup(_powerupPrefabs);
        }
    }

    IEnumerator SpawnSpecialPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            SpawnRange();          
            yield return new WaitForSeconds(_fixedSpawnTime);
            SpawnPowerup(_specialPowerupPrefabs);
        }
    }

    private void SpawnPowerup(GameObject[] powerupArray)
    {
        if (powerupArray.Length == 0) return;

        GameObject powerupToSpawn = powerupArray[Random.Range(0, powerupArray.Length)];
        GameObject newSpawnPowerup = Instantiate(powerupToSpawn, _posToSpawn, Quaternion.identity);
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSpecialPowerupRoutine());
    }
}
