using UnityEngine.Events;

//--- Helper Enums ---//
public enum Call_State
{
    Waiting,                    // ANY of the call participants are disconnected
    Active,                     // ALL of the call participants are connected TOGETHER
    Waited_Too_Long,            // The call terminated because the participants waited too long
    Completed                   // The call finished because the group was done their call
}



//--- Helper Events ---//
public class Call_CompletionEvent : UnityEvent<Call_Group, Call_State> { }