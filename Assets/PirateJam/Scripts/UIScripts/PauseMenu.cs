using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public void RestartLevel()
    {
        SceneController.instance.ReloadCurrentScene();
    }

    public void ReturnToMainMenu()
    {
        SceneController.instance.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
