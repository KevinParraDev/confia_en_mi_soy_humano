using UnityEngine;

public abstract class UIViewBase : MonoBehaviour
{
    protected UI id = UI.None;
    public UI ID { get { return id; } }

    protected abstract void Awake();
    public abstract void Initialize(params object[] parameters);
    protected virtual void AddListeners() { }
    protected virtual void RemoveListeners() { }
    public abstract void Conclude();
}