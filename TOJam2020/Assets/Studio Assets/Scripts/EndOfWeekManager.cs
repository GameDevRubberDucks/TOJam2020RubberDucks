using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class EndOfWeekManager : MonoBehaviour
{
    //public End text input
    public TextMeshProUGUI endScreenText;
    public float textAnimationSpeed;

    //private
    Persistence_Manager PManager;
    string Finaltext;
    int totalMoney;
    int callMissed;
    float customerSat;
    string operatorEVA;
    string operatorFinalStat;


    // Start is called before the first frame update
    void Start()
    {
        PManager = GameObject.Find("PersistenceManager").GetComponent<Persistence_Manager>();
        totalMoney = PManager.m_totalMoney; // make sure to link up with Persistence Manager;

        //get the number of calls missed from the Persistence Manager
        callMissed = PManager.callsMissed;

        //get the total costumer satisfaction from persistence manager
        customerSat = PManager.GetGameSatisfaction();

        //stores the final text to be displayed
        Finaltext = " ";

        if (callMissed <= 5 && customerSat >= 70.0f)
        {
            operatorEVA = "Fair";
            operatorFinalStat = "Come Back Tommorrow \n You are \nNOT FIRED";
        }
        else if (callMissed <= 10 && customerSat >= 50.0f)
        {
            operatorEVA = "Average";
            operatorFinalStat = "Come Back Tommorrow \n You are \nALMOST FIRED";
        }
        else
        {
            operatorEVA = "BAD";
            operatorFinalStat = "Please Pack Your Desk\n You are \nFIRED";
        }


        //compose final text
        Finaltext = "WEEKLY SUMMARY \n ------------------------------ \n Revenue: $" + totalMoney +
          " \n\n Missed Call Requests: " + callMissed + " \n\n Customer Satisfaction: " + customerSat + "% \n\n ------------------------------ \n Operator Evaluation: \n" +
          operatorEVA + "\n\n" + operatorFinalStat +
          "\n\n Press Space to Return to Main Menu."
          ;

        //start coroutine for displaying final text one letter at a time
        StartCoroutine(TypeText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("Main Menu");
    }


    IEnumerator TypeText()
    {
        foreach (char letter in Finaltext.ToCharArray())
        {
            endScreenText.text += letter;
            yield return new WaitForSeconds(textAnimationSpeed);
        }
    }
}
