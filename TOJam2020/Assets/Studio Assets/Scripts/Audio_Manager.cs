using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{

    private static Audio_Manager m_instance;

    public AudioClip[] audioClips;
    

    // Start is called before the first frame update
    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        this.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetAudioClip (int clipIndex)
    {

        return audioClips[clipIndex];
    }

    public void PlaySound()
    {
        this.GetComponent<AudioSource>().Play();
    }


    public void PlayOneShot(int clipIndex, float volume)
    {
        this.GetComponent<AudioSource>().PlayOneShot(GetAudioClip(clipIndex), volume);
    }
}
