using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListSet<T> : ScriptableObject
{
    public List<T> List = new List<T>();

    public int Count => List.Count;

    public T this[int index]
    {
        get { return List[index]; }
        set { List[index] = value; }
    }

    private void OnDisable()
    {
        List.Clear();
    }

    private void OnEnable()
    {
        List.Clear();
    }

    public bool Containts (T obj)
    {
        return List.Contains(obj);
    }

    public void Add(T obj)
    {
        List.Add(obj);
    }

    public void Remove(T obj)
    {
        List.Remove(obj);
    }

    public void RemoveAt(int index)
    {
        List.RemoveAt(index);
    }

    public void Insert(int index, T obj)
    {
        List.Insert(index, obj);
    }

    public void Clear()
    {
        List.Clear();
    }
}
