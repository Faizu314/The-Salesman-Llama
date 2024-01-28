using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialProjectile : MonoBehaviour
{

    [SerializeField] private List<Projectile> m_Projectiles;

    private Action<SpecialProjectile> m_OnProjectileDestroyed;
    private Projectile m_ActivatedProjectile;

    public Projectile ActivatedProjectile => m_ActivatedProjectile;

    private void OnEnable()
    {
        foreach (var projectile in m_Projectiles)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    public void Prepare()
    {
        m_ActivatedProjectile = m_Projectiles[UnityEngine.Random.Range(0, m_Projectiles.Count)];
    }

    public void Wield(Transform projectileSocket)
    {
        m_ActivatedProjectile.gameObject.SetActive(true);
        m_ActivatedProjectile.Wield(projectileSocket);
    }

    public float Shoot(Vector3 position, Vector3 forward, Action<SpecialProjectile> onDestroy)
    {
        m_ActivatedProjectile.transform.parent = null;
        m_ActivatedProjectile.Shoot(position, forward, OnProjectileDestroyed);

        m_OnProjectileDestroyed = onDestroy;

        return m_ActivatedProjectile.CooldownTime;
    }

    private void OnProjectileDestroyed(Projectile proj)
    {
        proj.transform.parent = transform;

        proj.transform.localPosition = Vector3.zero;
        proj.transform.localRotation = Quaternion.identity;

        proj.gameObject.SetActive(false);

        m_OnProjectileDestroyed?.Invoke(this);
    }
}
