using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHunter : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private GameEvent m_startModularCameraEvent;
    [SerializeField]
    private GameObjectEvent m_modularCameraAddEvent;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(m_playerData.ForcedCharacter.GhostHuntCharacter, transform);
        m_modularCameraAddEvent.Invoke(gameObject);
        m_startModularCameraEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
