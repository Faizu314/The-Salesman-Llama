using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Projectiles", fileName = "Projectile")]
public class ProjectileData : ScriptableObject {

    public float Speed;
    public float Range;
    public float Damage;
}
