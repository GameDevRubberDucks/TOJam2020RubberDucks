using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Day_Manager : MonoBehaviour
{
    //--- TextMeshs ---//
    public TextMeshProUGUI clock;
    //public TextMeshProUGUI dailyEarnings;
    //public TextMeshProUGUI clock;

    public float dayLengthIRL = 10.0f;
    private float dayLengthIG = 0.0f;

    public float dayStartTime = 9.0f;
    public float dayEndTime = 17.0f;

    private float timeElapsed = 0.0f;
    private float timeElapsedLERP = 0.0f;
    private float timeElapsedIGLERP = 0.0f;

    private int hours = 0;
    private int minutes = 0;

    private string timeString = " ";

    private int dayCounter = 0;
    public int maxDayCounter = 7;

    public bool playing = true;
    public bool shouldCountTime = true;

    public GameObject cashCalculator;

    // UI representations
    public Image dayProgressBar;
    public Color dayProgressMed;
    public Color dayProgressHigh;
    public TextMeshProUGUI txtDayCounter;
    public GameObject txtDayStart;
    public GameObject txtDayOver;
    public GameObject txtBackground;
    public float dayBufferLength = 3.0f;

    public static Day_Manager _instance;
    private Persistence_Manager persistence;


    //Audio Variables
    private Audio_Manager audioManager;

    // Start is called before the first frame update
    private void Awake()
    {
        persistence = GameObject.FindObjectOfType<Persistence_Manager>();
        dayCounter = persistence.m_dayNumber;
        audioManager = GameObject.Find("AudioManager").GetComponent<Audio_Manager>();
    }
    
    void Start()
    {
        dayLengthIG = dayEndTime - dayStartTime;
        timeElapsed = 0.0f;
        txtDayCounter.text = "Day " + (dayCounter + 1).ToString();

        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<Day_Manager>();

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        clock = null ?? GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        //dailyEarnings = null ?? GameObject.Find("Daily Earning").GetComponent<TextMeshProUGUI>();
        cashCalculator = null ?? GameObject.Find("Game_Controller");

        // Shouldn't start counting time until the day begins properly
        shouldCountTime = false;

        // Should prevent key input until the day starts
        GameObject.FindObjectOfType<Key_Manager>().enabled = false;

        // Show the day start text
        txtDayStart.SetActive(true);
        txtBackground.SetActive(true);

        // Turn off the day start text after a few seconds and then begin the day properly
        Invoke("StartDay", dayBufferLength);
    }

    // Update is called once per frame
    void Update()
    {
        //If we want to keep this function this class
        if (shouldCountTime)
        {
            timeElapsed = timeElapsed + Time.deltaTime;

            //Day end protocol
            if (timeElapsed >= dayLengthIRL)
            {
                //Call CalculateEndOfDayMoney
                //dailyEarnings.GetComponent<TextMeshProUGUI>().text = "$ " + this.GetComponent<CashCalculation_Script>().CalculateCashForDay(dayCounter + 1).ToString(); ;

                dayCounter++;

                if (dayCounter >= maxDayCounter)
                {
                    //End week and game
                    // save the data out to the persistence manager
                    persistence.m_dayNumber = dayCounter;
                    persistence.m_totalMoney = Mathf.RoundToInt(GetComponent<CashCalculation_Script>().TotalCashEarned());

                    // Show the day over text
                    txtDayOver.SetActive(true);
                    txtBackground.SetActive(true);

                    // Remove all the callers from the rooms
                    GameObject.FindObjectOfType<Room_Manager>().DisconnectAllCallers();

                    // Stop updating all of the calls
                    GameObject.FindObjectOfType<Call_Manager>().IsActive = false;

                    // Disable input
                    GameObject.FindObjectOfType<Key_Manager>().enabled = false;

                    //End day
                    Invoke("MoveToEndWeekScreen", dayBufferLength);
                }
                else
                {
                    shouldCountTime = false;

                    //Play dayEnd Audio
                    audioManager.PlayOneShot(5,0.5f);

                    // save the data out to the persistence manager
                    persistence.m_dayNumber = dayCounter;
                    persistence.m_totalMoney = Mathf.RoundToInt(GetComponent<CashCalculation_Script>().TotalCashEarned());

                    // Show the day over text
                    txtDayOver.SetActive(true);
                    txtBackground.SetActive(true);

                    // Remove all the callers from the rooms
                    GameObject.FindObjectOfType<Room_Manager>().DisconnectAllCallers();

                    // Stop updating all of the calls
                    GameObject.FindObjectOfType<Call_Manager>().IsActive = false;

                    // Disable input
                    GameObject.FindObjectOfType<Key_Manager>().enabled = false;

                    //End day
                    Invoke("MoveToEndDayScreen", dayBufferLength);                    
                }
            }
        }
        
        if (clock)
        {

            timeElapsedLERP = timeElapsed / dayLengthIRL; // This is the percent of the day that has gone by
            dayProgressBar.fillAmount = timeElapsedLERP;

            if (timeElapsedLERP > 0.9f)
                dayProgressBar.color = dayProgressHigh;
            else if (timeElapsedLERP > 0.7f)
                dayProgressBar.color = dayProgressMed;
            
            dayLengthIG = (dayEndTime * 60.0f) - (dayStartTime * 60.0f);

            timeElapsedIGLERP = Mathf.Lerp((dayStartTime * 60.0f), (dayEndTime * 60.0f), timeElapsedLERP);

            hours = (int)timeElapsedIGLERP / 60;
            minutes = (int)timeElapsedIGLERP % 60;

            timeString = hours.ToString("00") + ":";

            timeString += minutes.ToString("00");

            clock.GetComponent<TextMeshProUGUI>().text = timeString;
        }
    }

    public void StartNextDay()
    {
        //Incerment Day counter
        dayCounter++;

        //Reset
        timeElapsed = 0.0f;
        shouldCountTime = true;

        // Load the main game
        SceneManager.LoadScene("Main");
    }

    public void StartDay()
    {
        // Hide the day start text
        txtDayStart.SetActive(false);
        txtBackground.SetActive(false);

        // Tell the call manager to start working
        GameObject.FindObjectOfType<Call_Manager>().IsActive = true;

        // Allow keyboard input
        GameObject.FindObjectOfType<Key_Manager>().enabled = true;

        // Should start counting time now
        shouldCountTime = true;
    }

    public void MoveToEndDayScreen()
    {
        SceneManager.LoadScene("EndOfDay");
    }

    public void MoveToEndWeekScreen()
    {
        SceneManager.LoadScene("EndOfWeek");
    }

    public int CurrentDay
    {
        get => dayCounter;
    }
}
