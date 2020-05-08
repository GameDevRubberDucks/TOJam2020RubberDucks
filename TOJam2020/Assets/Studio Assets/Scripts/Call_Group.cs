﻿using UnityEngine;
using System.Collections.Generic;

public class Call_Group
{
    //--- Public Events ---/
    public Call_CompletionEvent m_OnCallCompleted;



    //--- Private Variables ---//
    private List<Call_Participant> m_callParticipants;
    private Call_State m_callState;
    private int m_numParticipants;
    private float m_waitTimeMax;
    private float m_waitTimeRemaining;
    private float m_callTimeMax;
    private float m_callTimeRemaining;



    //--- Constructors ---//
    public Call_Group(int _numParticipants, float _waitTimeMax, float _callTimeMax)
    {
        // Init the private data
        m_callParticipants = new List<Call_Participant>();
        m_callState = Call_State.Waiting;
        m_numParticipants = _numParticipants;
        m_waitTimeMax = _waitTimeMax;
        m_waitTimeRemaining = m_waitTimeMax;
        m_callTimeMax = _callTimeMax;
        m_callTimeRemaining = m_callTimeMax;

        // Create the call participants and keep track of them
        for (int i = 0; i < m_numParticipants; i++)
            m_callParticipants.Add(new Call_Participant());
    }



    //--- Methods ---//
    public void UpdateCall()
    {
        // Update the current state to see if all the participants are in a chatroom together
        UpdateCallState();

        // Depending on the state, handle the timer update
        if (m_callState == Call_State.Waiting)
        {
            // Lower the patience meter
            m_waitTimeRemaining -= Time.deltaTime;

            // TODO: Update the UI representations of this wait time
            // ...

            // The callers waited too long and ran out of patience
            if (m_waitTimeRemaining <= 0.0f)
                m_OnCallCompleted.Invoke(this, Call_State.Waited_Too_Long);
        }
        else if (m_callState == Call_State.Active)
        {
            // Lower the call time
            m_callTimeRemaining -= Time.deltaTime;

            // TODO: Update the UI representations of the call time
            // ...

            // The callers completed their call successfully
            if (m_callTimeRemaining <= 0.0f)
                m_OnCallCompleted.Invoke(this, Call_State.Completed);
        }
    }

    public void UpdateCallState()
    {
        // Grab the first caller's room so we can see if everyone else is there too
        Room_Name firstCallerRoom = m_callParticipants[0].CurrentRoom;

        // Check in with the individual callers and see if they are in the same room
        foreach(var caller in m_callParticipants)
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
            m_callState = Call_State.Active;
        else
            m_callState = Call_State.Waiting;
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

}
