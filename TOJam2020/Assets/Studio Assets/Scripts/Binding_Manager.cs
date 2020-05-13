using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Binding_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    [HideInInspector] public UnityEvent m_OnBindingsChanged; 



    //--- Private Variables ---//
    private List<Call_Individual> m_selectedCallers;
    private Dictionary<KeyCode, Call_Individual> m_keyBindings;
    private Room_Manager m_roomManager;
    private KeyCode m_keyToSwap;
    private bool m_isInSwapMode;
    private Call_Group m_groupToBind;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_selectedCallers = new List<Call_Individual>();
        m_keyBindings = new Dictionary<KeyCode, Call_Individual>();
        m_roomManager = GameObject.FindObjectOfType<Room_Manager>();
        m_keyToSwap = KeyCode.None;
        m_isInSwapMode = false;
        m_groupToBind = null;

        // Setup the dictionary to contain blank key bindings for all the letters of the alphabet
        for(var keyCode = KeyCode.A; keyCode <= KeyCode.Z; keyCode++)
            m_keyBindings.Add(keyCode, null);
    }



    //--- Methods ---//
    public bool SetSwapMode(bool _enabled)
    {
        // Set the swap mode
        m_isInSwapMode = _enabled;

        // If now out of swap mode, reset the stored key that was going to be swapped
        if (!m_isInSwapMode)
            m_keyToSwap = KeyCode.None;

        // Return the new value
        return m_isInSwapMode;
    }

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
        // If in swap mode, we should prepare to switch the bindings
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
                // Hold the caller temporarily so we can perform a swap
                Call_Individual tempCaller = m_keyBindings[m_keyToSwap];

                // Swap the bindings
                BindCallerToKey(m_keyToSwap, m_keyBindings[_alphabetKey]);
                BindCallerToKey(_alphabetKey, tempCaller);

                // Invoke the event since the key bindings have changed
                m_OnBindingsChanged.Invoke();

                // Clear the held swap key
                m_keyToSwap = KeyCode.None;
            }
        }
        else if (m_groupToBind != null) // Otherwise, if the player selected a call group in the backlog, we should be binding those mappings
        {
            // We should check if the binding is currently open. If it isn't, we should back out
            if (!CheckIfBindingOpen(_alphabetKey))
                return;

            // Grab the individual callers from the bound group
            List<Call_Individual> callers = m_groupToBind.CallParticipants;

            // Loop through and find the next one that needs a binding
            for (int i = 0; i < callers.Count; i++)
            {
                // Grab the caller reference
                var caller = callers[i];

                // If the caller is already bound, we can move on to the next one
                if (caller.BoundKeyCode != KeyCode.None)
                    continue;

                // Otherwise, we can go ahead and perform the binding
                BindCallerToKey(_alphabetKey, caller);

                // Invoke the event since the key bindings have changed
                m_OnBindingsChanged.Invoke();

                // We should also mark the caller as selected to make it easier to move it around after
                SelectCaller(caller);

                // We now also need to move the caller into the waiting room
                m_roomManager.TransferCallers(new List<Call_Individual> { caller }, Room_Name.Waiting);

                // If this is the last caller, then the group is fully bound and we can unlink it
                if (i == callers.Count - 1)
                {
                    // The group is no longer in bind mode
                    m_groupToBind.IsInBindMode = false;

                    // Stop tracking the group
                    m_groupToBind = null;
                }

                // Finally, we should break the loop to prevent binding the next caller to the same key
                break;
            }
        }
        else  // Otherwise, the player is selecting / deselecting a caller
        {
            // Grab the call participant reference associated with the keycode
            Call_Individual caller = m_keyBindings[_alphabetKey];

            // Check if there is an actual caller bound to that key
            if (caller != null)
            {
                // If the caller is currently unselected, we should select them and vice versa
                if (m_selectedCallers.Contains(caller))
                    DeselectCaller(caller);
                else
                    SelectCaller(caller);
            }
        }
    }

    public void SelectCaller(Call_Individual _caller)
    {
        // Set the caller's selected value
        _caller.IsSelected = true;

        // Add the caller to the selected list
        m_selectedCallers.Add(_caller);
    }

    public void DeselectCaller(Call_Individual _caller)
    {
        // Set the caller's selected value
        _caller.IsSelected = false;

        // Remove the caller from the selected list
        m_selectedCallers.Remove(_caller);
    }

    public void DeselectAll()
    {
        // Loop through and deselect all of the individual callers
        for (int i = 0; i < m_selectedCallers.Count; i++)
        {
            DeselectCaller(m_selectedCallers[i]);
            i--;
        }
    }

    public bool CheckIfBindingOpen(KeyCode _bindKey)
    {
        // Return true if the key binding is not associated with a caller
        return (m_keyBindings[_bindKey] == null);
    }

    public void BindCallerToKey(KeyCode _bindKey, Call_Individual _caller)
    {
        if (_bindKey == KeyCode.None)
        {
            Debug.Log("NONE");
        }

        // Bind the caller to the given key
        m_keyBindings[_bindKey] = _caller;

        // Ensure the caller slot is actually filled. It could be empty
        if (_caller != null)
            _caller.BoundKeyCode = _bindKey;
    }

    public void UnbindKey(KeyCode _bindKey)
    {
        // If the keycode isn't valid, just back out anyways
        if (_bindKey == KeyCode.None)
            return;

        // Remove the caller from the bound key
        m_keyBindings[_bindKey] = null;
    }

    public void OnCallGroupDisconnected(Call_Group _group, Call_State _finalState)
    {
        // Grab the call participants
        List<Call_Individual> callers = _group.CallParticipants;

        // Unbind all of the callers within the group
        foreach (var caller in callers)
            UnbindKey(caller.BoundKeyCode);

        // If the call group was the one that was bound, we should unbind it
        if (_group == m_groupToBind)
            m_groupToBind = null;

        // If any of the callers was in the selection, we should remove that as well
        foreach(var caller in callers)
        {
            if (m_selectedCallers.Contains(caller))
                DeselectCaller(caller);
        }

        // Invoke the event since the key bindings have changed
        m_OnBindingsChanged.Invoke();

        // Unhook the listener
        _group.m_OnCallCompleted.RemoveListener(this.OnCallGroupDisconnected);
    }



    //--- Setters and Getters ---//
    public List<Call_Individual> SelectedCallers
    {
        get => m_selectedCallers;
    }

    public Call_Group CallGroupToBind
    {
        set
        {
            // If there was a group previously set, turn it off
            if (m_groupToBind != null)
                m_groupToBind.IsInBindMode = false;

            // If a different group was selected previously, we should clear the selection
            if (m_groupToBind != value)
                DeselectAll();

            // If the new binding group is aleady fully bound, back out
            bool fullyBound = true;
            for (int i = 0; i < value.CallParticipants.Count; i++)
            {
                // If anyone in the group is not bound, we can continue to set this to binding mode
                if (value.CallParticipants[i].BoundKeyCode == KeyCode.None)
                {
                    fullyBound = false;
                    break;
                }
            }
            if (fullyBound)
                return;

            // Otherwise, if this is a valid call group to bind, store the group
            m_groupToBind = value;

            // The group is now in binding mode
            m_groupToBind.IsInBindMode = true;

            // Hook into the group's disconnect event so we can eventually unbind all the callers when it goes away
            value.m_OnCallCompleted.AddListener(OnCallGroupDisconnected);
        }
    }

    public Dictionary<KeyCode, Call_Individual> KeyBindings
    {
        get => m_keyBindings;
    }

    public bool IsInSwapMode
    {
        get => m_isInSwapMode;
    }

    public KeyCode KeyToSwap
    {
        get => m_keyToSwap;
    }
}
