using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[Serializable]
public class Room : MonoBehaviour
{
    //--- Public Variables ---//
    [HideInInspector] public UnityEvent m_OnRoomCallersChanged;



    //--- Private Variables ---//
    private List<Call_Individual> m_callersInRoom;
    [SerializeField] private Room_Name m_roomName;
    [SerializeField] private int m_maxCapacity;
    [SerializeField] private bool m_isActive;



    //--- Methods ---//
    public void ClearCallerList()
    {
        // Reset the callers currently in the room
        m_callersInRoom = new List<Call_Individual>();
    }

    public bool CheckForAdd()
    {
        // Check if there is space for at least one more person to be added or not
        return (m_maxCapacity - m_callersInRoom.Count > 0);
    }

    public bool CheckForAdd(List<Call_Individual> _newCallers)
    {
        // If this room isn't active, there is no capacity
        if (!m_isActive)
            return false;

        // If there isn't enough space to add the new callers, back out
        return (_newCallers.Count <= (m_maxCapacity - m_callersInRoom.Count));
    }

    public bool AddCaller(Call_Individual _newCaller)
    {
        // Ensure this will work
        if (!CheckForAdd())
            return false;

        // Tell the caller they now belong to this room
        _newCaller.CurrentRoom = m_roomName;

        // Add the caller to the list
        m_callersInRoom.Add(_newCaller);

        // Invoke the event to indicate the room has changed
        m_OnRoomCallersChanged.Invoke();

        // Return true to indicate the add worked
        return true;
    }

    public bool AddCallers(List<Call_Individual> _newCallers)
    {
        // Perform a check to ensure this will work
        if (!CheckForAdd(_newCallers))
            return false;

        // Add the new callers
        foreach (var caller in _newCallers)
        {
            // If adding failed, return false to indicate this
            if (!AddCaller(caller))
                return false;
        }

        // Return true to indicate the add worked
        return true;
    }

    public bool CheckForRemoval(Call_Individual _callerToRemove)
    {
        // Return true if the caller is in the room, false otherwise
        return (m_callersInRoom.Contains(_callerToRemove));
    }

    public bool CheckForRemoval(List<Call_Individual> _callersToRemove)
    {
        // Ensure all of the callers in the list are actually in this room
        foreach(var caller in _callersToRemove)
        {
            // If one of the callers isn't actually in this room, return false to say it won't work
            if (!CheckForRemoval(caller))
                return false;
        }

        // All of the callers are in the room so return true to indicate that the removal will work
        return true;
    }   

    public bool RemoveCaller(Call_Individual _callerToRemove)
    {
        // Perform a check to ensure this will work
        if (!CheckForRemoval(_callerToRemove))
            return false;

        // Remove the caller from the list
        m_callersInRoom.Remove(_callerToRemove);

        // The caller is now temporarily unassigned but should be re-assigned when being placed in another room
        _callerToRemove.CurrentRoom = Room_Name.Unassigned;

        // Invoke the event to indicate the room has changed
        m_OnRoomCallersChanged.Invoke();

        // Return true to indicate everything worked
        return true;
    }

    public bool RemoveCallers(List<Call_Individual> _callersToRemove)
    {
        // Perform a check to ensure this will work
        if (!CheckForRemoval(_callersToRemove))
            return false;

        // Remove the participant from this room
        foreach (var caller in _callersToRemove)
        {
            // Remove the caller but return false if it fails
            if (!RemoveCaller(caller))
                return false;
        }

        // Return true to indicate the removal worked
        return true;
    }



    //--- Getters and Setters ---//
    public Room_Name RoomName
    {
        get => m_roomName;
        set => m_roomName = value;
    }

    public int MaxCapacity
    {
        get => m_maxCapacity;
        set => m_maxCapacity = value;
    }

    public bool IsActive
    {
        get => m_isActive;
        set => m_isActive = value;
    }

    public int CurrentCapacity
    {
        get => m_callersInRoom.Count;
    }

    public List<Call_Individual> Callers
    {
        get => m_callersInRoom;
    }
}
