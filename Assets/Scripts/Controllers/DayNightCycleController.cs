using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class DayNightCycleController : MonoBehaviour
{
    private int sec; //internal count of seconds from start of cycle
    [SerializeField] int cycleLength; //set the length of the day/night cycles
    [SerializeField] int cycleWarningTime; //dawn/dusk time before cycle change

    private bool dawnDusk;
    private bool day; 

    private float ct;

    private bool displayUp; //is the day/night cycle alert displayed?
    

    [SerializeField] GameObject displayWindow;
    [SerializeField] TextMeshProUGUI displayText;

    [SerializeField] Material daySkybox; //skybox for daytime
    [SerializeField] Material nightSkybox; //skybox for nighttime

    [SerializeField] Material dawnDuskSkybox; //skybox for dawn/dusk periods
    [SerializeField] Light lightSource; //the directional light for the scene

    public UnityEvent EnterDay; //this triggers when the day cycle begins and calls all the necessary methods
    public UnityEvent EnterNight;//this triggers when the night cycle begins and calls all the necessary methods

    // Start is called before the first frame update
    void Start()
    {
     sec = 0;
     ct = 0;   
     displayWindow.SetActive(false);
     displayUp =false;
     day = true;
     EnterDay.Invoke();
     dawnDusk = false;
    }

    // Update is called once per frame
    void Update()
    {
     ct+=Time.deltaTime;
     if(ct>=1){ //increment seconds passed since start of cycle
        ct = 0;
        sec++;
        if(displayUp&&sec>=5){ //hides alert display after 5 seconds
            displayUp = false;
            displayWindow.SetActive(false);
        }
        if(sec>=cycleLength){ //if its been a full cycle, invoke next cycle
            if(day){
                EnterNight.Invoke();
                day = false;
            } else{
                EnterDay.Invoke();
                day = true;
            }
            ct = 0;
            sec = 0;
        } else if(sec>=(cycleLength-cycleWarningTime) && !dawnDusk){
            ChangeMidSkybox();
        }
     }   
    }

    public void DisplayDayAlert(){ //displays text alert of entering day cycle
        displayWindow.SetActive(true);
        displayText.text = "Entering Day Cycle";
        displayText.color = Color.black;
        displayUp = true;
    }

    public void DisplayNightAlert(){//displays text alert of entering night cycle
        displayWindow.SetActive(true);
        displayText.text = "Entering Night Cycle";
        displayText.color = Color.white;
        displayUp = true;
    }

    public void ChangeDaySkybox(){ //changes the skybox and light intensity for day cycle
        UnityEngine.RenderSettings.skybox = daySkybox;
        lightSource.intensity = 1;
        dawnDusk = false;
    }

    public void ChangeNightSkybox(){ //changes the skybox and light intensity for night cycle
        UnityEngine.RenderSettings.skybox = nightSkybox;
        lightSource.intensity = .25f;
        dawnDusk = false;
    }

    public void ChangeMidSkybox(){ // changes the skybox and light intensity for the dawn/dusk periods 
        UnityEngine.RenderSettings.skybox = dawnDuskSkybox;
        lightSource.intensity = .65f;
        dawnDusk = true;
    }
}
