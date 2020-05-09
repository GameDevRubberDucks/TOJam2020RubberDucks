using UnityEngine;
using TMPro;

public class Call_Individual_UI : MonoBehaviour
{
    //--- Public Variables ---//
    public TextMeshProUGUI m_txtKeyBind;



    //--- Private Variables ---//
    private Call_Individual m_refCaller;

    

    //--- Methods ---//
    public void InitWithData(Call_Individual _refCaller)
    {
        // Store the refernce to the caller object for later
        m_refCaller = _refCaller;

        // Update the UI
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Grab the caller's keybind and show it on the UI
        KeyCode callerKeybind = m_refCaller.BoundKeyCode;
        m_txtKeyBind.text = callerKeybind.ToString();
    }
}
