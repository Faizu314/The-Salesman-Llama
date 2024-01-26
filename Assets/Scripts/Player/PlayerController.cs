using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private PlayerMotor m_PlayerMotor;
    [SerializeField] private Ballista m_SpittingBallista;

    private Camera m_Camera;

    private void Start() {
        m_Camera = Camera.main;
    }

    private void Update() {
        m_PlayerMotor.VelocityDir = m_PlayerInput.GetMovementDir(m_Camera);
        float absoluteAngle = m_PlayerInput.GetFaceDir(transform.position, m_Camera, new(0f, 1f));
        m_PlayerMotor.Rotation = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, absoluteAngle) + transform.rotation.eulerAngles.y;

        GetButtons();
    }

    private void GetButtons() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            m_SpittingBallista.Wield();
            m_SpittingBallista.Shoot();
        }
    }
}
