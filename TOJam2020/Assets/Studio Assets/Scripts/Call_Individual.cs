using UnityEngine;

public class Call_Individual
{
    //--- Private Variables ---//
    private static int m_numTotal = 0;
    private Room_Name m_currentRoom;
    private KeyCode m_boundKeyCode;
    private bool m_isSelected;
    private int m_id;



    //--- Constructors ---//
    public Call_Individual()
    {
        // Init the private variables
        m_currentRoom = Room_Name.Unassigned;
        m_boundKeyCode = KeyCode.None;
        m_isSelected = false;

        // Randomize the first ID to ensure the characters are random each time
        if (m_numTotal == 0)
            m_numTotal = Random.Range(0, 500);
        m_id = m_numTotal++;
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

    public int ID
    {
        get => m_id;
    }
}
