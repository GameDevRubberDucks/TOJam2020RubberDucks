using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Day_Manager : MonoBehaviour
{
    public TextMeshProUGUI clock;
    public float dayLengthIRL = 10.0f;
    private float dayLengthIG = 0.0f;

    public float dayStartTime = 9.0f;
    public float dayEndTime = 17.0f;

    private float timeElapsed = 0.0f;
    private float timeElapsedLERP = 0.0f;
    private float timeElapsedIGLERP = 0.0f;

    private float conversionNumber = 0.0f;

    private int hours = 0;
    private int minutes = 0;

    private string timeString = " ";

    public bool playing = true;

    // Start is called before the first frame update
    void Start()
    {
       
        dayLengthIG = dayEndTime - dayStartTime;

    }

    // Update is called once per frame
    void Update()
    {
        //If we want to keep this function this class
        if (playing)
        {
            timeElapsed = timeElapsed + Time.deltaTime;
            //Day end protocol
            if (timeElapsed >= dayLengthIRL)
            {
                //End day 
                //Call CalculateEndOfDayMoney
                timeElapsed = 0.0f;
            }
        }
        //Fine Time elapsed / time remaining
        timeElapsedLERP = timeElapsed / dayLengthIRL; // This is the percent of the day that has gone by

        dayLengthIG = (dayEndTime * 60.0f) - (dayStartTime * 60.0f);

        Debug.Log("LErp: " + timeElapsedLERP);

        //timeElapsedIGLERP = dayLengthIG * timeElapsedLERP;
        timeElapsedIGLERP = Mathf.Lerp((dayStartTime * 60.0f), (dayEndTime * 60.0f), timeElapsedLERP);

        Debug.Log("DayLErp: " +  timeElapsedIGLERP);

        hours = (int)timeElapsedIGLERP / 60;
        minutes = (int)timeElapsedIGLERP % 60;
        
        timeString = hours.ToString("00") + ":";

            timeString += minutes.ToString("00");

        clock.GetComponent<TextMeshProUGUI>().text = timeString;
        
        //24 hours
        //clock.GetComponent<TextMeshProUGUI>().text = (24 - (timeRemaining / 12.5f)).ToString();
    }







    public bool CheckDayEnd()
    {

        if (0.0f <= 0.0f)
            {

                return true;
            }

        return false;
    }
}
