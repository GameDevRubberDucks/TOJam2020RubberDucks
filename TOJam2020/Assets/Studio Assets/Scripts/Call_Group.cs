using UnityEngine;
using System.Collections.Generic;

public class Call_Group
{
    //--- Public Events ---/
    public Call_CompletionEvent m_OnCallCompleted;



    //--- Private Variables ---//
    private Room_Manager m_roomManager;
    private List<Call_Individual> m_callParticipants;
    private Call_State m_callState;
    private int m_numParticipants;
    private float m_waitTimeMax;
    private float m_waitTimeRemaining;
    private float m_callTimeMax;
    private float m_callTimeRemaining;
    private bool m_isInBindMode;
    private bool m_shouldCheckState;

    //--- Audio Variabls ---//
    private Audio_Manager audioManager = null;
    private Call_State prevState;



    //--- Constructors ---//
    public Call_Group(int _numParticipants, float _waitTimeMax, float _callTimeMax)
    {
        // Init the event
        m_OnCallCompleted = new Call_CompletionEvent();

        // Init the private data
        m_roomManager = GameObject.FindObjectOfType<Room_Manager>();
        m_callParticipants = new List<Call_Individual>();
        m_callState = Call_State.Waiting;
        m_numParticipants = _numParticipants;
        m_waitTimeMax = _waitTimeMax;
        m_waitTimeRemaining = m_waitTimeMax;
        m_callTimeMax = _callTimeMax;
        m_callTimeRemaining = m_callTimeMax;
        m_isInBindMode = false;
        m_shouldCheckState = true;

        // Create the call participants and keep track of them
        for (int i = 0; i < m_numParticipants; i++)
            m_callParticipants.Add(new Call_Individual());
    }



    //--- Methods ---//
    public void UpdateCall()
    {
        // Should always be checking the state, unless the game is paused or over
        if (m_shouldCheckState)
        {
            // Update the current state to see if all the participants are in a chatroom together
            UpdateCallState();

            // Depending on the state, handle the timer update
            if (m_callState == Call_State.Waiting)
            {
                // Lower the patience meter
                m_waitTimeRemaining -= Time.deltaTime;

                // The callers waited too long and ran out of patience
                if (m_waitTimeRemaining <= 0.0f)
                    m_OnCallCompleted.Invoke(this, Call_State.Waited_Too_Long);
            }
            else if (m_callState == Call_State.Active)
            {
                // Lower the call time
                m_callTimeRemaining -= Time.deltaTime;

                // The callers completed their call successfully
                if (m_callTimeRemaining <= 0.0f)
                    m_OnCallCompleted.Invoke(this, Call_State.Completed);
            }
        }
    }

    public void UpdateCallState()
    {
        // Grab the first caller's room so we can see if everyone else is there too
        Room_Name firstCallerRoom = m_callParticipants[0].CurrentRoom;

        // We need to check if it's ONLY this call group in this room. There shouldn't be another group in here
        if (m_roomManager.GetCurrentCapacity(firstCallerRoom) != m_numParticipants)
        {
            m_callState = Call_State.Waiting;
            return;
        }

        // Check in with the individual callers and see if they are in the same room
        foreach (var caller in m_callParticipants)
        {
            // If one of the participants is not in the same room, this call is in a waiting state by default
            if (firstCallerRoom != caller.CurrentRoom)
            {
                m_callState = Call_State.Waiting;
                return;
            }
        }

        // If all of the callers are in the same room, we still need to determine if they are actually in a chat room
        // If they are not in a chat room but they are all together, they are still waiting
        if (firstCallerRoom >= Room_Name.Chat_1 && firstCallerRoom <= Room_Name.Chat_5)
        { 
            m_callState = Call_State.Active;
            if (prevState == Call_State.Waiting)
            {
                if (audioManager == null)
                { 
                    audioManager = GameObject.Find("AudioManager").GetComponent<Audio_Manager>();
                }
                audioManager.PlayOneShot(2,1.0f);
            }
            prevState = m_callState;
        }
        else
        {
            m_callState = Call_State.Waiting;
            prevState = m_callState;
        }
    }


    //--- Getter Functions ---//
    public int GetNumParticipants()
    {
        return m_numParticipants;
    }

    public float GetWaitTimeMax()
    {
        return m_waitTimeMax;
    }

    public float GetWaitTimeRemaining()
    {
        return m_waitTimeRemaining;
    }

    public float GetCallTimeMax()
    {
        return m_callTimeMax;
    }

    public float GetCallTimeRemaining()
    {
        return m_callTimeRemaining;
    }




    //--- Setters and Getters ---//
    public List<Call_Individual> CallParticipants
    {
        get => m_callParticipants;
    }

    public bool IsInBindMode
    {
        get => m_isInBindMode;
        set => m_isInBindMode = value;
    }

    public bool ShouldUpdate
    {
        get => m_shouldCheckState;
        set => m_shouldCheckState = value;
    }
}
