using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpecialThrower : MonoBehaviour
{

    [SerializeField] private Transform m_SpecialAttackSocket;
    [SerializeField] private Transform m_AttackStartPoint;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SpecialProjectile m_SpecialProjectilePrefab;
    [SerializeField] private PlayerAnimationListener m_animationListener;
    [SerializeField] private SpecialAttackInicatorUI m_indicatorUI;

    private ObjectPool<SpecialProjectile> m_SpecialProjectilesPool;
    private SpecialProjectile m_LoadedProjectile;
    private bool m_CanThrow = true;

    private void Start()
    {
        m_animationListener.throwProjectile += () => StartCoroutine(nameof(Shoot));
        m_SpecialProjectilesPool = new(CreateProjectile, GetProjectile, ReleaseProjectile, defaultCapacity: 10, maxSize: 10);
        StartCoroutine(nameof(Cooldown_Co), 2);
    }

    private IEnumerator Test_Co()
    {
        yield return new WaitForSeconds(1f);
        LoadSpecialAttack();
    }

    private SpecialProjectile CreateProjectile()
    {
        var proj = Instantiate(m_SpecialProjectilePrefab);
        return proj;
    }

    private void GetProjectile(SpecialProjectile proj)
    {
        proj.Prepare();
        m_indicatorUI.EnableIcon(proj.ActivatedProjectile.Icon);
    }

    private void ReleaseProjectile(SpecialProjectile proj)
    {
        proj.gameObject.SetActive(false);
    }

    private void PrepareNextSpecialAttack()
    {
        Debug.Log("Prepare");
        m_LoadedProjectile = m_SpecialProjectilesPool.Get();
        AudioManager.Instance.PlayOneShot(FModEvents.Instance.CoolDownSpecial, transform.position);
    }

    private void LoadSpecialAttack()
    {
        Debug.Log("Load " + m_LoadedProjectile.ActivatedProjectile.gameObject.name, m_LoadedProjectile.ActivatedProjectile.gameObject);
        m_indicatorUI.DisableIcon();
        if (m_LoadedProjectile == null) m_LoadedProjectile = m_SpecialProjectilesPool.Get();
        m_LoadedProjectile.Wield(m_SpecialAttackSocket);
        m_LoadedProjectile.gameObject.SetActive(true);
        m_Animator.SetBool("Throw", true);
    }

    public void SpecialAttack()
    {
        if (!m_CanThrow)
            return;

        LoadSpecialAttack();
    }

    private IEnumerator Shoot()
    {
        float coolDown = m_LoadedProjectile.Shoot(m_AttackStartPoint.position, m_AttackStartPoint.forward, (x) => m_SpecialProjectilesPool.Release(x));

        m_Animator.SetBool("Throw", false);
        m_CanThrow = false;
        yield return Cooldown_Co(coolDown);
    }

    private IEnumerator Cooldown_Co(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);

        m_CanThrow = true;

        PrepareNextSpecialAttack();
    }
}
