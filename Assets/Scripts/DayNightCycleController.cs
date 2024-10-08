using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DayNightCycleController : MonoBehaviour
{
    private int sec;
    [SerializeField] int cycleLength;
    private bool day;

    private float ct;

    private bool displayUp;
    

    [SerializeField] GameObject displayWindow;
    [SerializeField] TextMeshProUGUI displayText;


    public UnityEvent EnterDay;
    public UnityEvent EnterNight;

    // Start is called before the first frame update
    void Start()
    {
     sec = 0;
     ct = 0;   
     displayWindow.SetActive(false);
     displayUp =false;
    }

    // Update is called once per frame
    void Update()
    {
     ct+=Time.deltaTime;
     if(ct>=1){
        ct = 0;
        sec++;
        if(displayUp&&sec>=5){
            displayUp = false;
            displayWindow.SetActive(false);
        }
        if(sec>=cycleLength){
            if(day){
                EnterNight.Invoke();
                day = false;
            } else{
                EnterDay.Invoke();
                day = true;
            }
            ct = 0;
            sec = 0;
        }
     }   
    }

    public void DisplayDayAlert(){
        displayWindow.SetActive(true);
        displayText.text = "Entering Day Cycle";
        displayUp = true;
    }

    public void DisplayNightAlert(){
        displayWindow.SetActive(true);
        displayText.text = "Entering Night Cycle";
        displayUp = true;
    }
}
