using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CallerLog_Script : MonoBehaviour
{
    //--- Global Variable ---//
    //UI Variables
    public Image callerCountIcon;
    public Sprite[] callerCountImages;
    public TextMeshProUGUI callDuration;
    public Image timerBar;
    public Image timerClock;
    public TextMeshProUGUI[] bindingLetters;
    public GameObject[] blockingImages;
    public GameObject bindingModeIndicator;

    [Header("Patience Meter")]
    public Image patience_EmojiIndicator;
    public Sprite patience_HappyIcon;
    public Sprite patience_MediumIcon;
    public Sprite patience_AngerIcon;
    public Color patience_HappyColour;
    public Color patience_MediumColour;
    public Color patience_AngerColour;

    //Caller Variables

    private Call_Group refGroup;
    //Call_Group groupCall;
    private int numCallers = 0;
    private float waitTimeMax = 0.0f;
    private float waitTimeRemaining = 0.0f;
    private float callTimeMax = 0.0f;
    private float callTimeRemaining = 0.0f;

    private float waitTimePercent = 0.0f;
    private float callTimePercent = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        numCallers = refGroup.GetNumParticipants();
        waitTimeMax = refGroup.GetWaitTimeMax();
        waitTimeRemaining =refGroup.GetWaitTimeRemaining();
        callTimeMax = refGroup.GetCallTimeMax();
        callTimeRemaining = refGroup.GetCallTimeRemaining();

        callerCountIcon.sprite = callerCountImages[refGroup.GetNumParticipants() - 2];
        callDuration.GetComponent<TextMeshProUGUI>().text = callTimeMax.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        // Fill in the timer section
        float callTimePercentage = (callTimeMax - refGroup.GetCallTimeRemaining()) / callTimeMax;
        timerClock.GetComponent<Image>().fillAmount = callTimePercentage;

        // Lower the patience bar
        float waitTimePercentage = refGroup.GetWaitTimeRemaining() / waitTimeMax;
        timerBar.GetComponent<Image>().fillAmount = waitTimePercentage;

        // Set the happiness icon and the bar colour according to the patience remaining
        patience_EmojiIndicator.sprite = (waitTimePercentage >= 0.75f) ? patience_HappyIcon : (waitTimePercentage >= 0.25f) ? patience_MediumIcon : patience_AngerIcon;
        timerBar.color = (waitTimePercentage >= 0.75f) ? patience_HappyColour : (waitTimePercentage >= 0.25f) ? patience_MediumColour : patience_AngerColour;

        // Show / hide the binding mode indicator depending on if this group is currently selected for binding
        bindingModeIndicator.SetActive(refGroup.IsInBindMode);

        // Show all of the key bindings
        for (int i = 0; i < refGroup.CallParticipants.Count; i++)
        {
            // Grab the caller reference
            var caller = refGroup.CallParticipants[i];

            // If the binding is empty, just ignore it
            if (caller.BoundKeyCode == KeyCode.None)
                continue;

            // Otherwise, show the binding in the text box
            bindingLetters[i].text = caller.BoundKeyCode.ToString();
        }
    }

    public void InitWithData(Call_Group _attachedGroup)
    {
        refGroup = _attachedGroup;

        // Show only the number of key binding slots that are needed for this call
        for (int i = 0; i < _attachedGroup.GetNumParticipants(); i++)
            blockingImages[i].SetActive(false);
    }

    public void OnClick()
    {
        // Find the binding manager in the scene
        Binding_Manager bindingManager = GameObject.FindObjectOfType<Binding_Manager>();

        // Pass the binding manager the attached call group so it is able to place the keybindings afterwards
        bindingManager.CallGroupToBind = this.refGroup;
    }

    public Call_Group RefGroup
    {
        get => refGroup;
    }
}
