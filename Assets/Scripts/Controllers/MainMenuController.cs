using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject OptionsWindow;
    [SerializeField] GameObject TeamPanel;
    [SerializeField] GameObject AssetsPanel;

    // Start is called before the first frame update
    void Start()
    {
        CloseOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenOptions(){
        OptionsWindow.SetActive(true);
        OpenTeamPanel();
    }

    public void CloseOptions(){
        OptionsWindow.SetActive(false);
    }
    
    public void OpenTeamPanel(){
        TeamPanel.SetActive(true);
        AssetsPanel.SetActive(false);
    }

    public void OpenAssetsPanel(){
        TeamPanel.SetActive(false);
        AssetsPanel.SetActive(true);
    }
}
