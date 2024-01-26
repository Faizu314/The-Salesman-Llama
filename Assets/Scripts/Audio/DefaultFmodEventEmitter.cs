using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class DefaultFmodEventEmitter : MonoBehaviour {

    [SerializeField] private FModEvents.EventReferenceType m_Event;

    protected StudioEventEmitter m_EventReference;

    protected virtual void Awake() {
        var eventReference = FModEvents.Instance.GetEventReference(m_Event);
        m_EventReference = AudioManager.Instance.InitializeEventEmitter(eventReference, gameObject);
    }
}
