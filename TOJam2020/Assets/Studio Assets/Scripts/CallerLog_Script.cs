using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CallerLog_Script : MonoBehaviour
{
    //--- Global Variable ---//

    //UI Variables
    public TextMeshProUGUI callers;
    public TextMeshProUGUI callDuration;
    public Image timerBar;
    public Image timerClock;

    //Caller Variables

    private Call_Group refGroup;
    //Call_Group groupCall;
    private int numCallers = 0;
    private float waitTimeMax = 0.0f;
    private float waitTimeRemaining = 0.0f;
    private float callTimeMax = 0.0f;
    private float callTimeRemaining = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        numCallers = refGroup.GetNumParticipants();
        waitTimeMax = refGroup.GetWaitTimeMax();
        waitTimeRemaining = refGroup.GetWaitTimeRemaining();
        callTimeMax = refGroup.GetCallTimeMax();
        callTimeRemaining = refGroup.GetCallTimeRemaining();

        callers.GetComponent<TextMeshProUGUI>().text= numCallers.ToString();
        callDuration.GetComponent<TextMeshProUGUI>().text = callTimeMax.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        //timerBar.GetComponent<Image>().fillAmount = waitTimeRemaining / waitTimeMax;

        //timerClock.GetComponent<Image>().fillAmount = callTimeRemaining / callTimeMax;

        timerBar.GetComponent<Image>().fillAmount = refGroup.GetWaitTimeRemaining() / waitTimeMax;

        timerClock.GetComponent<Image>().fillAmount = refGroup.GetCallTimeRemaining() / callTimeMax;
    }

    public void InitWithData(Call_Group _attachedGroup)
    {
        refGroup = _attachedGroup;
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
