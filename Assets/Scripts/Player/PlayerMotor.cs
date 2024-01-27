using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Collider m_PlayerBounds;

    public Vector3 VelocityDir;
    public float Speed;
    public float Rotation;

    private Rigidbody m_Rb;

    private void Start() {
        m_Rb = GetComponent<Rigidbody>();
    }

    private void RestrictMovement() {
        if (m_PlayerBounds == null)
            return;

        Bounds bounds = m_PlayerBounds.bounds;
        if (!bounds.Contains(m_Rb.position))
            m_Rb.MovePosition(bounds.ClosestPoint(m_Rb.position));
    }

    private void Update() {
        RestrictMovement();
    }

    private void FixedUpdate() {
        m_Rb.rotation = Quaternion.Euler(new(m_Rb.rotation.x, Rotation, m_Rb.rotation.z));
        m_Rb.velocity = VelocityDir * Speed;

        //RestrictMovement();
    }

}
