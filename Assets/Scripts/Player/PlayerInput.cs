using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputManager m_InputManager;

    private GameInput m_GlobalInput;

    private void Start() {
        m_InputManager = InputManager.Instance;
        m_GlobalInput = m_InputManager.GlobalInput;

        m_GlobalInput.PlayerMovement.Enable();
    }

    public Vector3 GetMovementDir(Vector3 playerPosition, Camera playerCam) {
        var inputDir = m_GlobalInput.PlayerMovement.MovementDir.ReadValue<Vector2>();

        Quaternion inputRot = Quaternion.FromToRotation(Vector3.forward, new(inputDir.x, 0f, inputDir.y));
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
    }
}
