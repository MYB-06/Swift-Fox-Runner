using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Transform Event")]
public class TransformEvent : ScriptableObject
{
    public UnityEvent<Transform> OnEventRaised;

    public void RaiseEvent(Transform transform)
    {
        OnEventRaised?.Invoke(transform);
    }
}
