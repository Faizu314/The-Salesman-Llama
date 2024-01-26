using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform m_spawnPoint;
    [SerializeField] Transform m_destinationPoint;
    [SerializeField] Enemy m_enemyPrefab;

    ObjectPool<Enemy> m_enemyPool;

    CancellationToken token;

    private void Awake()
    {
        m_enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, null, false);
        StartSpawnEnemies();
    }

    private Enemy CreateEnemy()
    {
        Enemy instance = Instantiate(m_enemyPrefab);
        instance.reachDestination += x => m_enemyPool.Release(x);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGet(Enemy instance)
    {
        instance.Init(m_spawnPoint, m_destinationPoint.position);
        instance.gameObject.SetActive(true);
        instance.transform.SetParent(transform, true);
    }

    private void OnRelease(Enemy instance)
    {
        instance.gameObject.SetActive(false);
    }

    public void StartSpawnEnemies()
    {
        token = new CancellationToken();
        SpawnEnemies(token).Forget();
    }

    async UniTaskVoid SpawnEnemies(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            m_enemyPool.Get();
            await UniTask.Delay(Random.Range(500, 2000), cancellationToken: token);
        }
    }
}
