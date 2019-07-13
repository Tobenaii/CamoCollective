using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventListener<T, V, U> : MonoBehaviour where T : CustomUnityEvent<U> where V : GameEvent<T, V, U>
{
    public V gameEvent;
    public T response;

    public void Invoke(U obj)
    {
        response.Invoke(obj);
    }

    private void OnEnable()
    {
        if (gameEvent == null)
            Debug.Log(gameObject.name);
        gameEvent.AttachListener(this);
    }

    private void OnDisable()
    {
        gameEvent.DetachListener(this);
    }
}

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent response;

    public void Invoke()
    {
        if (response.GetPersistentEventCount() == 0)
            return;
        response.Invoke();
    }

    private void OnEnable()
    {
        if (gameEvent == null)
            Debug.Log(gameObject.name);
        if (gameEvent == null)
            Debug.Log("Null Event at " + gameObject.name);
        gameEvent.AttachListener(this);
    }

    private void OnDisable()
    {
        gameEvent.DetachListener(this);
    }
}

