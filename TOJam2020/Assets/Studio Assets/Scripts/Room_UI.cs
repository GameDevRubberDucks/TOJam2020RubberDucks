using UnityEngine;
using UnityEngine.UI;

public class Room_UI : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject m_disconnectIndicator;
    public GameObject[] m_screenLayouts;
    public Room m_roomRef;



    //--- Methods ---//
    public void InitWithData(Room _roomRef)
    {
        // Store the room ref
        m_roomRef = _roomRef;

        // Show the UI
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Depending on if the room is active, we should show or hide the blocker object
        m_disconnectIndicator.SetActive(!m_roomRef.IsActive);

        // Show the correct screen split layout for room depending on its current size
        m_screenLayouts[m_roomRef.MaxCapacity - 1].SetActive(true);
    }
}
