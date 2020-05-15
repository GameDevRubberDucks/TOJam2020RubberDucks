using UnityEngine;

public class Persistence_Manager : MonoBehaviour
{
    private static Persistence_Manager m_instance;

    public int[] m_chatRoomSizes;
    public int m_totalMoney;
    public int m_dayNumber;
    public int callsMissed;
    public int callsCompleted;
    public float gamesSatisfaction;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int GetRoomIdx(Room_Name _roomName)
    {
        return (_roomName - Room_Name.Chat_1);
    }


    //This will calculate the satisfaction of the game
    public float GetGameSatisfaction()
    {
        return gamesSatisfaction / callsCompleted;
    }
}
