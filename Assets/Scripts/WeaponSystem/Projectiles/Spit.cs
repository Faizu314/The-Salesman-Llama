using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spit : BaseProjectile
{
    [SerializeField] private float m_Speed;

    private void OnCollisionEnter(Collision collision) {
        Explode();
    }

    protected override IMoveStrategy GetMoveStrategy() {
        return new DefaultTravelStrategy(m_Speed, GetComponent<Rigidbody>());
    }
}
