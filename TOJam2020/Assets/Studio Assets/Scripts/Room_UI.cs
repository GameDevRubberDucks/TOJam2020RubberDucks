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

    [Header("Room Indicator")]
    public Image m_screenNumberLabelImg;
    public Color m_screenNumberOff;
    public Color m_screenNumberOn;
    public Color m_screenNumberSelected;
    public TextMeshProUGUI m_screenNumberLabelText;
    public KeyCode m_roomNumberKey;



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

    private void Update()
    {
        // Update the screen light
        UpdateScreenLight();
    }



    //--- Methods ---//
    public void UpdateRoomDisplay()
    {
        // Depending on if the room is active, we should show or hide the blocker object
        m_disconnectedScreen.SetActive(!m_roomRef.IsActive);

        // Set the number light to the correct colour
        UpdateScreenLight();

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
        m_screenNumberLabelText.text = roomNumberStr;
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
        m_screenNumberLabelImg.color = m_screenNumberOff;
    }

    public void UpdateScreenLight()
    {
        // If the room is deactivated, use that colour
        // Otherwise, switch between the on and selected colours
        if (!m_roomRef.IsActive)
            m_screenNumberLabelImg.color = m_screenNumberOff;
        else
            m_screenNumberLabelImg.color = (Input.GetKey(m_roomNumberKey)) ? m_screenNumberSelected : m_screenNumberOn;
    }



    //--- Setters and Getters ---//
    public Room RoomRef
    {
        get => m_roomRef;
    }
}
