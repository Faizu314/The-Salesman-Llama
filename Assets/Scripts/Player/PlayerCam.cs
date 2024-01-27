using Cinemachine;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private CinemachineVirtualCamera m_PlayerCam;

    [SerializeField] private Vector2 m_ZOffsetRange;
    [SerializeField] private float m_MovementSpeed;

    private Cinemachine3rdPersonFollow m_CamFollow;

    private void Start() {
        m_CamFollow = m_PlayerCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        m_CamFollow.ShoulderOffset.z = 0f;
    }

    private void LateUpdate() {
        int dir = m_PlayerInput.GetCamMovementDir();

        float z = m_CamFollow.ShoulderOffset.z;
        z += dir * m_MovementSpeed * Time.deltaTime;
        z = Mathf.Clamp(z, m_ZOffsetRange.x, m_ZOffsetRange.y);

        m_CamFollow.ShoulderOffset.z = z;
    }

}
