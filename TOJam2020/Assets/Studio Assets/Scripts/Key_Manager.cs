using UnityEngine;

public class Key_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public KeyCode m_swapKey;
    public KeyCode m_deselectKey;
    public KeyCode m_disconnectKey;



    //--- Unity Methods ---//
    private void Update()
    {
        // Handle the room keys, which are 1 - 5 on the top of the keyboard (not the numpad)
        for (var roomKeyCode = KeyCode.Alpha1; roomKeyCode <= KeyCode.Alpha5; roomKeyCode++)
        {
            // Check each room key individually
            if (Input.GetKeyDown(roomKeyCode))
            {
                Debug.Log("Room Key: " + roomKeyCode);
            }
        }



        // Handle the alphabet keys
        for (var alphaKeyCode = KeyCode.A; alphaKeyCode <= KeyCode.Z; alphaKeyCode++)
        {
            // Check each alphabet key individually
            if (Input.GetKeyDown(alphaKeyCode))
            {
                Debug.Log("Alpha Key: " + alphaKeyCode);
            }
        }



        // Handle the special keys
        if (Input.GetKeyDown(m_swapKey))
        {
            Debug.Log("Swap Key");
        }
        else if (Input.GetKeyDown(m_deselectKey))
        {
            Debug.Log("Deselect Key");
        }
        else if (Input.GetKeyDown(m_disconnectKey))
        {
            Debug.Log("Disconnect Key");
        }
    }
}
