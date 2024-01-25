using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private PlayerMotor m_PlayerMotor;

    private Camera m_Camera;

    private void Start() {
        m_Camera = Camera.main;
    }

    private void Update() {
        m_PlayerMotor.VelocityDir = m_PlayerInput.GetMovementDir(transform.position, m_Camera);
        float absoluteAngle = m_PlayerInput.GetFaceDir(transform.position, m_Camera, new(0f, 1f));
        m_PlayerMotor.Rotation = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, absoluteAngle) + transform.rotation.eulerAngles.y;
    }
}
