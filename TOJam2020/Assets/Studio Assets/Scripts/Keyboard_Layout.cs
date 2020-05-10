using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Keyboard_Layout", menuName = "Custom/Keyboard_Layout", order = 1)]
public class Keyboard_Layout : ScriptableObject
{
    //--- Public Variables ---//
    public KeyCode[] m_keyList = new KeyCode[26];



    //--- Private Variables ---//
    private Dictionary<KeyCode, int> m_keyMapping;



    //--- Methods ---//
    public int GetIndexFromKeyCode(KeyCode _key)
    {
        // If the key mappings haven't been set, do that now
        if (m_keyMapping == null)
            GenerateKeyMappings();

        // Return the index for the given key code
        return m_keyMapping[_key];
    }



    //--- Utility Methods ---//
    private void GenerateKeyMappings()
    {
        // Initialize the dictionary
        m_keyMapping = new Dictionary<KeyCode, int>();

        // Pass the keys into the dictionary with their given indices as the value
        // This way, we can get a button index from a character code
        for (int i = 0; i < m_keyList.Length; i++)
            m_keyMapping.Add(m_keyList[i], i);
    }
}
