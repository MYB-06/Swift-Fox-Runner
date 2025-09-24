using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Layered")]
public class LayeredTransformEvent : ScriptableObject
{
    public UnityEvent<Transform, LayerType> OnEventRaised;

    public void RaiseEvent(Transform transform, LayerType layer)
    {
        OnEventRaised?.Invoke(transform, layer);
    }
}
