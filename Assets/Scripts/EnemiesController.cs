using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private List<Sprite> AllEnemies; // changed to SO, left for reference
    [SerializeField] private List<EnemyDefinition> AllEnemyDefinitions;
    [SerializeField] private List<SpawnPoint> SpawnPoints;
    [SerializeField] private GameObject EnemyPrefab;

    private int _maxEnemies = 3;
    private int _currentEnemies = 0;
    private bool _selectNextEnemy = true; // true to select first spawned enemy

    private List<IEnemy> _currentEnemyList = new List<IEnemy>();

    private void Awake()
    {
        ConfigureEnemiesController();
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void OnEnable()
    {
        AttachListeners();
    }

    private void OnDisable()
    {
        DettachListeners();
    }

    private void AttachListeners()
    {
        GameEvents.EnemyKilled += EnemyKilled;
    }

    private void DettachListeners()
    {
        GameEvents.EnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled(IEnemy enemy, DamageType damageType)
    {
        _currentEnemyList.Remove(enemy);
        if(_currentEnemyList.Count > 0)
            _currentEnemyList[0].SelectMe();
        else
            _selectNextEnemy = true;
        FreeSpawnPoint(enemy.GetEnemyPosition());
        DestroyKilledEnemy(enemy.GetEnemyObject());
        StartCoroutine(SpawnEnemyViaCor());
        GameControlller.Instance.score += enemy.GetScore(damageType);
    }

    private void SpawnEnemies()
    {
        while (_currentEnemies < _maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnEnemyViaCor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (_currentEnemies >= _maxEnemies)
        {
            Debug.LogError("Max Enemies reached! Kil some to spawn new");
            return;
        }

        int freeSpawnPointIndex = -1;
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (SpawnPoints[i].IsOccupied) continue;
            
            freeSpawnPointIndex = i;
            break;
        }

        if (freeSpawnPointIndex == -1) return;
        
        SpawnPoints[freeSpawnPointIndex].IsOccupied = true;
        SoulEnemy enemy = Instantiate(EnemyPrefab, SpawnPoints[freeSpawnPointIndex].Position.position, Quaternion.identity, transform).GetComponent<SoulEnemy>();
        int enemyIndex = Random.Range(0, AllEnemyDefinitions.Count);
        enemy.SetupEnemy(AllEnemyDefinitions[enemyIndex], SpawnPoints[freeSpawnPointIndex]);
        if (_selectNextEnemy)
        {
            enemy.SelectMe();
            _selectNextEnemy = false;
        }
        _currentEnemies++;
        _currentEnemyList.Add(enemy);
    }

    private void DestroyKilledEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }

    private void FreeSpawnPoint(SpawnPoint spawnPoint)
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (spawnPoint != SpawnPoints[i]) continue;
            
            SpawnPoints[i].IsOccupied = false;
            _currentEnemies--;
            break;
        }
    }

    private void ConfigureEnemiesController()
    {
        _maxEnemies = SpawnPoints != null ? SpawnPoints.Count : 3;
    }

}

[System.Serializable]
public class SpawnPoint
{
    public Transform Position;
    public bool IsOccupied;
}