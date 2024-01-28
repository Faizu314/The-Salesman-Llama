using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

    [SerializeField] private ProjectileData m_ProjectileData;
    public float CooldownTime => m_ProjectileData.Cooldown;
    public Sprite Icon => m_ProjectileData.Icon;

    private Vector3 m_StartPos;
    private Rigidbody m_Rb;
    private Action<Projectile> m_OnDestroy;
    private Collider m_collider;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_collider = GetComponentInChildren<Collider>();
        m_collider.enabled = false;
    }

    public void Shoot(Vector3 position, Vector3 forward, Action<Projectile> onDestroy)
    {
        var soundEvent = FModEvents.Instance.GetEventReference(m_ProjectileData.SpawnSound);
        AudioManager.Instance.PlayOneShotWithParameters(soundEvent, transform.position, m_ProjectileData.Attenuation.x, m_ProjectileData.Attenuation.y);

        m_collider.enabled = true;
        m_Rb.isKinematic = false;

        transform.parent = null;
        transform.position = position;
        transform.forward = forward;

        m_StartPos = position;
        m_OnDestroy = onDestroy;

        VfxSpawner.Instance.SpawnVFX(m_ProjectileData.SpawnParticles, transform.position, forward);

        m_Rb.velocity = forward * m_ProjectileData.Speed;

        StartCoroutine(nameof(CheckRange_Co));
    }

    public void Wield(Transform socket)
    {
        if (m_Rb == null)
            m_Rb = GetComponent<Rigidbody>();
        m_Rb.isKinematic = true;
        transform.parent = socket;
        transform.localPosition = m_ProjectileData.LocalPositionOffset;
        transform.localRotation = m_ProjectileData.LocalRotationOffset;
    }

    private void OnHit()
    {
        m_collider.enabled = false;
        StopAllCoroutines();

        VfxSpawner.Instance.SpawnVFX(m_ProjectileData.HitParticles, transform.position, -transform.forward);

        m_OnDestroy?.Invoke(this);
    }

    private IEnumerator CheckRange_Co()
    {
        var fixedUpdateWait = new WaitForFixedUpdate();

        yield return fixedUpdateWait;

        while (Vector3.SqrMagnitude(m_Rb.position - m_StartPos) < m_ProjectileData.Range * m_ProjectileData.Range)
        {
            yield return fixedUpdateWait;
        }

        m_OnDestroy?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var soundEvent = FModEvents.Instance.GetEventReference(m_ProjectileData.HitSound);
            AudioManager.Instance.PlayOneShotWithParameters(soundEvent, transform.position, m_ProjectileData.Attenuation.x, m_ProjectileData.Attenuation.y);
            //apply damage
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.AddDamage(m_ProjectileData.Damage);
            OnHit();
        }
    }
}
