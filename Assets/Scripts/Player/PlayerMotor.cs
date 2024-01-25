using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public Vector3 Velocity;
    public float Rotation;

    private Transform m_Transform;

    private void Start() {
        m_Transform = transform;
    }

    private void Update() {
        var rot = m_Transform.rotation.eulerAngles;
        rot.y = Rotation;
        m_Transform.rotation = Quaternion.Euler(rot);

        m_Transform.position += Velocity * Time.deltaTime;
    }

}
