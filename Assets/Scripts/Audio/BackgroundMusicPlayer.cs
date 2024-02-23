using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_ambience;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_music;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_menuMusic;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_winSound;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_failSound;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_gameStartCountdownStartSound;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_gameOverCountdownStartedSound;
    // private EventInstance m_Ambience;
    // private EventInstance m_Music;
    // private EventInstance m_MenuMusic;

    // private EventReference m_WinSound;
    // private EventReference m_FailSound;
    // private EventReference m_GameStartCountdownStartSound;
    // private EventReference m_GameOverCountdownStartedSound;

    private AudioSource menuSource;
    private AudioSource ambienceSource;
    private AudioSource musicSource;
    private AudioSource winSource;
    private AudioSource failSource;
    private AudioSource gameStartCountdownStartSource;
    private AudioSource gameOverCountdownStartSource;

    private void Start()
    {

        // m_WinSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.WinMatch);
        // m_FailSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.LoseMatch);
        // m_GameStartCountdownStartSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.StartMatchCountDown);
        // m_GameOverCountdownStartedSound = FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.EndLastSeconds);

        // m_Ambience = AudioManager.Instance.CreateInstance(
        //     FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.Ambience)
        //     );

        // m_Music = AudioManager.Instance.CreateInstance(
        //     FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.BackgroundMusic)
        //     );

        // m_MenuMusic = AudioManager.Instance.CreateInstance(
        //     FModEvents.Instance.GetEventReference(FModEvents.EventReferenceType.MainMenuTheme)
        //     );

        // m_Ambience.start();
        // m_MenuMusic.start();
        ambienceSource = AudioPlayer.PlayNonSpatialized(m_ambience);
        menuSource = AudioPlayer.PlayNonSpatialized(m_menuMusic);

        GameManager.Instance.gameStartCountdownStarted += OnGameStartCountdown;
        GameManager.Instance.gameEndCountdownStarted += OnGameOverCountdown;
        //GameManager.Instance.gameStarted += PlayBackgroundMusic;
        GameManager.Instance.gameOvered += OnGameOver;
    }

    private void PlayBackgroundMusic()
    {
        // m_Music.start();
        musicSource = AudioPlayer.PlayNonSpatialized(m_music);
    }

    private void OnGameStartCountdown()
    {
        // m_MenuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if (menuSource != null)
            menuSource.Stop();

        musicSource = AudioPlayer.PlayNonSpatialized(m_music);
        gameStartCountdownStartSource = AudioPlayer.PlayNonSpatialized(m_gameStartCountdownStartSound);
    }

    private void OnGameOverCountdown()
    {
        gameOverCountdownStartSource = AudioPlayer.PlayNonSpatialized(m_gameOverCountdownStartedSound);
    }

    private void OnGameOver(bool success)
    {
        if (success)
            winSource = AudioPlayer.PlayNonSpatialized(m_winSound);
        else
            failSource = AudioPlayer.PlayNonSpatialized(m_failSound);

        if (musicSource != null) musicSource.Stop();
        menuSource = AudioPlayer.PlayNonSpatialized(m_menuMusic);
    }
}
