using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    private EventInstance m_Ambience;
    private EventInstance m_Music;
    private EventInstance m_MenuMusic;

    private EventReference m_WinSound;
    private EventReference m_FailSound;
    private EventReference m_GameStartCountdownStartSound;
    private EventReference m_GameOverCountdownStartedSound;

    private void Start()
    {

        m_WinSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.WinMatch);
        m_FailSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.LoseMatch);
        m_GameStartCountdownStartSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.StartMatchCountDown);
        m_GameOverCountdownStartedSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.EndLastSeconds);

        m_Ambience = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.Ambience)
            );

        m_Music = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.BackgroundMusic)
            );

        m_MenuMusic = AudioManager.Instance.CreateInstance(
            FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.MainMenuTheme)
            );

        m_Ambience.start();
        m_MenuMusic.start();

        GameManager.Instance.gameStartCountdownStarted += OnGameStartCountdown;
        GameManager.Instance.gameEndCountdownStarted += OnGameOverCountdown;
        //GameManager.Instance.gameStarted += PlayBackgroundMusic;
        GameManager.Instance.gameOvered += OnGameOver;
    }

    private void PlayBackgroundMusic()
    {
        m_Music.start();
    }

    private void OnGameStartCountdown()
    {
        m_MenuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        m_Music.start();
        AudioManager.Instance.PlayOneShot(m_GameStartCountdownStartSound, Camera.main.transform.position);
    }

    private void OnGameOverCountdown()
    {
        AudioManager.Instance.PlayOneShot(m_GameOverCountdownStartedSound, Camera.main.transform.position);
    }

    private void OnGameOver(bool success)
    {
        if (success)
            AudioManager.Instance.PlayOneShot(m_WinSound, Camera.main.transform.position);
        else
            AudioManager.Instance.PlayOneShot(m_FailSound, Camera.main.transform.position);

        m_Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_MenuMusic.start();
    }
}
