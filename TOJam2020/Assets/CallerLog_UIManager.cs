using UnityEngine;
using System.Collections.Generic;

public class CallerLog_UIManager : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform m_uiListParent;
    public GameObject m_uiElementPrefab;



    //--- Private Variables ---//
    private List<CallerLog_Script> m_callerUIObjs;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_callerUIObjs = new List<CallerLog_Script>();
    }



    //--- Methods ---//
    public void AddCallGroupUI(Call_Group _group)
    {
        // Instantiate the prefab as a child of the list parent
        GameObject newUIElement = Instantiate(m_uiElementPrefab, m_uiListParent);

        // Grab the script off the prefab
        CallerLog_Script uiScript = newUIElement.GetComponent<CallerLog_Script>();

        // Pass the group to the script so that it can get set up
        uiScript.InitWithData(_group);

        // Store the script in the internal list
        m_callerUIObjs.Add(uiScript);
    }

    public void RemoveCallGroupUI(Call_Group _group)
    {
        // Need to find the associated script
        CallerLog_Script uiScript = null;

        // Find the UI element that is connected to the group
        foreach(var callerUI in m_callerUIObjs)
        {
            // If we found the right UI element, store in and stop searching 
            if (callerUI.RefGroup == _group)
            {
                uiScript = callerUI;
                break;
            }
        }

        // If it is still null, back out because it isn't in the list anyways
        if (uiScript == null)
        {
            Debug.LogError("No ui script could be found that matches with the call group in RemoveCallGroupUI()!");
            return;
        }

        // Remove the group from the internal list
        m_callerUIObjs.Remove(uiScript);

        // Destroy the UI's gameobject
        Destroy(uiScript.gameObject);
    }
}
