using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BoxCollider m_spawnBox;
    [SerializeField] Transform m_barberShopDestination;
    [SerializeField] Enemy m_enemyPrefab;
    [SerializeField] int m_delayMin;
    [SerializeField] int m_delayMax;

    ObjectPool<Enemy> m_enemyPool;

    CancellationTokenSource tokenSource;

    private void Awake()
    {
        m_enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, null, false);
    }

    private void Start()
    {
        GameManager.Instance.gameStarted += StartSpawnEnemies;
        GameManager.Instance.gameOvered += StopSpawnEnemies;
    }

    private Enemy CreateEnemy()
    {
        Enemy instance = Instantiate(m_enemyPrefab);
        instance.BarberShopDestination = m_barberShopDestination;
        instance.reachDestination += x => m_enemyPool.Release(x);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnGet(Enemy instance)
    {
        float x = m_spawnBox.transform.position.x + m_spawnBox.center.x + Random.Range(-m_spawnBox.bounds.extents.x, m_spawnBox.bounds.extents.x);
        Vector3 pos = new Vector3(x, 0, m_spawnBox.transform.position.z);
        instance.Init(pos, m_spawnBox.transform.rotation);
        instance.gameObject.SetActive(true);
        instance.transform.SetParent(transform, true);
    }

    private void OnRelease(Enemy instance)
    {
        instance.gameObject.SetActive(false);
    }

    public void StartSpawnEnemies()
    {
        tokenSource = new CancellationTokenSource();
        SpawnEnemies(tokenSource).Forget();
    }

    public void StopSpawnEnemies(bool success)
    {
        if (tokenSource != null) tokenSource.Cancel();
    }

    async UniTaskVoid SpawnEnemies(CancellationTokenSource tokenSource)
    {
        while (!tokenSource.IsCancellationRequested)
        {
            m_enemyPool.Get();
            await UniTask.Delay(Random.Range(m_delayMin, m_delayMax), cancellationToken: tokenSource.Token);
        }
    }
}
