using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{

    private static Audio_Manager m_instance;

    //--- Variable to hold to sources and the clips ---//
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;
    

    // Start is called before the first frame update
    private void Awake()
    {
        //Make it persistent, load from scene to scene
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //play music
        PlaySound();
    }

    //Get the audio clip that you want
    public AudioClip GetAudioClip (int clipIndex)
    {

        return audioClips[clipIndex];
    }

    //Plays a sound, right not just used for the music
    public void PlaySound()
    {
        audioSources[0].Play();

    }

    //This is to play a oneshot gets index of clip you want and the volume you want to play it at
    public void PlayOneShot(int clipIndex, float volume)
    {
        audioSources[1].PlayOneShot(GetAudioClip(clipIndex), volume);
    }
}
