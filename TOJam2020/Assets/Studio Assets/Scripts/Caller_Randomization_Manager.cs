using UnityEngine;

public class Caller_Randomization_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    public Sprite[] m_shirts;
    public Sprite[] m_faces;
    public Sprite[] m_eyes;
    public Sprite[] m_mouths;

    

    //--- Getters ---//
    public Sprite RandomShirt
    {
        get => m_shirts[Random.Range(0, m_shirts.Length)];
    }

    public Sprite RandomFace
    {
        get => m_faces[Random.Range(0, m_faces.Length)];
    }

    public Sprite RandomEyes
    {
        get => m_eyes[Random.Range(0, m_eyes.Length)];
    }

    public Sprite RandomMouth
    {
        get => m_mouths[Random.Range(0, m_mouths.Length)];
    }
}
