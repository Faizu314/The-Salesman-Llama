using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [SerializeField] private float m_Range;

    protected IMoveStrategy m_MoveStrategy;
    protected IWieldStrategy m_WieldStrategy;

    private IMoveStrategy m_NoMove;
    private Vector3 m_InitialPos;
    private Vector3 m_MoveDir;
    private Action<GameObject> m_ToRelease;

    protected virtual void Awake() {
        m_NoMove = new NoMoveStrategy();
        m_MoveStrategy = m_NoMove;
        m_WieldStrategy = GetWieldStrategy();
    }

    protected virtual IMoveStrategy GetMoveStrategy() {
        return m_NoMove;
    }
    protected virtual IWieldStrategy GetWieldStrategy() {
        return new NoWieldStrategy();
    }

    public void Shoot(Vector3 barrelPos, Vector3 barrelDir, Action<GameObject> toRelease) {
        transform.position = barrelPos;
        transform.forward = barrelDir;
        m_MoveDir = barrelDir;
        m_InitialPos = barrelPos;
        m_ToRelease = toRelease;
    }

    private void Update() {
        if (Vector3.SqrMagnitude(transform.position - m_InitialPos) < m_Range * m_Range)
            Move(m_MoveDir);
        else
            Explode();
    }

    protected virtual void Explode() {
        m_ToRelease?.Invoke(gameObject);
    }

    private void Move(Vector3 dir) {
        m_MoveStrategy.Move(dir);
    }

    public void Wield(Transform wieldSocket) {
        m_WieldStrategy.Wield(wieldSocket);
        m_MoveStrategy = m_NoMove;
    }

    public void Unwield() {
        m_WieldStrategy.Unwield();
        m_MoveStrategy = GetMoveStrategy();
    }
}
 