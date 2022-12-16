using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    // List to store the instantiated enemies
    private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    private Transform _playerPos;

    // Offset distance to move ahead of the player
    [SerializeField]
    private float _offset = 5f;

    // Max number of enemies to spawn
    [SerializeField]
    private int _maxEnemies = 3;

    // Time interval between enemy spawns
    [SerializeField]
    private float _spawnInterval = 2f;

    // Time interval to wait before spawning more enemies if the max number has been reached
    [SerializeField]
    private float _waitInterval = 5f;

    void Update()
    {
        // Only spawn enemies if the max number hasn't been reached, or if all enemies have been destroyed
        if (_enemies.Count < _maxEnemies || _enemies.FindAll(e => e == null).Count == _enemies.Count)
        {
            // Calculate the spawn position ahead of the player
            Vector3 spawnPosition = _playerPos.position + Vector3.right * 6f;

            // Set the spawner's position to the calculated spawn position
            transform.position = spawnPosition;

            // Choose a random number of enemies to spawn, between 1 and 3
            int numEnemiesToSpawn = Random.Range(1, 4);

            // Spawn the chosen number of enemies
            for (int i = 0; i < numEnemiesToSpawn; i++)
            {
                GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                _enemies.Add(enemy);
            }

            // Start the enemy spawning coroutine
            StartCoroutine(WaitAndSpawnEnemies());
        }
    }

    IEnumerator WaitAndSpawnEnemies()
    {
        // Choose a random wait interval before starting the enemy spawning coroutine
        float waitInterval = Random.Range(1f, 10f);
        yield return new WaitForSeconds(waitInterval);

        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // Wait for the spawn interval before spawning the next enemy
        yield return new WaitForSeconds(_spawnInterval);

        // If the max number of enemies has been reached, wait for the wait interval before spawning more
        if (_enemies.Count == _maxEnemies)
        {
            yield return new WaitForSeconds(_waitInterval);
        }
    }
}
