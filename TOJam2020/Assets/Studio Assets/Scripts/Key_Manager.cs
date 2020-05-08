using UnityEngine;
using System.Collections.Generic;

public class Key_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public KeyCode m_swapKey;
    public KeyCode m_deselectKey;
    public KeyCode m_disconnectKey;



    //--- Private Variables ---//
    private Binding_Manager m_bindingManager;
    private Room_Manager m_roomManager;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_bindingManager = GameObject.FindObjectOfType<Binding_Manager>();
        m_roomManager = GameObject.FindObjectOfType<Room_Manager>();
    }

    private void Update()
    {
        // Handle the room keys, which are 1 - 5 on the top of the keyboard (not the numpad)
        for (var roomKeyCode = KeyCode.Alpha1; roomKeyCode <= KeyCode.Alpha5; roomKeyCode++)
        {
            // Check each room key individually
            if (Input.GetKeyDown(roomKeyCode))
            {
                // Get all of the selected callers
                List<Call_Individual> selectedCallers = m_bindingManager.SelectedCallers;

                // Try to transfer all of the selected callers to the given room
                m_roomManager.TransferCallers(selectedCallers, GetRoomNameFromKeyCode(roomKeyCode));

                // Clear the selection
                m_bindingManager.DeselectAll();
            }
        }



        // Handle the alphabet keys
        for (var alphaKeyCode = KeyCode.A; alphaKeyCode <= KeyCode.Z; alphaKeyCode++)
        {
            // Check each alphabet key individually
            if (Input.GetKeyDown(alphaKeyCode))
            {
                // Pass the keycode to the binding manager so it can manage swapping or selecting
                m_bindingManager.HandleLetterKeyPressed(alphaKeyCode);
            }
        }



        // Handle the special keys
        if (Input.GetKeyDown(m_swapKey))
        {
            // Switch in and out of swap mode
            m_bindingManager.ToggleSwapMode();
        }
        else if (Input.GetKeyDown(m_deselectKey))
        {
            // Deselect all of the selected callers at once
            m_bindingManager.DeselectAll();
        }
        else if (Input.GetKeyDown(m_disconnectKey))
        {
            // Get all of the selected callers
            List<Call_Individual> selectedCallers = m_bindingManager.SelectedCallers;

            // Try to transfer all of the selected callers to the waiting room
            m_roomManager.TransferCallers(selectedCallers, Room_Name.Waiting);

            // Clear the selection
            m_bindingManager.DeselectAll();
        }
    }



    //--- Utility Functions ---//
    public Room_Name GetRoomNameFromKeyCode(KeyCode _code)
    {
        switch(_code)
        {
            case KeyCode.Alpha1:
                return Room_Name.Chat_1;

            case KeyCode.Alpha2:
                return Room_Name.Chat_2;

            case KeyCode.Alpha3:
                return Room_Name.Chat_3;

            case KeyCode.Alpha4:
                return Room_Name.Chat_4;

            case KeyCode.Alpha5:
                return Room_Name.Chat_5;

            default:
                return Room_Name.Unassigned;
        }
    }
}
