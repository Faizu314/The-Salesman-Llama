using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpecialThrower : MonoBehaviour {

    [SerializeField] private Transform m_SpecialAttackSocket;
    [SerializeField] private Transform m_AttackStartPoint;

    [SerializeField] private SpecialProjectile m_SpecialProjectilePrefab;

    private ObjectPool<SpecialProjectile> m_SpecialProjectilesPool;
    private SpecialProjectile m_LoadedProjectile;
    private bool m_CanThrow = true;

    private void Start() {
        m_SpecialProjectilesPool = new(CreateProjectile, GetProjectile, ReleaseProjectile, defaultCapacity: 10, maxSize: 10);

        StartCoroutine(nameof(Test_Co));
    }

    private IEnumerator Test_Co() {
        yield return new WaitForSeconds(1f);
        LoadSpecialAttack();
    }

    private SpecialProjectile CreateProjectile() {
        var proj = Instantiate(m_SpecialProjectilePrefab);
        return proj;
    }

    private void GetProjectile(SpecialProjectile proj) {
        proj.gameObject.SetActive(true);
    }

    private void ReleaseProjectile(SpecialProjectile proj) {
        proj.gameObject.SetActive(false);
    }

    private void LoadSpecialAttack() {
        m_LoadedProjectile = m_SpecialProjectilesPool.Get();
        m_LoadedProjectile.Wield(m_SpecialAttackSocket);
    }

    public void SpecialAttack() {
        if (!m_CanThrow)
            return;

        float coolDown = m_LoadedProjectile.Shoot(m_AttackStartPoint.position, m_AttackStartPoint.forward, (x) => m_SpecialProjectilesPool.Release(x));

        m_CanThrow = false;
        StartCoroutine(nameof(Cooldown_Co), coolDown);
    }

    private IEnumerator Cooldown_Co(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime);

        m_CanThrow = true;

        OnCooldown();
    }

    private void OnCooldown() {
        AudioManager.Instance.PlayOneShot(FModEvents.Instance.CoolDownSpecial, transform.position);
        LoadSpecialAttack();
    }
}
