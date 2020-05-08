using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Room
{
    //--- Private Variables ---//
    private List<Call_Participant> m_callersInRoom;
    [SerializeField] private Room_Name m_roomName;
    [SerializeField] private int m_maxCapacity;
    private int m_currentCapacity;
    [SerializeField] private bool m_isActive;



    //--- Constructors ---//
    public Room(Room_Name _roomName, int _maxCapacity)
    {
        // Init the private variables
        m_callersInRoom = new List<Call_Participant>();
        m_roomName = _roomName;
        m_maxCapacity = _maxCapacity;
        m_currentCapacity = 0;
        m_isActive = false;
    }



    //--- Methods ---//
    public bool CheckForCapacity(List<Call_Participant> _newCallers)
    {
        // If this room isn't active, there is no capacity
        if (!m_isActive)
            return false;

        // If there isn't enough space to add the new callers, back out
        return (_newCallers.Count <= (m_maxCapacity - m_currentCapacity));
    }

    public bool AddParticipants(List<Call_Participant> _newCallers)
    {
        // Perform a check to ensure this will work
        if (!CheckForCapacity(_newCallers))
            return false;

        // Add the participants to the list
        foreach (var newCaller in _newCallers)
            m_callersInRoom.Add(newCaller);

        // Tell all of the participants that they are now in this room
        foreach (var activeCaller in m_callersInRoom)
            activeCaller.CurrentRoom = m_roomName;

        // Return true to indicate the add worked
        return true;
    }

    public bool CheckForRemoval(List<Call_Participant> _callersToRemove)
    {
        // Ensure all of the callers in the list are actually in this room
        foreach(var caller in _callersToRemove)
        {
            // If one of the callers isn't actually in this room, return false to say it won't work
            if (!m_callersInRoom.Contains(caller))
                return false;
        }

        // All of the callers are in the room so return true to indicate that the removal will work
        return true;
    }   

    public bool RemoveParticipants(List<Call_Participant> _callersToRemove)
    {
        // Perform a check to ensure this will work
        if (!CheckForRemoval(_callersToRemove))
            return false;

        // Remove the participant from this room
        foreach (var caller in _callersToRemove)
        {
            // Remove the caller from the list
            m_callersInRoom.Remove(caller);

            // The caller is now temporarily unassigned but should be re-assigned when being placed in another room
            caller.CurrentRoom = Room_Name.Unassigned;
        }

        // Return true to indicate the removal worked
        return true;
    }



    //--- Getters and Setters ---//
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
}
