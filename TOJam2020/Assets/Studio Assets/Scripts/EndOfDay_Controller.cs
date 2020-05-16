using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class EndOfDay_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    public Room_UI[] m_roomUIObjs;
    public GameObject m_continueUI;
    public GameObject m_upgradeSelectionIndicator;
    public TextMeshProUGUI m_dayText;
    public TextMeshProUGUI m_moneyText;



    //--- Private Variables ---//
    private Persistence_Manager m_persistence;
    private List<Room> m_roomObjs;
    private int m_selectedRoom;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_persistence = GameObject.FindObjectOfType<Persistence_Manager>();
        m_selectedRoom = -1;
        m_roomObjs = new List<Room>();
        foreach (var roomUI in m_roomUIObjs)
            m_roomObjs.Add(roomUI.RoomRef);

        // Set the day and money texts
        m_dayText.text = "Day " + (m_persistence.m_dayNumber + 1);
        m_moneyText.text = "$" + m_persistence.m_totalMoney;
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
            // Save out the room sizes to the persistence manager
            for (int i = 0; i < m_roomObjs.Count; i++)
                m_persistence.m_chatRoomSizes[i] = m_roomObjs[i].MaxCapacity;
            
            // Move to the next day
            SceneManager.LoadScene("Main");
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
                else if (m_selectedRoom == -1) // If nothing is currently upgraded
                {
                    // If the room cannot be upgraded, keep going
                    if (m_roomObjs[i].MaxCapacity == 5)
                        continue;

                    // Upgrade the room to its new state
                    UpgradeRoom(i);

                    // This is the newly selected room
                    m_selectedRoom = i;
                }
                else // Another room is currently set to be upgraded, but they are swapping to another room
                {
                    // If the room cannot be upgraded, keep going
                    if (m_roomObjs[i].MaxCapacity == 5)
                        continue;

                    // Downgrade the previous room
                    DowngradeRoom(m_selectedRoom);

                    // Upgrade the new room
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
