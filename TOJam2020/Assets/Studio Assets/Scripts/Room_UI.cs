using UnityEngine;
using UnityEngine.UI;

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



    //--- Private Variables ---//
    private Room_UI_Layout m_activeLayout;



    //--- Unity Methods ---//
    private void OnEnable()
    {
        // Hook into the room's listener
        m_roomRef.m_OnRoomCallersChanged.AddListener(this.OnRoomUpdated);
    }

    private void OnDisable()
    {
        // Unhook from the room's listener
        m_roomRef.m_OnRoomCallersChanged.RemoveListener(this.OnRoomUpdated);
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
        //m_screenNumberLabel.sprite = (m_roomRef.IsActive) ? m_screenNumberOn : m_screenNumberOff;

        // Enable the correct screen split layout for room depending on its current size
        m_activeLayout = m_screenLayouts[m_roomRef.MaxCapacity - 2];
        m_activeLayout.SetLayoutVisible(true);
    }

    public void OnRoomUpdated()
    {
        // Place the new portraits into the slots, based on the callers in the room
        m_activeLayout.PlacePortraits(m_roomRef.Callers);
    }
}
