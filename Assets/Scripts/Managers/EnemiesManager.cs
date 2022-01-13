using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private float _enemiesSpawnTime;
    private float _spawnTimer;

    [SerializeField] private GameObject _shooterPrefab, _chaserPrefab;

    private List<GameObject> _shipsSpawnedList = new List<GameObject>();

    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    void Awake()
    {
        _enemiesSpawnTime = PlayerPrefs.GetFloat("EnemiesSpawnTime");
    }

    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if(_spawnTimer >= _enemiesSpawnTime)
        {
            SpawnEnemies();
            _spawnTimer = 0;
        }
    }

    void SpawnEnemies()
    {

    }
}
