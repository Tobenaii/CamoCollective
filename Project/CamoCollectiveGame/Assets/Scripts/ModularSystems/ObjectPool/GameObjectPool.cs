using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularSystem/ObjectPool/GameObject")]
public class GameObjectPool : ScriptableObject
{
    public float initialAmmount;
    public GameObject objectPrefab;

    public Stack<GameObject> pool;
    private Transform parent;

    private bool initialized;

    private void OnEnable()
    {
        initialized = false;
    }

    private void Init()
    {
        //Instantiate object pool parent if there is none
        if (GameObject.Find("ObjectPool") == null)
            Instantiate(new GameObject("ObjectPool"));
        parent = GameObject.Find("ObjectPool").transform;

        //Reset pool and instantiate objects up to initial ammount
        pool = new Stack<GameObject>();

        for (int i = 0; i < initialAmmount; i++)
        {
            AddObject();
        }
        initialized = true;
    }

    public void Reset()
    {
        Init();
    }

    private void AddObject()
    {
        GameObject obj = Instantiate(objectPrefab, parent);
        obj.SetActive(false);
        pool.Push(obj);
    }

    public GameObject GetObject()
    {
        if (!initialized)
            Init();
        //Add an object if pool is empty
        if (pool.Count == 0)
            AddObject();
        if (pool.Peek() == null)
        {
            Reset();
        }
        //Kick the gameObject out of the house and chuck into the big cruel world after preparing it :(
        pool.Peek().transform.SetParent(parent);
        pool.Peek().gameObject.SetActive(true);
        return pool.Pop();
    }

    public void DestroyObject(GameObject obj)
    {
        if (pool == null)
            Init();
        //He dead
        obj.transform.SetParent(parent);
        obj.SetActive(false);
        pool.Push(obj);
    }
}
