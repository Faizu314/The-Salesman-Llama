using Phezu.Util;

public class InputManager : Singleton<InputManager>
{
    private GameInput m_GameInput;
    public GameInput GlobalInput => m_GameInput;

    private void Awake() {
        m_GameInput = new();
        m_GameInput.Enable();
    }

    protected override void OnDestroy() {
        m_GameInput.Disable();
    }
}
