using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxScript : MonoBehaviour
{
    AudioSource controller;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AudioSource>();
        controller.loop = true;
    }
    IEnumerator FadeCoroutine(bool set)
    {
        if (set)
        {
            controller.volume = 0;
        }

        for (float i = 0; i < 1; i+=0.01f)
        {
            yield return new WaitForSeconds(0.1f);
            if (set)
            { 
                controller.volume = i; 
            }
            else
            {
                controller.volume -= i;
            }
                
        }

        if (set)
        {
            controller.Play();
        }
        else
        {
            controller.Stop();
        }
        StopCoroutine(FadeCoroutine(set));

    }
    public void playMusic(AudioClip audio)
    {
        controller.clip = audio;
        StartCoroutine(FadeCoroutine(true));
    }
    public void StopPlaying()
    {
        StartCoroutine(FadeCoroutine(false));
    }
    public AudioClip GetCurrentAudio()
    {
        return controller.clip;
    }
}
