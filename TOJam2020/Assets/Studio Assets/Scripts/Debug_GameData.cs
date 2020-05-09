using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Debug_GameData : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Controls")]
    public bool m_debugEnabled;

    [Header("Room Manager")]
    public Room_Manager m_roomManager;
    public List<Text> m_roomDebugText;

    [Header("Binding Manager")]
    public Binding_Manager m_bindingManager;
    public Text m_selectionText;
    public Text m_bindingText;



    //--- Unity Methods ---//
    private void Update()
    {
        // Show the debug information
        if (m_debugEnabled)
        {
            // Output the various debug features
            ShowRoomDebugInfo();
            ShowSelectionInformation();
            ShowBindingInformation();
        }
    }



    //--- Methods ---//
    public void ShowRoomDebugInfo()
    {
        // Show the info for the unassigned room
        string unassignedRoomText = "Unassigned:\nMax Capacity: " + m_roomManager.m_rooms[0].MaxCapacity + "\nCurrent Capacity: " + m_roomManager.m_rooms[0].CurrentCapacity;
        m_roomDebugText[0].text = unassignedRoomText;

        // Show the info for the other rooms
        for (int i = 1; i < m_roomManager.m_rooms.Count; i++)
        {
            // Concatenate the letters for all of the callers in the room
            string occupantString = "";
            foreach (var caller in m_roomManager.m_rooms[i].Callers)
                occupantString += caller.BoundKeyCode.ToString();

            // Output the room info
            string roomText = m_roomManager.m_rooms[i].RoomName.ToString() + ":\nMax Capacity: " + m_roomManager.m_rooms[i].MaxCapacity + "\nCurrent Capacity: " + m_roomManager.m_rooms[i].CurrentCapacity + "\nIsActive: " + m_roomManager.m_rooms[i].IsActive.ToString() + "\nOccupants: " + occupantString;
            m_roomDebugText[i].text = roomText;
        }
    }

    public void ShowSelectionInformation()
    {
        // Concatenate all of the currently selected keys
        string selectionStr = "";
        foreach (var caller in m_bindingManager.SelectedCallers)
            selectionStr += caller.BoundKeyCode.ToString();

        // Output the text
        m_selectionText.text = "Current Selections: " + selectionStr;
    }

    public void ShowBindingInformation()
    {
        //// Grab the keybindings from the manager and get the individual keys and values as well
        //Dictionary<KeyCode, Call_Individual> keyBindings = m_bindingManager.KeyBindings;
        //var keys = keyBindings.Keys;
        //var values = keyBindings.Values;

        //// Going to hold all of the key bindings together in a vertical list
        //string bindingStr = "";

        //// Iterate through the dictionary information and set it up to be output
        //for(int i = 0; i < keys.Count && i < values.Count; i++)
        //    bindingStr += keys[i].

        //m_bindingText.text = m_bindingManager.KeyBindings.ToString();

        // Convert the dictionary to a string
        // Found here: https://stackoverflow.com/questions/5899171/is-there-anyway-to-handy-convert-a-dictionary-to-string
        string bindingTitle = "Key Bindings:\n";
        string bindingStr = string.Join(Environment.NewLine, m_bindingManager.KeyBindings);

        // Display the string
        m_bindingText.text = bindingTitle + bindingStr;
    }
}
