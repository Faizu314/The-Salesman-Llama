using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action OnSpitButtonPress;

    private InputManager m_InputManager;
    private GameInput m_GlobalInput;

    private void Start() {
        m_InputManager = InputManager.Instance;
        m_GlobalInput = m_InputManager.GlobalInput;

        m_GlobalInput.PlayerMovement.Enable();
        m_GlobalInput.PlayerButtons.Enable();

        m_GlobalInput.PlayerButtons.Spit.performed += (x) => OnSpitButtonPress?.Invoke();
    }

    public Vector3 GetMovementDir(Camera playerCam) {
        var inputDir = m_GlobalInput.PlayerMovement.MovementDir.ReadValue<Vector2>();

        if (inputDir.SqrMagnitude() == 0f)
            return Vector3.zero;

        Quaternion inputRot = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(new(inputDir.x, 0f, inputDir.y), Vector3.up);
        Vector3 camForward = playerCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        return inputRot * camForward * inputDir.magnitude;
    }

    /// <param name="baseRotDir">This is a vector on the xz plane which defines the direction the player is looking at when its rotation is 0.</param>
    public float GetFaceDir(Vector3 playerPosition, Camera playerCam, Vector2 baseRotDir) {
        Vector3 mousePos = m_GlobalInput.PlayerMovement.MousePosition.ReadValue<Vector2>();

        mousePos.z = playerCam.nearClipPlane;
        Ray ray = playerCam.ScreenPointToRay(mousePos);
        Plane ground = new(Vector3.up, playerPosition);

        ground.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        Vector3 playerToMouse = point - playerPosition;

        return -Vector2.SignedAngle(baseRotDir, new(playerToMouse.x, playerToMouse.z));
    }

    private void OnDestroy() {
        m_GlobalInput.PlayerMovement.Disable();
        m_GlobalInput.PlayerButtons.Disable();
    }
}
