using UnityEngine;

public class Call_Individual
{
    //--- Private Variables ---//
    private Room_Name m_currentRoom;



    //--- Constructors ---//
    public Call_Individual()
    {
        // Init the private variables
        m_currentRoom = Room_Name.Unassigned;
    }



    //--- Getters and Setters ---//
    public Room_Name CurrentRoom
    {
        get => m_currentRoom;
        set => m_currentRoom = value;
    }
}
