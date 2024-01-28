using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Projectiles", fileName = "Projectile")]
public class ProjectileData : ScriptableObject
{
    [Header("Shoot Data")]
    public float Cooldown;
    public float Speed;
    public float Range;
    public float Damage;

    [Space(5f)]
    [Header("Socketing Data")]
    public Vector3 LocalPositionOffset;
    public Quaternion LocalRotationOffset;

    [Space(5f)]
    [Header("UI Data")]
    public Sprite Icon;

    [Space(5f)]
    [Header("Sound Data")]
    public FModEvents.EventReferenceType HitSound;
    public FModEvents.EventReferenceType SpawnSound;

    [Space(5f)]
    [Header("Particle Data")]
    public VfxSpawner.VFX HitParticles;
    public VfxSpawner.VFX SpawnParticles;
}
