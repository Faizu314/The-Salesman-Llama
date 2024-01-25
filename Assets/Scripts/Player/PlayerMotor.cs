using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public Vector3 VelocityDir;
    public float Speed;
    public float Rotation;

    private Transform m_Transform;

    private void Start() {
        m_Transform = transform;
    }

    private void Update() {
        var rot = m_Transform.rotation.eulerAngles;
        rot.y = Rotation;
        m_Transform.rotation = Quaternion.Euler(rot);

        m_Transform.position += VelocityDir * Speed * Time.deltaTime;
    }

}
