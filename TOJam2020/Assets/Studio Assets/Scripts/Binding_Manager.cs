using UnityEngine;
using System.Collections.Generic;

public class Binding_Manager : MonoBehaviour
{
    //--- Private Variables ---//
    private List<Call_Individual> m_selectedCallers;
    private Dictionary<KeyCode, Call_Individual> m_keyBindings;
    private KeyCode m_keyToSwap;
    private bool m_isInSwapMode;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_selectedCallers = new List<Call_Individual>();
        m_keyBindings = new Dictionary<KeyCode, Call_Individual>();
        m_keyToSwap = KeyCode.None;
        m_isInSwapMode = false;

        // Setup the dictionary to contain blank key bindings for all the letters of the alphabet
        for(var keyCode = KeyCode.A; keyCode <= KeyCode.Z; keyCode++)
            m_keyBindings.Add(keyCode, null);
    }



    //--- Methods ---//
    public bool ToggleSwapMode()
    {
        // Swap in and out of swap mode
        m_isInSwapMode = !m_isInSwapMode;

        // If now out of swap mode, reset the stored key that was going to be swapped
        m_keyToSwap = KeyCode.None;

        // Return the new value
        return m_isInSwapMode;
    }

    public void HandleLetterKeyPressed(KeyCode _alphabetKey)
    {
        // If in swap mode, we should prepare to switch the bindings. Otherwise, we should handle selections
        if (m_isInSwapMode)
        {
            // If this is the first alphabet key pressed since entering swap mode, we should store it so we can prepare to swap next time
            if (m_keyToSwap == KeyCode.None)
            {
                // Store the key so that next time, we are able to actually perform the swap
                m_keyToSwap = _alphabetKey;
            }
            else
            {
                // Swap the values stored at the newly entered key value and the previously stored one
                Call_Individual temp = m_keyBindings[m_keyToSwap];
                m_keyBindings[m_keyToSwap] = m_keyBindings[_alphabetKey];
                m_keyBindings[_alphabetKey] = temp;

                // Clear the held swap key
                m_keyToSwap = KeyCode.None;
            }
        }
        else
        {
            // Grab the call participant reference associated with the keycode
            Call_Individual caller = m_keyBindings[_alphabetKey];

            // Check if there is an actual caller bound to that key
            if (caller != null)
            {
                // If the caller is currently unselected, we should select them and vice versa
                if (m_selectedCallers.Contains(caller))
                    m_selectedCallers.Remove(caller);
                else
                    m_selectedCallers.Add(caller);
            }
        }
    }

    public void DeselectAll()
    {
        // Clear the full list of selections
        m_selectedCallers.Clear();
    }



    //--- Setters and Getters ---//
    public List<Call_Individual> SelectedCallers
    {
        get => m_selectedCallers;
    }
}
