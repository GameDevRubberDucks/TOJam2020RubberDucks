using UnityEngine;
using System.Collections.Generic;

public class Room_UI_Layout : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject m_portraitPrefab;
    public GameObject m_layoutParent;
    public Transform[] m_portraitHolders;



    //--- Private Variables ---//
    private List<Call_Individual_UI> m_uiObjs;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_uiObjs = new List<Call_Individual_UI>();
    }



    //--- Methods ---//
    public void ClearAllPortraits()
    {
        // Loop through the portrait holders and delete their children (the portraits)
        foreach (var portraitHolder in m_portraitHolders)
            DestroyAllChildren(portraitHolder);

        // Clear the list of UI objs
        m_uiObjs.Clear();
    }

    public void PlacePortraits(List<Call_Individual> _callers)
    {
        // Start by clearing all of the existing portraits
        ClearAllPortraits();

        // Reset the list of UI objects
        m_uiObjs = new List<Call_Individual_UI>();

        // Loop through the portrait holders and spawn new portraits based on the given callers
        for (int i = 0; i < _callers.Count; i++)
        {
            // Spawn a new portrait into the slot
            GameObject newPortrait = Instantiate(m_portraitPrefab, m_portraitHolders[i]);

            // Grab the UI script and get it setup then store the ref to it
            Call_Individual_UI uiComp = newPortrait.GetComponent<Call_Individual_UI>();
            uiComp.InitWithData(_callers[i]);
            m_uiObjs.Add(uiComp);
        }
    }

    public void SetLayoutVisible(bool _visible)
    {
        // Toggle the layout
        m_layoutParent.SetActive(true);
    }



    //--- Getters and Setters ---//
    public int PortraitSlotCount
    {
        get => m_portraitHolders.Length;
    }



    //--- Utility Methods ---//
    public void DestroyAllChildren(Transform _parent)
    {
        // Iterate through and remove all of this parent's children
        for (int i = 0; i < _parent.childCount; i++)
            Destroy(_parent.GetChild(i).gameObject);
    }
}
