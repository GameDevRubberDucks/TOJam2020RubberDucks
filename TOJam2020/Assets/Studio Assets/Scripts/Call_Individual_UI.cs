using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Call_Individual_UI : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Main Controls")]
    public TextMeshProUGUI m_txtKeyBind;
    public GameObject m_selectionHighlight;
    public Animator m_animator;

    [Header("Colour Set A")]
    public Image m_imgBody;
    public Image m_imgHead;

    [Header("Colour Set B")]
    public Image m_imgMouth;
    public Image m_imgNose;
    public Image m_imgEyes;
    public Image m_imgBrow;
    public Image m_imgHair;



    //--- Private Variables ---//
    private Call_Individual m_refCaller;
    private Caller_Randomization_Manager m_random;



    //--- Unity Methods ---//
    private void Update()
    {
        UpdateUI();
    }



    //--- Methods ---//
    public void InitWithData(Call_Individual _refCaller)
    {
        // Store the refernce to the caller object for later
        m_refCaller = _refCaller;

        // Update the UI
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Grab the caller's keybind and show it on the UI
        KeyCode callerKeybind = m_refCaller.BoundKeyCode;
        m_txtKeyBind.text = callerKeybind.ToString();

        // Show / hide the selection highlight to match the selection state
        m_selectionHighlight.SetActive(m_refCaller.IsSelected);

        // Randomize the caller's look
        RandomizeLook();
    }

    public void RandomizeLook()
    {
        // Find the randomization manager so we can pull the sprites from it
        m_random = GameObject.FindObjectOfType<Caller_Randomization_Manager>();

        // Get the caller's unique ID and use it to seed the random so the randomization will always be the same for this caller
        int uniqueID = m_refCaller.ID;
        Random.InitState(uniqueID);

        // Get two random colours so we can colour the different parts of the body
        Color colourA = m_random.RandomColour;
        Color colourB = m_random.RandomColour;

        // If the two colours match, continue randomizing until they don't any longer
        while (colourB == colourA)
            colourB = m_random.RandomColour;

        // Generate the body features in groups so everything in the group has the same colour
        GenerateColourGroupA(colourA);
        GenerateColourGroupB(colourB);

        // Configure the animator if in a chat room, otherwise, don't animate at all
        if (GetComponentInParent<Key_Animator>() != null)
        {
            // If on the keyboard, don't animate at all
            m_animator.enabled = false;
        }
        else
        {
            // If in the call, animate with a random offset and a random speed
            float animatorOffset = Random.value;
            m_animator.SetFloat("Offset", animatorOffset);
            m_animator.speed = Random.value;
        }

        // Re-seed the random to something else now
        Random.InitState((int)Time.time);
    }

    public void GenerateColourGroupA(Color _colourA)
    {
        // Set the body and head
        m_imgBody.sprite = m_random.RandomBody;
        m_imgHead.sprite = m_random.RandomHead;

        // Colour the body and head together
        m_imgBody.color = _colourA;
        m_imgHead.color = _colourA;
    }

    public void GenerateColourGroupB(Color _colourB)
    {
        // Set the facial features
        m_imgMouth.sprite = m_random.RandomMouth;
        m_imgNose.sprite = m_random.RandomNose;
        m_imgEyes.sprite = m_random.RandomEyes;
        m_imgBrow.sprite = m_random.RandomBrows;
        m_imgHair.sprite = m_random.RandomHair;

        // Colour the facial features together
        m_imgMouth.color = _colourB;
        m_imgNose.color = _colourB;
        m_imgEyes.color = _colourB;
        m_imgBrow.color = _colourB;
        m_imgHair.color = _colourB;
    }
}
