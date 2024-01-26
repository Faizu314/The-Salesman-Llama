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

    private void Update() {
        if (m_PlayerBounds == null)
            return;

        Bounds bounds = m_PlayerBounds.bounds;
        if (!bounds.Contains(m_Rb.transform.position))
            m_Rb.transform.position = bounds.ClosestPoint(m_Rb.transform.position);
    }

    private void FixedUpdate() {
        m_Rb.rotation = Quaternion.Euler(new(0f, Rotation, 0f));
        m_Rb.velocity = VelocityDir * Speed;
    }

}
