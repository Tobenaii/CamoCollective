using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolInstance : MonoBehaviour
{
    private GameObjectPool m_gameObjectPool;
    public GameObjectPool Pool => m_gameObjectPool;
    public void SetPool(GameObjectPool pool)
    {
        m_gameObjectPool = pool;
    }
}
