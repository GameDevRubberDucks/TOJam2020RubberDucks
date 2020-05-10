using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Call_Individual_UI : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Main Controls")]
    public TextMeshProUGUI m_txtKeyBind;
    public GameObject m_selectionHighlight;

    [Header("Randomization Controls")]
    public Image m_imgShirt;
    public Image m_imgFace;
    public Image m_imgEyes;
    public Image m_imgMouth;



    //--- Private Variables ---//
    private Call_Individual m_refCaller;



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
        var randomizationManager = GameObject.FindObjectOfType<Caller_Randomization_Manager>();

        // Get the caller's unique ID and use it to seed the random so the randomization will always be the same for this caller
        int uniqueID = m_refCaller.ID;
        Random.InitState(uniqueID);

        // Set the different features
        m_imgShirt.sprite = randomizationManager.RandomShirt;
        m_imgFace.sprite = randomizationManager.RandomFace;
        m_imgEyes.sprite = randomizationManager.RandomEyes;
        m_imgMouth.sprite = randomizationManager.RandomMouth;

        // Re-seed the random to something else now
        Random.InitState((int)Time.time);
    }
}
