using UnityEngine;

public class DefaultMoveStrategy : IMoveStrategy {

    private float m_Speed;
    private Rigidbody m_Rb;

    public DefaultMoveStrategy(float speed, Rigidbody rb) {
        m_Speed = speed;
        m_Rb = rb;
    }

    public void Move(Vector3 dir) {
        m_Rb.velocity = m_Speed * dir;
    }
}
