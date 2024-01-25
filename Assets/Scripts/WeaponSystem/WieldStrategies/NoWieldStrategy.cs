using UnityEngine;

public class NoWieldStrategy : IWieldStrategy {
    public void Unwield() {
        return;
    }

    public void Wield(Transform wieldSocket) {
        return;
    }
}
