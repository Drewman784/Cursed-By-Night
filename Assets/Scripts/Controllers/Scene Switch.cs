using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{ 
    void start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Start Button
    public void playButton()
    {
        SceneManager.LoadScene("Main_Level");
    }

    public void optionsButton()
    {
        SceneManager.LoadScene("options");
    }

    //quit applicatiomn
    public void closeApp()
    {
        Application.Quit();
    }


}
