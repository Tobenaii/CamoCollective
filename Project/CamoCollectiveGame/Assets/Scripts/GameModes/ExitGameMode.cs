using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameMode : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadSceneAsync("ModeSelectionScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }
}
