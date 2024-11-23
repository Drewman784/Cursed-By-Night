using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject controlScreen;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            EnterPauseMenu();
        }
    }

    public void EnterPauseMenu(){
        Time.timeScale = 0;
        player.SetMenu(true);
        pauseMenu.SetActive(true);
        ExitControlScreen();
        
    }

    public void ExitPauseMenu(){
        Time.timeScale = 1;
        player.SetMenu(false);
        pauseMenu.SetActive(false);
    }

    public void EnterControlScreen(){
        controlScreen.SetActive(true);
    }

    public void ExitControlScreen(){
        controlScreen.SetActive(false);
    }

    public void QuitToMenu(){
        SceneManager.LoadScene("Main_Menu");
    }
}
