using UnityEngine;
using Phezu.Util;

public class VfxSpawner : Singleton<VfxSpawner>
{
    [SerializeField] private ParticleSystem m_SpitHit;
    [SerializeField] private ParticleSystem m_SpitSpawn;
    [SerializeField] private ParticleSystem m_SpecialHit;
    [SerializeField] private ParticleSystem m_SpecialSpawn;

    public enum VFX { SpitHit, SpitSpawn, SpecialHit, SpecialSpawn }

    public void SpawnVFX(VFX vfx, Vector3 position) {
        switch (vfx) {
            case VFX.SpitHit:
                SpawnVFX(m_SpitHit, position);
                break;
            case VFX.SpitSpawn:
                SpawnVFX(m_SpitSpawn, position);
                break;
            case VFX.SpecialHit:
                SpawnVFX(m_SpecialHit, position);
                break;
            case VFX.SpecialSpawn:
                SpawnVFX(m_SpecialSpawn, position);
                break;
            default:
                return;
        }
    }

    private void SpawnVFX(ParticleSystem vfx, Vector3 position) {
        if (vfx == null)
            return;

        if (vfx.gameObject.activeSelf) {
            vfx.Stop();
        }

        vfx.transform.position = position;
        vfx.gameObject.SetActive(true);
        vfx.Play();
    }
}
