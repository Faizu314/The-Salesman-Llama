using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class AudioPlayer : MonoBehaviour
{

    [System.Serializable]
    public class PlayAtPointInput
    {
        [SerializeField] public AudioClip clip;
        [SerializeField] public AudioMixerGroup mixerGroup;
        [SerializeField][Range(0, 1)] public float volume = 1f;
        [SerializeField][Range(-3, 3)] public float pitch = 1f;
        [SerializeField] public float maxVolumeDistance = 10f;
        [SerializeField] public float minVolumeDistance = 50f;
        [SerializeField][Range(0, 1)] public float pitchVariance = 0f;
        [SerializeField] public bool isLooping = false;
        [SerializeField] public bool fadeOnCancel = false;
        [SerializeField] public float fadeDuration = 0f;
        [SerializeField] public AnimationCurve fadeOutCurve = default;
    }

    [System.Serializable]
    public class PlayNonSpatializedInput
    {
        [SerializeField] public AudioClip clip;
        [SerializeField] public AudioMixerGroup mixerGroup;
        [SerializeField][Range(0, 1)] public float volume = 1f;
        [SerializeField][Range(-3, 3)] public float pitch = 1f;
        [SerializeField][Range(0, 1)] public float pitchVariance = 0f;
        [SerializeField] public bool isLooping = false;
    }

    // Structs
    //////////////////////

    public class AudioWorker
    {
        public AudioSource audioSource;
        public Transform target;
        public bool isBusy;
    }

    // Inspector Vars
    //////////////////////

    [SerializeField] static int startingPoolSize = 10;

    // Internal Vars
    //////////////////////

    static List<AudioWorker> audioWorkers = new List<AudioWorker>();

    // Static Methods
    //////////////////////

    public static AudioSource PlayNonSpatialized(PlayNonSpatializedInput input, CancellationTokenSource cts = default)
    {
        AudioWorker worker = GetFreeWorker();
        worker.isBusy = true;
        worker.audioSource.spatialBlend = 0f;
        worker.audioSource.loop = input.isLooping;
        worker.audioSource.clip = input.clip;
        worker.audioSource.outputAudioMixerGroup = input.mixerGroup;
        worker.audioSource.volume = input.volume;
        worker.audioSource.pitch = input.pitch + (Random.Range(0, input.pitchVariance) * (Random.value < .5 ? 1 : -1));
        worker.audioSource.spatialize = false;
        RunPlayNonSpatialized(worker, cts).Forget();
        return worker.audioSource;
    }

    public static AudioSource PlayAtPoint(Vector3 worldPosition, PlayAtPointInput input, CancellationTokenSource cts = default, float duration = Mathf.Infinity)
    {
        AudioWorker worker = GetFreeWorker();
        worker.audioSource.transform.position = worldPosition;
        worker.isBusy = true;
        worker.audioSource.spatialBlend = 1f;
        worker.audioSource.rolloffMode = AudioRolloffMode.Linear;
        worker.audioSource.loop = input.isLooping;
        worker.audioSource.clip = input.clip;
        worker.audioSource.outputAudioMixerGroup = input.mixerGroup;
        worker.audioSource.volume = input.volume;
        worker.audioSource.pitch = input.pitch + (Random.Range(0, input.pitchVariance) * (Random.value < .5 ? 1 : -1));
        worker.audioSource.minDistance = input.maxVolumeDistance;
        worker.audioSource.maxDistance = input.minVolumeDistance;
        worker.audioSource.spatialize = true;
        RunPlayAtPoint(worker, input.fadeDuration, input.fadeOutCurve, input.fadeOnCancel, cts, duration).Forget();
        return worker.audioSource;
    }

    public static AudioSource PlayAtPoint(Transform transform, PlayAtPointInput input, CancellationTokenSource cts = default, float duration = Mathf.Infinity)
    {
        AudioWorker worker = GetFreeWorker();
        worker.target = transform;
        worker.audioSource.transform.position = transform.position;
        worker.isBusy = true;
        worker.audioSource.spatialBlend = 1f;
        worker.audioSource.rolloffMode = AudioRolloffMode.Linear;
        worker.audioSource.loop = input.isLooping;
        worker.audioSource.clip = input.clip;
        worker.audioSource.outputAudioMixerGroup = input.mixerGroup;
        worker.audioSource.volume = input.volume;
        worker.audioSource.pitch = input.pitch + (Random.Range(0, input.pitchVariance) * (Random.value < .5 ? 1 : -1));
        worker.audioSource.minDistance = input.maxVolumeDistance;
        worker.audioSource.maxDistance = input.minVolumeDistance;
        worker.audioSource.spatialize = true;
        RunPlayAtPoint(worker, input.fadeDuration, input.fadeOutCurve, input.fadeOnCancel, cts, duration).Forget();
        return worker.audioSource;
    }

    static async UniTaskVoid RunPlayAtPoint(AudioWorker worker, float fadeOutDuration, AnimationCurve fadeOutCurve, bool fadeOnCancel, CancellationTokenSource cts, float duration = Mathf.Infinity)
    {
        var cancelled = false;
        var elapsed = 0f;
        var originalVolume = worker.audioSource.volume;
        worker.audioSource.Play();
        do
        {
            if (worker.target != null) worker.audioSource.transform.position = worker.target.position;
            if (duration - elapsed < fadeOutDuration)
            {
                worker.audioSource.volume = originalVolume * fadeOutCurve.Evaluate((duration - elapsed) / fadeOutDuration);
            }
            if (cts == null)
                await UniTask.NextFrame();
            else
                cancelled = await UniTask.NextFrame(cts.Token).SuppressCancellationThrow();
            elapsed += Time.deltaTime;
        } while (worker?.audioSource != null && (worker?.audioSource.isPlaying ?? false) && !cancelled && elapsed < duration);
        if (fadeOnCancel && worker?.audioSource != null)
        {
            elapsed = 0;
            do
            {
                worker.audioSource.volume = originalVolume * fadeOutCurve.Evaluate((fadeOutDuration - elapsed) / fadeOutDuration);
                await UniTask.NextFrame();
                elapsed += Time.deltaTime;
            } while (elapsed < fadeOutDuration);
        }
        if (worker?.audioSource != null)
        {
            worker.audioSource.Stop();
            worker.audioSource.clip = null;
        }
        worker.isBusy = false;
    }

    static async UniTaskVoid RunPlayNonSpatialized(AudioWorker worker, CancellationTokenSource cts)
    {
        var cancelled = false;
        worker.audioSource.Play();
        do
        {
            if (cts == null)
                await UniTask.NextFrame();
            else
                cancelled = await UniTask.NextFrame(cts.Token).SuppressCancellationThrow();
        } while (worker.audioSource != null && worker.audioSource.isPlaying && !cancelled);
        worker.audioSource.Stop();
        worker.audioSource.clip = null;
        worker.isBusy = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        audioWorkers.Clear();
        for (int i = 0; i < startingPoolSize; i++)
        {
            GameObject obj = new GameObject("Audio Worker");
            DontDestroyOnLoad(obj);
            var audioSource = obj.AddComponent<AudioSource>();
            obj.layer = default;
            audioWorkers.Add(new AudioWorker
            {
                audioSource = audioSource,
                isBusy = false
            });
        }
    }

    static AudioWorker GetFreeWorker()
    {
        var worker = audioWorkers.FirstOrDefault(w => !w.isBusy);
        if (worker == null)
        {
            IncreasePoolSize();
            return GetFreeWorker();
        }
        return worker;
    }

    static void IncreasePoolSize()
    {
        var currentCount = audioWorkers.Count;
        for (int i = 0; i < Mathf.CeilToInt((currentCount + 1.0f) / 2.0f); i++)
        {
            GameObject obj = new GameObject("Audio Worker");
            DontDestroyOnLoad(obj);
            var audioSource = obj.AddComponent<AudioSource>();
            obj.layer = default;
            audioWorkers.Add(new AudioWorker
            {
                audioSource = audioSource,
                isBusy = false
            });
        }
    }

}
