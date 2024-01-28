using UnityEngine;
using Phezu.Util;

public class VfxSpawner : Singleton<VfxSpawner>
{
    public void SpawnVFX(ParticleSystem vfx, Vector3 position, Vector3 forward)
    {
        if (vfx == null)
            return;

        var vfxGO = Instantiate(vfx, position, Quaternion.identity);
        vfxGO.transform.forward = forward;
    }
}
