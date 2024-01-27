using FMOD.Studio;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    private EventInstance m_Ambience;
    private EventInstance m_Music;

    private void Start() {

        m_Ambience = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.Ambience)
            );

        m_Music = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.BackgroundMusic)
            );

        m_Ambience.start();
        m_Music.start();
    }
}
