using UnityEngine;

public class DefaultWieldStrategy : IWieldStrategy {

    private Vector3 m_WieldOffset;
    private Vector3 m_WieldRotation;
    private Transform m_Transform;

    public DefaultWieldStrategy(Transform transform, Vector3 wieldOffset, Vector3 wieldRotation) {
        m_WieldOffset = wieldOffset;
        m_WieldRotation = wieldRotation;
        m_Transform = transform;
    }

    public void Wield(Transform wieldSocket) {
        m_Transform.parent = wieldSocket;
        m_Transform.localPosition = m_WieldOffset;
        m_Transform.localRotation = Quaternion.Euler(m_WieldRotation);
    }

    public void Unwield() {
        m_Transform.parent = null;
    }
}
