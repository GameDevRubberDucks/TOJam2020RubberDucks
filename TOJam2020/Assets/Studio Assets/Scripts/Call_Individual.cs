using UnityEngine;

public class Call_Individual
{
    //--- Private Variables ---//
    private Room_Name m_currentRoom;
    private KeyCode m_boundKeyCode;
    private bool m_isSelected;



    //--- Constructors ---//
    public Call_Individual()
    {
        // Init the private variables
        m_currentRoom = Room_Name.Unassigned;
        m_boundKeyCode = KeyCode.None;
        m_isSelected = false;
    }



    //--- Getters and Setters ---//
    public Room_Name CurrentRoom
    {
        get => m_currentRoom;
        set => m_currentRoom = value;
    }

    public KeyCode BoundKeyCode
    {
        get => m_boundKeyCode;
        set => m_boundKeyCode = value;
    }

    public bool IsSelected
    {
        get => m_isSelected;
        set => m_isSelected = value;
    }
}
