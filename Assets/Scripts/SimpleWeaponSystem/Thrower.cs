using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Thrower : MonoBehaviour {

    [SerializeField] private Transform m_SpitStartPoint;

    [SerializeField] private Projectile m_SpitPrefab;

    private ObjectPool<Projectile> m_SpitPool;
    private bool m_CanSpit = true;

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
        if (!m_CanSpit)
            return;

        var proj = m_SpitPool.Get();

        proj.Shoot(m_SpitStartPoint.position, m_SpitStartPoint.forward, OnProjectileDestroyed);

        StartCoroutine(nameof(Cooldown_Co), proj.CooldownTime);
    }

    private void OnProjectileDestroyed(Projectile proj) {
        m_SpitPool.Release(proj);
    }

    private IEnumerator Cooldown_Co(float cooldownTime) {
        m_CanSpit = false;

        yield return new WaitForSeconds(cooldownTime);

        m_CanSpit = true;

        OnCooldown();
    }

    private void OnCooldown() {
        AudioManager.Instance.PlayOneShot(FModEvents.Instance.CoolDownNormal, transform.position);
    }
}
