using UnityEngine;

public interface IWieldStrategy {
    void Wield(Transform wieldSocket);
    void Unwield();
}
