using UnityEngine;
using System.Collections.Generic;

public class Call_PortraitUI_Manager : MonoBehaviour
{
    //--- Helper Structures ---//
    private struct Call_PortraitSet
    {
        public Call_Individual_UI m_keyboardObj;
        public Call_Individual_UI m_roomObj;
        public Call_Individual_UI m_callLogObj;
    }



    //--- Public Variables ---//
    [Header("Controls")]
    public GameObject m_callerUIPrefab;

    [Header("Keyboard")]
    public Transform[] m_keyboardLocations;



    //--- Private Variables ---//
    private Dictionary<Call_Individual, Call_PortraitSet> m_callerSets;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_callerSets = new Dictionary<Call_Individual, Call_PortraitSet>();
    }

    

    //--- Methods ---//
    public void GeneratePortraitSet(Call_Individual _newCaller)
    {
        // Grab the new caller's bound key
        KeyCode boundKey = _newCaller.BoundKeyCode;

        // Get the key object that we should spawn under based on the key code
        Transform keyParent = m_keyboardLocations[GetKeyboardIndex(boundKey)];

        // Create a new portrait set for the caller
        Call_PortraitSet newSet = new Call_PortraitSet();

        // Instantiate the portrait onto the keyboard and grab the UI script ref from it
        GameObject keyboardPortrait = Instantiate(m_callerUIPrefab, keyParent);
        Call_Individual_UI keyboardPortraitComp = keyboardPortrait.GetComponent<Call_Individual_UI>();

        // Initialize the UI component and then save it to the newly generated set
        keyboardPortraitComp.InitWithData(_newCaller);
        newSet.m_keyboardObj = keyboardPortraitComp;

        // Store the set into the list
        m_callerSets.Add(_newCaller, newSet);
    }

    public void DestroyPortraitSet(Call_Individual _callerToDestroy)
    {

    }



    //--- Utility Methods ---//
    private int GetKeyboardIndex(KeyCode _keyCode)
    {
        // Get the keyboard location by checking how far the keycode is offset from A
        return (_keyCode - KeyCode.A);
    }
}
