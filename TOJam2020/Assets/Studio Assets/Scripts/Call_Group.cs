using UnityEngine;
using UnityEngine.Events;

public class Call_Group
{
    //--- Public Events ---/
    public Call_CompletionEvent m_OnCallCompleted;



    //--- Private Variables ---//
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
        m_callState = Call_State.Waiting;
        m_numParticipants = _numParticipants;
        m_waitTimeMax = _waitTimeMax;
        m_waitTimeRemaining = m_waitTimeMax;
        m_callTimeMax = _callTimeMax;
        m_callTimeRemaining = m_callTimeMax;
    }



    //--- Methods ---//
    public void UpdateCall()
    {
        // Update the current state depending on if individual callers have moved around
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
        // TODO: Check in with the individual callers and see if they are in the same room
    }
}
