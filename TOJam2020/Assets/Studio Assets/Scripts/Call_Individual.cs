using UnityEngine;

public class Call_Participant
{
    //--- Private Variables ---//
    private KeyCode m_boundKeyCode;
    private Room_Name m_currentRoom;



    //--- Constructors ---//
    public Call_Participant()
    {
        // Init the private variables
        m_boundKeyCode = KeyCode.None;
        m_currentRoom = Room_Name.Unassigned;
    }



    //--- Getters and Setters ---//
    public KeyCode BoundKeyCode
    {
        get => m_boundKeyCode;
        set => m_boundKeyCode = value;
    }

    public Room_Name CurrentRoom
    {
        get => m_currentRoom;
        set => m_currentRoom = value;
    }
}
