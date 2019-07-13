using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent<T, V, U> : ScriptableObject where T : CustomUnityEvent<U> where V : GameEvent<T, V, U>
{
    private List<GameEventListener<T, V, U>> listeners = new List<GameEventListener<T, V, U>>();

    public void Invoke(U obj)
    {
        foreach (GameEventListener<T, V, U> e in listeners)
        {
            e.Invoke(obj);
        }
    }

    public void AttachListener(GameEventListener<T, V, U> e)
    {
        listeners.Add(e);
    }

    public void DetachListener(GameEventListener<T, V, U> e)
    {
        listeners.Remove(e);
    }
}

[CreateAssetMenu(menuName = "ModularSystem/Event/Null")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Invoke()
    {
        foreach (GameEventListener e in listeners)
        {
            e.Invoke();
        }
    }

    public void AttachListener(GameEventListener e)
    {
        listeners.Add(e);
    }

    public void DetachListener(GameEventListener e)
    {
        listeners.Remove(e);
    }
}

[System.Serializable]
public abstract class CustomUnityEvent<T> : UnityEvent<T>
{
}

[System.Serializable]
public class CustomFloatEvent : CustomUnityEvent<float>
{
}

[System.Serializable]
public class CustomGameObjectEvent : CustomUnityEvent<GameObject>
{
}

[System.Serializable]
public class CustomStringEvent : CustomUnityEvent<string>
{
}
