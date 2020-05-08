using UnityEngine;
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

    public bool TransferCallers(List<Call_Individual> _callers, Room _startRoom, Room _endRoom)
    {
        // Ensure that the rooms are capable of the planned removal / addition
        bool canTransfer = (_startRoom.CheckForRemoval(_callers) && _endRoom.CheckForAdd(_callers));

        // If the transfer cannot work, back out now
        if (!canTransfer)
            return false;

        // Otherwise, perform the transfer
        bool removeSuccess = _startRoom.RemoveCallers(_callers);
        bool addSuccess = _endRoom.AddCallers(_callers);

        // Return if the transfer was succesful on both ends
        return (removeSuccess && addSuccess);
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
