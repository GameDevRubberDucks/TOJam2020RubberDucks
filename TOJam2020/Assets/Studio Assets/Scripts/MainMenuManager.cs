using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject creditObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // load and start the main game Scene
    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
    // Bring in Credit Page
    public void Credit()
    {
        creditObject.SetActive(true);
    }
    //Quit and Exit game
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitCredit()
    {
        creditObject.SetActive(false);
    }
}
