/*AUTHOR: EYAL*/
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    //arrays of the AudioSources to be played during the game
    public AudioSource[] effectAudioSources;
    private Dictionary<string,AudioSource> effects = new Dictionary<string, AudioSource>();

    //singleton
    public static SoundManager Instance { get; private set; }

    private void Awake() 
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //store the AudioSources in dictionaries, with their names as the corresponding keys. This should make looking them up faster.
    private void Start()
    {
        //Debug.Log("In sound manager Start");
        foreach(var a in effectAudioSources)
        {
            effects.Add(a.name,a);
        }
    }
    
    //play the specified effect.
    public void playEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            AudioSource sourceToPlay = effects[name];
            sourceToPlay.Play();
        }

        Debug.Log("just played effect " + name);
    }

    public void stopEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            AudioSource sourceToStop = effects[name];
            sourceToStop.Stop();
        }
    }

    public void stopAllAudio()
    {
        foreach(var a in effectAudioSources)
        {
            a.Stop();
        }
    }
}