using UnityEngine;
using System.Collections.Generic;

public class Call_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public int m_MAX_NUM_CALLS;
    public float m_timeBetweenCalls;



    //--- Private Variables ---//
    private List<Call_Group> m_callList;
    private float m_timeSinceLastCall;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables 
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
        // Create a new call group
        Call_Group newCall = new Call_Group(0, 0, 0);

        // Hook into the call completion event so we can handle it finishing
        newCall.m_OnCallCompleted.AddListener(this.OnCallCompleted);

        // Add the call group to the list
        m_callList.Add(newCall);

        // TODO: Generate a new UI element for the call
        // ...

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

        // TODO: Remove the UI element from the call backlog
        // ...

        // Either way, unhook the event and remove the call from the list
        _callObj.m_OnCallCompleted.RemoveAllListeners();
        m_callList.Remove(_callObj);
    }
}
