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

    private float waitTimePercent = 0.0f;
    private float callTimePercent = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        numCallers = 10;// refGroup.GetNumParticipants();
        waitTimeMax = 40.0f;// refGroup.GetWaitTimeMax();
        waitTimeRemaining = 20.0f;// refGroup.GetWaitTimeRemaining();
        callTimeMax = 90.0f;// refGroup.GetCallTimeMax();
        callTimeRemaining = 10.0f;// refGroup.GetCallTimeRemaining();

        callers.GetComponent<TextMeshProUGUI>().text= numCallers.ToString();
        callDuration.GetComponent<TextMeshProUGUI>().text = callTimeMax.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        waitTimePercent = waitTimeRemaining / waitTimeMax;
        callTimePercent = callTimeRemaining / callTimeMax;

        timerBar.GetComponent<Image>().fillAmount = waitTimePercent;

        timerClock.GetComponent<Image>().fillAmount = callTimePercent;
    }

    public void InitWithData(Call_Group _attachedGroup)
    {
        refGroup = _attachedGroup;
    }
}
