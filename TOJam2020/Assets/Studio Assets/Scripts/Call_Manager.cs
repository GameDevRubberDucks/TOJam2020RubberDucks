using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Call_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public int m_MAX_NUM_CALLS;
    public float m_timeBetweenCalls;
    public float[] m_possibleWaitTimes;
    public float[] m_possibleCallLengths;



    //--- Private Variables ---//
    private Room_Manager m_roomManager;
    private CallerLog_UIManager m_callLogUI;
    private List<Call_Group> m_callList;
    private float m_timeSinceLastCall;

    private List<float> cashFromCall;
    private int callsCompletedTotal; // In case we want career how many calls they have completed
    private int callsCompletedDaily; //How many calls completed in the day
    public TextMeshProUGUI totalCash;
    //public TextMeshProUGUI callCash;


    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables 
        m_roomManager = GameObject.FindObjectOfType<Room_Manager>();
        m_callLogUI = GameObject.FindObjectOfType<CallerLog_UIManager>();
        m_callList = new List<Call_Group>();
        m_timeSinceLastCall = m_timeBetweenCalls;
        totalCash = GameObject.Find("Txt_Money").GetComponent<TextMeshProUGUI>();
        //callCash = GameObject.Find("Call Cash").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // Only try to generate new calls if there is still space to do so
        if (m_callList.Count < m_MAX_NUM_CALLS)
        {
            // Count up since the last call
            m_timeSinceLastCall += Time.deltaTime;

            // If enough time has passed, generate a new call
            if (m_timeSinceLastCall >= m_timeBetweenCalls)
                GenerateCall();
        }

        // Update all of the calls
        foreach (var call in m_callList)
            call.UpdateCall();
    }



    //--- Methods ---//
    public void GenerateCall()
    {
        // Create a new call group by randomly generating the necessary properties
        // The call capacity can be anywhere between 2 people and the highest room count (inclusive). The +1 is because the Random.Range() function is exclusive at the upper bound
        // The wait time is randomly selected from a list set in the inspector
        // The call duration is randomly selected from a list set in the inspector
        int newCallCapacity = Random.Range(2, m_roomManager.HighestRoomCapacity + 1);
        float newCallWaitTime = m_possibleWaitTimes[Random.Range(0, m_possibleWaitTimes.Length)];
        float newCallLength = m_possibleCallLengths[Random.Range(0, m_possibleCallLengths.Length)];
        Call_Group newCall = new Call_Group(newCallCapacity, newCallWaitTime, newCallLength);

        // Place all of the new call individuals into the unassigned room
        m_roomManager.AddUnassignedCallers(newCall.CallParticipants);

        // Hook into the call completion event so we can handle it finishing
        newCall.m_OnCallCompleted.AddListener(this.OnCallCompleted);

        // Add the call group to the list
        m_callList.Add(newCall);

        // Generate a new UI element for the call
        m_callLogUI.AddCallGroupUI(newCall);

        // Reset the timer
        m_timeSinceLastCall = 0.0f;
    }



    //--- Event Hooks ---//
    public void OnCallCompleted(Call_Group _callObj, Call_State _callFinalState)
    {
        // Handle the call termination differently, depending on if ended well or not
        if (_callFinalState == Call_State.Waited_Too_Long)
        {
            // TODO: Play negative feedback
        }
        else if (_callFinalState == Call_State.Completed)
        {
            // TODO: Play positive feedback

            // Add points, reputation, etc
            callsCompletedDaily++;
            GameObject.FindObjectOfType<CashCalculation_Script>().CalculateCashForCall(_callObj);


            //callCash.GetComponent<TextMeshProUGUI>().text = GameObject.FindObjectOfType<CashCalculation_Script>().CalculateCashForCall(_callObj).ToString();
            totalCash.GetComponent<TextMeshProUGUI>().text = GameObject.FindObjectOfType<CashCalculation_Script>().TotalCashEarned().ToString(); //Returns Total Cash
            //Debug the new money recieved.
            //Debug.Log(this.GetComponent<CashCalculation_Script>().CalculateCashForCall(_callObj));
        }

        // Remove the UI element from the call backlog
        m_callLogUI.RemoveCallGroupUI(_callObj);

        // Unhook the event
        _callObj.m_OnCallCompleted.RemoveListener(this.OnCallCompleted);

        // Remove the callers from their associated rooms
        m_roomManager.DisconnectCallers(_callObj.CallParticipants);

        // We should also remove the call from the list
        // However, we need to wait until the end of the frame to ensure all the other call operations are complete
        StartCoroutine(DeleteCall(_callObj));
    }

    public IEnumerator DeleteCall(Call_Group _callObj)
    {
        // Wait until the frame is over to ensure all other call operations are completed
        yield return new WaitForEndOfFrame();

        // Remove the caller from the list
        m_callList.Remove(_callObj);
    }
}
