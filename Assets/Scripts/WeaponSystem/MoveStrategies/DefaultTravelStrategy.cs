using UnityEngine;

public class DefaultTravelStrategy : IMoveStrategy {

    private float m_Speed;
    private Rigidbody m_Rb;

    public DefaultTravelStrategy(float speed, Rigidbody rb) {
        m_Speed = speed;
        m_Rb = rb;
    }

    public void Move(Vector3 dir) {
        m_Rb.velocity = m_Speed * Time.fixedDeltaTime * dir;
    }
}
