using UnityEngine;
using System.Collections;

public class CallerLog_UIManager : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform m_uiListParent;
    public GameObject m_uiElementPrefab;



    //--- Private Variables ---//
    //private List<Caller_UI> m_callerUIObjs;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        //m_callerUIObjs = new List<Caller_UI>();
    }



    //--- Methods ---//
    public void AddCallGroupUI(Call_Group _group)
    {
        // TODO: Instantiate the prefab as a child of the list parent

        // TODO: Grab the script off the prefab and add it to the internal list

        // TODO: Pass the group to the script so that it can get set up
    }

    public void RemoveCallGroupUI(Call_Group _group)
    {
        // TODO: Find the UI element that is connected to the group

        // TODO: Destroy the UI object

        // TODO: Remove the group from the internal list
    }
}
