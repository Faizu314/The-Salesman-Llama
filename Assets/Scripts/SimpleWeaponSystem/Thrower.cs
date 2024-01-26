using UnityEngine;
using UnityEngine.Pool;

public class Thrower : MonoBehaviour {

    [SerializeField] private Transform m_SpitStartPoint;

    [SerializeField] private Projectile m_SpitPrefab;

    private ObjectPool<Projectile> m_SpitPool;

    private void Start() {
        m_SpitPool = new(CreateProjectile, GetProjectile, ReleaseProjectile, defaultCapacity: 10, maxSize: 10);
    }

    private Projectile CreateProjectile() {
        var proj = Instantiate(m_SpitPrefab);
        return proj;
    }

    private void GetProjectile(Projectile proj) {
        proj.gameObject.SetActive(true);
    }

    private void ReleaseProjectile(Projectile proj) {
        proj.gameObject.SetActive(false);
    }

    public void Spit() {
        var proj = m_SpitPool.Get();

        proj.Shoot(m_SpitStartPoint.position, m_SpitStartPoint.forward, (x) => m_SpitPool.Release(x));
    }
}
