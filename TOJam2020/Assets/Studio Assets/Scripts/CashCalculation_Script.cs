using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashCalculation_Script : MonoBehaviour
{
    private float totalCash = 0.0f;
    private const float cashCallComplete = 100.0f;
    private const float fullCallCompleteBonus = 100.0f;
    private const float cashPerPersonInCall = 200.0f;
    private float patienceLeft = 0.0f; 
    private float timeLeftPercent = 0.0f; 
    private float timeMax = 0.0f;
    private const float endOfDayBonus = 25.0f;
    private float dayCounter = 0.0f;
    private const float streakConst = 10.0f;
    private float streak = 0.0f;


    private float daysEarning = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        
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
        if (patienceLeft <= 0.33)
        {
            patiencePenalty = 0.0f;
        }
        else
        {
            patiencePenalty = patienceLeft;
        }
        
        timeMax = disGroup.GetCallTimeMax();
        //Bonus for completing a call. Depending on the amount of time left in the call. Less time = more money , More time = less money
        timeLeftPercent = disGroup.GetCallTimeRemaining() / timeMax;
        float timeLeftBonus = cashCallComplete * (1.0f - timeLeftPercent) * patiencePenalty;

        //Calculates Streak bonus
        float streakBonus = streakConst * streak;

        

        newCash = perPersonCash + timeLeftBonus + fullCallCompleteBonus + streakBonus;

        totalCash += newCash;

        daysEarning += newCash;
        return totalCash;
    }

    //Called to see how much you made in one day
    public float CalculateCashForDay ()
    {
        //Calculates day bonus
        float dayBonus = endOfDayBonus * dayCounter;

        daysEarning += dayBonus;

        return daysEarning;
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
