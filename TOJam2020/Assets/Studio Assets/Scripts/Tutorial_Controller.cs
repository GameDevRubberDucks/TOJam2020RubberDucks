using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform m_tutPanelParent;
    public GameObject m_prevIndicator;
    public GameObject m_nextIndicator;



    //--- Private Variables ---//
    private int m_currentPanel;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_currentPanel = 0;

        // Setup the panels
        ChangePanel();
    }

    private void Update()
    {
        // Switch between the panels using the arrow keys
        // Skip the rest by pressing enter
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Don't do anything if we are currently on the first panel
            if (m_currentPanel <= 0)
                return;

            // Otherwise, we should go back a panel
            m_currentPanel--;
            ChangePanel();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Don't do anything if we are currently on the last panel
            if (m_currentPanel >= m_tutPanelParent.childCount - 1)
                return;

            // Otherwise, we should go forward a panel
            m_currentPanel++;
            ChangePanel();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // Move to the main game and skip the rest of this tutorial
            SceneManager.LoadScene("Main");
        }
    }

    public void ChangePanel()
    {
        // Show / hide the arrow key indicators depending where we are in the panels
        m_prevIndicator.SetActive(m_currentPanel > 0);
        m_nextIndicator.SetActive(m_currentPanel < m_tutPanelParent.childCount - 1);

        // Disable all of the panels except for the active one
        for (int i = 0; i < m_tutPanelParent.childCount; i++)
            m_tutPanelParent.GetChild(i).gameObject.SetActive(i == m_currentPanel);
    }
}
