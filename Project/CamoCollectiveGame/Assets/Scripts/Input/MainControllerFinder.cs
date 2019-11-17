using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllerFinder : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_mainController;

    public void SetMainController(int controller)
    {
        m_mainController.Value = controller;
    }
}
