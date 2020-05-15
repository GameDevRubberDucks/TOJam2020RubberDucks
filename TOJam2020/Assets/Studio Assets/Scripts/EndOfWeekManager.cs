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
    string text;
    int textDisplayAmount;

    // Start is called before the first frame update
    void Start()
    {
        textDisplayAmount = 0;
        text = "This is a test asdlfkjal;sdj fl;askdjf;l kajsdf;lkjas dl;fkj ads;lkfhj aosdiuhfalkjsdn";
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
        foreach (char letter in text.ToCharArray())
        {
            endScreenText.text += letter;
            yield return new WaitForSeconds(textAnimationSpeed);
        }
    }
}
