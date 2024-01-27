using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {

    [SerializeField] private ProjectileData m_ProjectileData;
    [SerializeField] private ParticleSystem m_ShootParticles;
    [SerializeField] private ParticleSystem m_HitParticles;

    public float CooldownTime => m_ProjectileData.Cooldown;

    private Vector3 m_StartPos;
    private Rigidbody m_Rb;
    private Action<Projectile> m_OnDestroy;

    private void Awake() {
        m_Rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 position, Vector3 forward, Action<Projectile> onDestroy) {
        m_Rb.isKinematic = false;

        transform.position = position;
        transform.forward = forward;

        m_StartPos = position;
        m_OnDestroy = onDestroy;

        if (m_ShootParticles != null)
            m_ShootParticles.Play();

        m_Rb.velocity = forward * m_ProjectileData.Speed;

        StartCoroutine(nameof(CheckRange_Co));
    }

    public void Wield(Transform socket) {
        m_Rb.isKinematic = true;
        transform.parent = socket;
        transform.localPosition = m_ProjectileData.LocalPositionOffset;
        transform.localRotation = m_ProjectileData.LocalRotationOffset;
    }

    private void OnHit() {
        StopAllCoroutines();

        if (m_HitParticles != null)
            m_HitParticles.Play();

        m_OnDestroy?.Invoke(this);
    }

    private IEnumerator CheckRange_Co() {
        var fixedUpdateWait = new WaitForFixedUpdate();

        yield return fixedUpdateWait;

        while (Vector3.SqrMagnitude(m_Rb.position - m_StartPos) < m_ProjectileData.Range * m_ProjectileData.Range) {
            yield return fixedUpdateWait;
        }

        OnHit();
    }

    private void OnCollisionEnter(Collision collision) {
        collision.gameObject.TryGetComponent<Enemy>(out var enemy);

        if (enemy != null) {
            //apply damage
        }

        OnHit();
    }
}
