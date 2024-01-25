using UnityEngine;
using UnityEngine.Pool;

public class Ballista : MonoBehaviour
{
    [SerializeField] private Transform m_Barrel;
    [SerializeField] private Transform m_WieldSocket;
    [SerializeField] private float m_CooldownTime;
    [SerializeField] private GameObject m_ProjectilePrefab;

    private ObjectPool<GameObject> m_ProjectilesPool;

    private float m_PrevShotTimestamp;
    private BaseProjectile m_CurrentlyWielded;

    private void Start() {
        m_PrevShotTimestamp = -m_CooldownTime;

        GameObject OnCreate() {
            var obj = Instantiate(m_ProjectilePrefab);
            return obj;
        }
        void OnGet(GameObject obj) {
            obj.SetActive(true);
        }
        void OnRelease(GameObject obj) {
            obj.SetActive(false);
        }

        m_ProjectilesPool = new(OnCreate, OnGet, OnRelease, null, true, 10, 10);
    }

    public void Shoot() {
        if (m_CurrentlyWielded == null)
            return;
        if (Time.time - m_PrevShotTimestamp <= m_CooldownTime)
            return;

        m_CurrentlyWielded.Unwield();
        m_CurrentlyWielded.Shoot(m_Barrel.position, m_Barrel.forward, (x) => m_ProjectilesPool.Release(x));
        m_CurrentlyWielded = null;

        m_PrevShotTimestamp = Time.time;
    }

    public void Wield() {
        if (Time.time - m_PrevShotTimestamp <= m_CooldownTime)
            return;

        var obj = m_ProjectilesPool.Get();
        m_CurrentlyWielded = obj.GetComponent<BaseProjectile>();
        m_CurrentlyWielded.Wield(m_WieldSocket);
    }
}
