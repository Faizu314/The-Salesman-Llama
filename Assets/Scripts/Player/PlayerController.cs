using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private PlayerMotor m_PlayerMotor;
    [SerializeField] private Thrower m_Thrower;
    [SerializeField] private float m_SpitCooldown;

    private Camera m_Camera;
    private float m_SpitTimestamp;

    private void Start() {
        m_SpitTimestamp = -m_SpitCooldown;
        m_Camera = Camera.main;

        m_PlayerInput.OnSpitButtonPress += Spit;
    }

    private void Update() {
        m_PlayerMotor.VelocityDir = m_PlayerInput.GetMovementDir(m_Camera);
        float absoluteAngle = m_PlayerInput.GetFaceDir(transform.position, m_Camera, new(0f, 1f));
        m_PlayerMotor.Rotation = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, absoluteAngle) + transform.rotation.eulerAngles.y;
    }

    private void Spit() {
        if (Time.time - m_SpitTimestamp < m_SpitCooldown)
            return;

        m_Thrower.Spit();
        m_SpitTimestamp = Time.time;
    }
}
