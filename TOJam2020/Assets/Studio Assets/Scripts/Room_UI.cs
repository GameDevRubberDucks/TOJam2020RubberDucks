using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room_UI : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject m_disconnectedScreen;
    public Room_UI_Layout[] m_screenLayouts;
    public GameObject m_callerUIPrefab;
    public Room m_roomRef;
    public Image m_screenNumberLabel;
    public Sprite m_screenNumberOn;
    public Sprite m_screenNumberOff;
    public TextMeshProUGUI m_screenNumberIndicator;



    //--- Private Variables ---//
    private Room_UI_Layout m_activeLayout;



    //--- Unity Methods ---//
    private void OnEnable()
    {
        // Hook into the room's listener
        m_roomRef.m_OnRoomCallersChanged.AddListener(this.OnRoomUpdated);
        m_roomRef.m_OnRoomDeactivated.AddListener(this.OnRoomDeactivated);
    }

    private void OnDisable()
    {
        // Unhook from the room's listener
        m_roomRef.m_OnRoomCallersChanged.RemoveListener(this.OnRoomUpdated);
        m_roomRef.m_OnRoomDeactivated.RemoveListener(this.OnRoomDeactivated);
    }

    private void Start()
    {
        // Configure the room's UI layout
        UpdateRoomDisplay();
    }
    


    //--- Methods ---//
    public void UpdateRoomDisplay()
    {
        // Depending on if the room is active, we should show or hide the blocker object
        m_disconnectedScreen.SetActive(!m_roomRef.IsActive);

        // Change the label light to disconnected / connected state
        m_screenNumberLabel.sprite = (m_roomRef.IsActive) ? m_screenNumberOn : m_screenNumberOff;

        // Store the active layout
        if (m_roomRef.IsActive)
            m_activeLayout = m_screenLayouts[m_roomRef.MaxCapacity - 2];
        else
            m_activeLayout = null;

        // Enable the active layout and disable the other layouts
        foreach(var layout in m_screenLayouts)
            layout.SetLayoutVisible(layout == m_activeLayout);

        // Show the correct room number
        Room_Name roomName = m_roomRef.RoomName;
        string roomNameStr = roomName.ToString();
        string roomNumberStr = roomNameStr.Substring(roomNameStr.Length - 1, 1);
        m_screenNumberIndicator.text = roomNumberStr;
    }

    public void OnRoomUpdated()
    {
        // Place the new portraits into the slots, based on the callers in the room
        m_activeLayout.PlacePortraits(m_roomRef.Callers);
    }

    public void OnRoomDeactivated()
    {
        // Enable the room off indicators
        m_disconnectedScreen.SetActive(true);
        m_screenNumberLabel.sprite = m_screenNumberOff;
    }



    //--- Setters and Getters ---//
    public Room RoomRef
    {
        get => m_roomRef;
    }
}
