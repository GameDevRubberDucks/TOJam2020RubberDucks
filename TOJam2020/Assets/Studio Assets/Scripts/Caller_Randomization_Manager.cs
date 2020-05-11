using UnityEngine;

public class Caller_Randomization_Manager : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Colours")]
    public Color[] m_possibleColours;

    [Header("Colour Set A")]
    public Sprite[] m_bodies;
    public Sprite[] m_heads;

    [Header("Colour Set B")]
    public Sprite[] m_mouths;
    public Sprite[] m_noses;
    public Sprite[] m_eyes;
    public Sprite[] m_brows;
    public Sprite[] m_hairstyles;

    

    //--- Getters ---//
    public Color RandomColour { get => m_possibleColours[Random.Range(0, m_possibleColours.Length)]; }
    public Sprite RandomBody { get => m_bodies[Random.Range(0, m_bodies.Length)]; }
    public Sprite RandomHead { get => m_heads[Random.Range(0, m_heads.Length)]; }
    public Sprite RandomMouth { get => m_mouths[Random.Range(0, m_mouths.Length)]; }
    public Sprite RandomNose { get => m_noses[Random.Range(0, m_noses.Length)]; }
    public Sprite RandomEyes { get => m_eyes[Random.Range(0, m_eyes.Length)]; }
    public Sprite RandomBrows { get => m_brows[Random.Range(0, m_brows.Length)]; }
    public Sprite RandomHair { get => m_hairstyles[Random.Range(0, m_hairstyles.Length)]; }
}
