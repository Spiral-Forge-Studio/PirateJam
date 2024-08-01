using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    public GameObject SettingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneController.instance.LoadNextScene();
    }

    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
    }
}
