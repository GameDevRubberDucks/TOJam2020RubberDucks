using UnityEngine;
using System.Collections.Generic;

public class EndOfDay_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    public Room_UI[] m_roomUIObjs;
    public GameObject m_continueUI;
    public GameObject m_upgradeSelectionIndicator;



    //--- Private Variables ---//
    private List<Room> m_roomObjs;
    private int m_selectedRoom;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_selectedRoom = -1;
        m_roomObjs = new List<Room>();
        foreach (var roomUI in m_roomUIObjs)
            m_roomObjs.Add(roomUI.RoomRef);
    }

    private void Update()
    {
        // Check for any interactions with the room
        CheckForRoomInteractions();

        // Show the continue UI and the selection indicator if a room has been upgraded
        m_continueUI.SetActive(m_selectedRoom != -1);
        m_upgradeSelectionIndicator.SetActive(m_selectedRoom != -1);

        // Move to the next day when space is pressed after and upgrade has been selected
        if (m_selectedRoom != -1 && Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: Move to the next day
            Debug.Log("Upgrade confirmed");
        }

        // Move the selection indicator over the room that is currently being upgraded
        if (m_selectedRoom != -1)
            m_upgradeSelectionIndicator.transform.position = m_roomUIObjs[m_selectedRoom].transform.position;
    }



    //--- Methods ---//
    public void CheckForRoomInteractions()
    {
        // Check if any of the room buttons have been pressed
        for (int i = 0; i < 5; i++)
        {
            // Determine the keycode for this room
            KeyCode roomKey = KeyCode.Alpha1 + i;

            // Check if the key was pressed
            if (Input.GetKeyDown(roomKey))
            {
                // If this room was already selected, we should downgrade it back to normal
                // Otherwise, we should upgrade it
                if (m_selectedRoom == i)
                {
                    // Downgrade the room back to the previous state
                    DowngradeRoom(i);

                    // Now, no upgrade is selected
                    m_selectedRoom = -1;
                }
                else if (m_selectedRoom == -1) // Should only be able to upgrade if the token has not already been used
                {
                    // If the room cannot be upgraded, keep going
                    if (m_roomObjs[i].MaxCapacity == 5)
                        continue;

                    // Upgrade the room to its new state
                    UpgradeRoom(i);

                    // This is the newly selected room
                    m_selectedRoom = i;
                }

                // No need to keep looping since we found the room that was selected
                break;
            }
        }
    }

    public void UpgradeRoom(int _roomIdx)
    {
        // Increase the room's capacity
        m_roomObjs[_roomIdx].ExpandRoom();

        // Update the room's UI
        m_roomUIObjs[_roomIdx].UpdateRoomDisplay();
    }

    public void DowngradeRoom(int _roomIdx)
    {
        // Lower the room's capacity
        m_roomObjs[_roomIdx].ShrinkRoom();

        // Update the room's UI
        m_roomUIObjs[_roomIdx].UpdateRoomDisplay();
    }
}
