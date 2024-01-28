using FMOD.Studio;
using FMODUnity;
using Phezu.Util;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AudioManager : Singleton<AudioManager> {

    private List<EventInstance> m_EventInstances = new();
    private List<StudioEventEmitter> m_EventEmitters = new();

    public void PlayOneShot(EventReference sound, Vector3 worldPos) {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShotWithParameters(EventReference sound, Vector3 position, float minDist, float maxDist) {
        EventInstance instance = RuntimeManager.CreateInstance(sound);

        instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, minDist);
        instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, maxDist);

        instance.set3DAttributes(position.To3DAttributes());
        instance.start();
        instance.release();
    }

    public EventInstance CreateInstance(EventReference eventReference) {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        m_EventInstances.Add(instance);
        return instance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject obj) {
        var emitter = obj.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        m_EventEmitters.Add(emitter);
        return emitter;
    }

    private void Cleanup() {
        foreach (var instance in m_EventInstances) {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        foreach (var emitter in m_EventEmitters) {
            emitter.Stop();
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Cleanup();
    }

}
