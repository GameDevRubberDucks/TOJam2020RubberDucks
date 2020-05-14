using UnityEngine;
using UnityEngine.UI;
public class Key_Animator : MonoBehaviour
{
    //--- Private Variables ---//
    private RectTransform m_rectTransform;
    private Shadow m_shadowObject;
    [SerializeField] private KeyCode m_attachedKey;
    private Rect m_baseRect;
    private float m_shadowHeight;
    public Audio_Manager keyAudio;


    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_shadowObject = GetComponent<Shadow>();
        m_rectTransform = this.GetComponent<RectTransform>();
        m_baseRect = m_rectTransform.rect;
        m_shadowHeight = m_shadowObject.effectDistance.y;
        keyAudio = GameObject.Find("AudioManager").GetComponent<Audio_Manager>();
    }

    private void Update()
    {
        // Move the key down on press and back up on release
        if (Input.GetKeyDown(m_attachedKey))
        {
            // Remove the shadow
            m_shadowObject.effectDistance = Vector2.zero;

            // Lower the key visualization by the same amount as the shadow was previously
            m_rectTransform.anchoredPosition = m_baseRect.position + new Vector2(0.0f, m_shadowHeight);

            //Play Audio
            keyAudio.PlayOneShot(0,1.0f);
            
        }
        else if (Input.GetKeyUp(m_attachedKey))
        {
            // Place the shadow back
            m_shadowObject.effectDistance = new Vector2(0.0f, m_shadowHeight);

            // Move the key visuals back
            m_rectTransform.anchoredPosition = m_baseRect.position;

            //play Audio
            keyAudio.PlayOneShot(1,1.0f);
        }
    }



    //--- Setters and Getters ---//
    public KeyCode AttachedKeyCode
    {
        set => m_attachedKey = value;
    }
}
