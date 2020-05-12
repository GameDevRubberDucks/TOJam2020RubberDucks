using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Room_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public List<Room> m_rooms;
 


    //--- Unity Methods ---//
    private void Awake()
    {
        // Clear the rooms and set them up
        foreach (var room in m_rooms)
            room.ClearCallerList();
    }



    //--- Methods ---//
    public void AddUnassignedCallers(List<Call_Individual> _callers)
    {
        // Place all of the callers into the unassigned room
        foreach (var caller in _callers)
            m_rooms[(int)Room_Name.Unassigned].AddCallers(_callers);
    }

    public bool TransferCallers(List<Call_Individual> _callers, Room_Name _endRoom)
    {
        // Get the end room object
        Room endRoom = m_rooms[(int)_endRoom];

        // Ensure the end target room has enough space for everyone. If not, return false to say it failed
        if (!endRoom.CheckForAdd(_callers))
            return false;

        // Loop through all of the callers and check all of the transfers to make sure they are all possible
        foreach(var caller in _callers)
        {
            // Get the caller's starting room
            Room startRoom = m_rooms[(int)caller.CurrentRoom];

            // Check to see that the transfer is viable from the caller's current room
            bool canPerformTransfer = startRoom.CheckForRemoval(caller);

            // If the transfer cannot be performed, return false to indicate that it won't work
            if (!canPerformTransfer)
                return false;
        }

        // If everything checked out, it is now time to perform the actual transfers
        foreach(var caller in _callers)
        {
            // Get the caller's starting room
            Room startRoom = m_rooms[(int)caller.CurrentRoom];

            // Remove the caller from its current room
            bool removeSuccess = startRoom.RemoveCaller(caller);

            // Add it to the new room
            bool addSuccess = endRoom.AddCaller(caller);

            // If either of the transfer components failed for some reason, we should return false to say this
            if (!removeSuccess || !addSuccess)
                return false;
        }

        // Return true to indicate everything worked
        return true;
    }

    public void DisconnectCallers(List<Call_Individual> _callers)
    {
        // Remove all of the callers from their rooms
        foreach(var caller in _callers)
        {
            // Get the room the caller is currently in
            Room room = m_rooms[(int)caller.CurrentRoom];

            // Remove the user from the room
            room.RemoveCaller(caller);
        }
    }

    public int GetCurrentCapacity(Room_Name _roomName)
    {
        // Return the current capacity of the requested room
        return m_rooms[(int)_roomName].CurrentCapacity;
    }



    //--- Getters ---//
    public int HighestRoomCapacity
    {
        get
        {
            // Start with a 0 capacity
            int maxCapacity = 0;

            // Loop through the chat rooms and determine which has the highest capacity
            for (var roomName = Room_Name.Chat_1; roomName <= Room_Name.Chat_5; roomName++)
            {
                // Grab the room reference
                Room roomRef = m_rooms[(int)roomName];

                // If the selected room is inactive, just move on to the next one
                if (!roomRef.IsActive)
                    continue;

                // Otherwise, compare against the current max capacity and update it if need be
                maxCapacity = (roomRef.MaxCapacity > maxCapacity) ? roomRef.MaxCapacity : maxCapacity;
            }

            // Return the highest capacity
            return maxCapacity;
        }
    }
}
