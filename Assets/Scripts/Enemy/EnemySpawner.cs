using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] m_spawnPoints;
    [SerializeField] GameObject[] m_enemyPrefabs;

    List<GameObject> m_slowEnemyPool;
    List<GameObject> m_mediumEnemyPool;
    List<GameObject> m_fastEnemyPool;

    CancellationToken slowToken;
    CancellationToken mediumToken;
    CancellationToken fastToken;

    private void Awake()
    {
        m_slowEnemyPool = new List<GameObject>();
        m_mediumEnemyPool = new List<GameObject>();
        m_fastEnemyPool = new List<GameObject>();

        StartSpawnEnemies();
    }

    public void StartSpawnEnemies()
    {
        slowToken = new CancellationToken();
        SpawnSlowEnemies(slowToken).Forget();
        mediumToken = new CancellationToken();
        SpawnMediumEnemies(mediumToken).Forget();
        fastToken = new CancellationToken();
        SpawnFastEnemies(fastToken).Forget();
    }

    async UniTaskVoid SpawnSlowEnemies(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            GameObject enemy = null;
            for (int i = 0; i < m_slowEnemyPool.Count; i++)
            {
                if (!m_slowEnemyPool[i].activeSelf)
                {
                    enemy = m_slowEnemyPool[i];
                    break;
                }
            }
            if (enemy == null)
            {
                enemy = Instantiate(m_enemyPrefabs[0]);
                m_slowEnemyPool.Add(enemy);
            }
            enemy.transform.position = m_spawnPoints[0].position;
            enemy.transform.rotation = m_spawnPoints[0].rotation;
            enemy.SetActive(true);

            await UniTask.Delay(Random.Range(500, 2000), cancellationToken: token);
        }
    }

    async UniTaskVoid SpawnMediumEnemies(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            GameObject enemy = null;
            for (int i = 0; i < m_mediumEnemyPool.Count; i++)
            {
                if (!m_mediumEnemyPool[i].activeSelf)
                {
                    enemy = m_mediumEnemyPool[i];
                    break;
                }
            }
            if (enemy == null)
            {
                enemy = Instantiate(m_enemyPrefabs[1]);
                m_mediumEnemyPool.Add(enemy);
            }
            enemy.transform.position = m_spawnPoints[1].position;
            enemy.transform.rotation = m_spawnPoints[1].rotation;
            enemy.SetActive(true);

            await UniTask.Delay(Random.Range(1000, 2500), cancellationToken: token);
        }
    }

    async UniTaskVoid SpawnFastEnemies(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            GameObject enemy = null;
            for (int i = 0; i < m_fastEnemyPool.Count; i++)
            {
                if (!m_fastEnemyPool[i].activeSelf)
                {
                    enemy = m_fastEnemyPool[i];
                    break;
                }
            }
            if (enemy == null)
            {
                enemy = Instantiate(m_enemyPrefabs[2]);
                m_fastEnemyPool.Add(enemy);
            }
            enemy.transform.position = m_spawnPoints[2].position;
            enemy.transform.rotation = m_spawnPoints[2].rotation;
            enemy.SetActive(true);

            await UniTask.Delay(Random.Range(1500, 3000), cancellationToken: token);
        }
    }
}
