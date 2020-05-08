using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class callerLog_Scroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       this.GetComponent<Scrollbar>().value += (Input.mouseScrollDelta.y)/100.0f;
       //Debug.Log(Input.mouseScrollDelta.y);
    }
}
