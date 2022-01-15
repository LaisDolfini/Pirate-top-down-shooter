using System.Collections;
using System.Collections.Generic;
using PirateTopDown.Pathfind;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [Header("Required Components------------")]
    [SerializeField] private SessionManager _sessionManager;
    [SerializeField] private PirateTopDown.Pathfind.Grid _pathfindingGrid;
    [SerializeField] private BulletsManager _enemiesBulletsManager;
    [SerializeField] private Transform _player;
    
    [Header("Enemies Required Components------------")]
    [SerializeField] private Sprite _enemiesSprite;
    private float _enemiesHeight;

    [SerializeField] private GameObject _shooterPrefab, _chaserPrefab;
    private List<GameObject> _shootersSpawnedList = new List<GameObject>();
    private List<GameObject> _chasersSpawnedList = new List<GameObject>();
    private List<Life> _shootersLifeList = new List<Life>();
    private List<Life> _chasersLifeList = new List<Life>();

    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    private float _enemiesSpawnTime, _spawnTimer;

    void Awake()
    {
        _enemiesSpawnTime = PlayerPrefs.GetFloat("EnemiesSpawnTime");
        _enemiesHeight = _enemiesSprite.bounds.extents.y;

        if(_shootersSpawnedList.Count != 0) EnemiesInitialSetup(_shootersSpawnedList, _shootersLifeList, true);
        if(_chasersSpawnedList.Count != 0) EnemiesInitialSetup(_chasersSpawnedList, _chasersLifeList, false);
    }

    void EnemiesInitialSetup(List<GameObject> enemiesList, List<Life> lifeList, bool isShooters)
    {
        int count = enemiesList.Count;
        for(int i = 0; i < count; i++)
        {
            enemiesList[i].GetComponent<Pathfinding>().Setup(_pathfindingGrid, _player);
            Life newLife = enemiesList[i].GetComponent<Life>();
            newLife.onDied += _sessionManager.SetScore;
            lifeList.Add(newLife);

            if(isShooters) enemiesList[i].GetComponent<ShotAttack>().ShotAttackSetup(_enemiesBulletsManager, _enemiesHeight, _player);
        }
    }

    void OnDisable()
    {
        int shootersCount = _shootersLifeList.Count;
        for(int i = 0; i < shootersCount; i++)
        {
            _shootersLifeList[i].onDied -= _sessionManager.SetScore;
        }

        int chasersCount = _chasersLifeList.Count;
        for(int i = 0; i < chasersCount; i++)
        {
            _chasersLifeList[i].onDied -= _sessionManager.SetScore;
        }
    }

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if(_spawnTimer >= _enemiesSpawnTime)
        {
            SpawnEnemy();
            _spawnTimer = 0;
        }
    }

    void SpawnEnemy()
    {
        GameObject chosenEnemy = null;
        List<GameObject> enemiesList = new List<GameObject>();
        List<Life> lifeList = new List<Life>();
        int listNumber = 0;
        int random = Random.Range(0, 2);

        switch(random)
        {
            case 0:
            enemiesList = _shootersSpawnedList;
            lifeList = _shootersLifeList;
            if(_shootersSpawnedList.Count == 0) break;
            chosenEnemy = VerifyEnemiesPool(_shootersSpawnedList, listNumber);        
            break;
            case 1:
            enemiesList = _chasersSpawnedList;
            lifeList = _chasersLifeList;
            if(_shootersSpawnedList.Count == 0) break;
            chosenEnemy = VerifyEnemiesPool(_chasersSpawnedList, listNumber);    
            break;
        }

        if(chosenEnemy != null) InstantiateEnemy(chosenEnemy, enemiesList, lifeList, listNumber);
        else
        {
            if(random == 0) InstantiateNewEnemy(_shooterPrefab, enemiesList, lifeList, random);
            else InstantiateNewEnemy(_chaserPrefab, enemiesList, lifeList, random);
        }
    }
    
    void InstantiateEnemy(GameObject enemyType, List<GameObject> enemiesList, List<Life> lifeList, int listNumber)
    {
        Vector3 spawnPos = RandomizedSpawnPosition();
        enemyType.transform.position = spawnPos;
        lifeList[listNumber].Revive();
        enemyType.SetActive(true);
    }

    void InstantiateNewEnemy(GameObject enemyType, List<GameObject> enemiesList, List<Life> lifeList, int random)
    {
        Vector3 spawnPos = RandomizedSpawnPosition();
        GameObject newEnemy = Instantiate(enemyType, spawnPos, Quaternion.identity, transform);
        newEnemy.GetComponent<Pathfinding>().Setup(_pathfindingGrid, _player);
        Life newLife = newEnemy.GetComponent<Life>();
        newLife.onDied += _sessionManager.SetScore;
        lifeList.Add(newLife);
        enemiesList.Add(newEnemy);

        if(random == 0) newEnemy.GetComponent<ShotAttack>().ShotAttackSetup(_enemiesBulletsManager, _enemiesHeight, _player);
    } 

    GameObject VerifyEnemiesPool(List<GameObject> enemiesList, int listNumber)
    {
        int count = enemiesList.Count;
        for(int i = 0; i < count; i++)
        {
            if(enemiesList[i].activeSelf) continue;

            listNumber = i;
            return enemiesList[i];
        }
        return null;
    }

    Vector3 RandomizedSpawnPosition()
    {
        int random = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[random].position;
    }
}
