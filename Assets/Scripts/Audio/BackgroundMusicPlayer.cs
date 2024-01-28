using FMOD.Studio;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    private EventInstance m_Ambience;
    private EventInstance m_Music;
    private EventInstance m_WinMusic;
    private EventInstance m_FailMusic;

    private void Start()
    {

        m_Ambience = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.Ambience)
            );

        m_Music = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.BackgroundMusic)
            );

        m_WinMusic = AudioManager.Instance.CreateInstance(
        FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.WinMatch)
        );

        m_FailMusic = AudioManager.Instance.CreateInstance(
        FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.LoseMatch)
        );

        m_Ambience.start();
        m_Music.start();

        GameManager.Instance.gameOvered += OnGameOver;
    }

    private void OnGameOver(bool success)
    {
        if (success)
            m_WinMusic.start();
        else
            m_FailMusic.start();

        m_Music.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
