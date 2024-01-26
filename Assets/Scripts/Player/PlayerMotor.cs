using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    public Vector3 VelocityDir;
    public float Speed;
    public float Rotation;

    private Rigidbody m_Rb;

    private void Start() {
        m_Rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        m_Rb.rotation = Quaternion.Euler(new(0f, Rotation, 0f));
        m_Rb.velocity = VelocityDir * Speed;
    }

}
