using UnityEngine;
using System.Collections.Generic;

public class Room_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public List<Room> m_rooms;
    public int m_numActiveRooms;

    

    //--- Methods ---//
    public bool TransferCallers(List<Call_Participant> _callers, Room _startRoom, Room _endRoom)
    {
        // Ensure that the rooms are capable of the planned removal / addition
        bool canTransfer = (_startRoom.CheckForRemoval(_callers) && _endRoom.CheckForCapacity(_callers));

        // If the transfer cannot work, back out now
        if (!canTransfer)
            return false;

        // Otherwise, perform the transfer
        bool removeSuccess = _startRoom.RemoveParticipants(_callers);
        bool addSuccess = _endRoom.AddParticipants(_callers);

        // Return if the transfer was succesful on both ends
        return (removeSuccess && addSuccess);
    }
}
