using UnityEngine;
using System.Collections.Generic;

public class Call_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public int m_MAX_NUM_CALLS;
    public float m_timeBetweenCalls;
    public float[] m_possibleWaitTimes;
    public float[] m_possibleCallLengths;



    //--- Private Variables ---//
    private Room_Manager m_roomManager;
    private CallerLog_Script m_callLog;
    private List<Call_Group> m_callList;
    private float m_timeSinceLastCall;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables 
        m_roomManager = GameObject.FindObjectOfType<Room_Manager>();
        m_callLog = GameObject.FindObjectOfType<CallerLog_Script>();
        m_callList = new List<Call_Group>();
        m_timeSinceLastCall = 0.0f;
    }

    private void Update()
    {
        // Count up since the last call
        m_timeSinceLastCall += Time.deltaTime;

        // If enough time has passed, generate a new call
        if (m_timeSinceLastCall >= m_timeBetweenCalls)
            GenerateCall();
    }



    //--- Methods ---//
    public void GenerateCall()
    {
        // Create a new call group by randomly generating the necessary properties
        // The call capacity can be anywhere between 2 people and the highest room count (inclusive). The +1 is because the 
        // The wait time is randomly selected from a list set in the inspector
        // The call duration is randomly selected from a list set in the inspector
        int newCallCapacity = Random.Range(2, m_roomManager.HighestRoomCapacity + 1);
        float newCallWaitTime = m_possibleWaitTimes[Random.Range(0, m_possibleWaitTimes.Length)];
        float newCallLength = m_possibleCallLengths[Random.Range(0, m_possibleCallLengths.Length)];
        Call_Group newCall = new Call_Group(newCallCapacity, newCallWaitTime, newCallLength);

        // Hook into the call completion event so we can handle it finishing
        newCall.m_OnCallCompleted.AddListener(this.OnCallCompleted);

        // Add the call group to the list
        m_callList.Add(newCall);

        // Generate a new UI element for the call
        m_callLog.AddCallGroupUI(newCall);

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

            // TODO: Add points, reputation, etc
        }

        // Remove the UI element from the call backlog
        m_callLog.RemoveCallGroupUI(_callObj);

        // Either way, unhook the event and remove the call from the list
        _callObj.m_OnCallCompleted.RemoveAllListeners();
        m_callList.Remove(_callObj);
    }
}
