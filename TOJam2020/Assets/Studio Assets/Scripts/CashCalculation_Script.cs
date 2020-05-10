using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashCalculation_Script : MonoBehaviour
{
    public TextMeshProUGUI txtTotalCash;

    private float totalCash = 0.0f;
    private const float cashCallComplete = 100.0f;
    private const float fullCallCompleteBonus = 100.0f;
    private const float cashPerPersonInCall = 200.0f;
    private float patienceLeft = 0.0f; 
    private float timeLeftPercent = 0.0f; 
    private float timeMax = 0.0f;
    private const float endOfDayBonus = 25.0f;
    //private float dayCounter = 0.0f;
    private const float streakConst = 10.0f;
    private float streak = 0.0f;


    private float daysEarning = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // Display the total amount of money
        txtTotalCash.text = "$" + Mathf.FloorToInt(totalCash).ToString();
    }

    //Call this function when the group disconnects
    //
    public float CalculateCashForCall(Call_Group disGroup)
    {
        //Variable to store total of new cash to add to total
        float newCash = 0.0f;
        float patiencePenalty = 0.0f;

        //Calculates money based on how many people are in the call
        float perPersonCash = disGroup.GetNumParticipants() * cashPerPersonInCall;
        //Calculates the penalty for patience
        patienceLeft = disGroup.GetWaitTimeRemaining() / disGroup.GetWaitTimeMax(); 
        if (patienceLeft >= 0.66)
        {
            patiencePenalty = 1.0f;
        }
        else
        {
            patiencePenalty = patienceLeft;
        }
        
        //This is only for if they didnt complete the call fully 
        //Bonus for completing a call. Depending on the amount of time left in the call. Less time = more money , More time = less money
        timeMax = disGroup.GetCallTimeMax();
        timeLeftPercent = disGroup.GetCallTimeRemaining() / timeMax;
        float timeLeftBonus = cashCallComplete * (1.0f - timeLeftPercent) * patiencePenalty;  // Little off but not bad
        //Calculates Streak bonus
        float streakBonus = streakConst * streak;


        newCash = perPersonCash + timeLeftBonus + fullCallCompleteBonus + streakBonus;
        

        totalCash += newCash;
        
        //Adds the calls earned cash into the daily earning
        daysEarning += newCash;
        
        //--- Debugs to check cash values ---//
        //Debug.Log("Per Person " + perPersonCash);
        //Debug.Log("Patient Pen " + patiencePenalty);
        //Debug.Log("Tl bonus " + timeLeftBonus);
        //Debug.Log("Streak " + streakBonus);

        //Return the cash earned from the call
        return newCash;
    }

    //Called to see how much you made in one day
    public float CalculateCashForDay (int dayCounter)
    {
        //Calculates day bonus
        float dayBonus = endOfDayBonus * dayCounter;

        daysEarning += dayBonus;
        //Return the earnings for that day (all calls plus end of day bonus)
        return daysEarning;
    }


    //Get total cash
    public float TotalCashEarned()
    {
        return totalCash;
    }

    //Getter if we want a breakdown section at the end of days
    public float GetCashMadeFromCalls()
    {
        return 0.0f;
    }

    public float GetDayBonus()
    {
        return 0.0f;
    }

    public float GetStreakBonus()
    {
        return 0.0f;
    }


}
