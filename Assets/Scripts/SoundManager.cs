/*AUTHOR: EYAL*/
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //floats used to record the volume levels
    public float masterVolume, effectsVolume, songsVolume;

    //arrays of the AudioSources to be played during the game
    public AudioSource[] effectAudioSources, songAudioSources;
    private Dictionary<string,AudioSource> effects = new Dictionary<string, AudioSource>(), songs = new Dictionary<string, AudioSource>();

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
        foreach(var a in songAudioSources)
        {
            songs.Add(a.name,a);
        }
    }
    
    //play the specified effect.
    public void playEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            AudioSource sourceToPlay = effects[name];
            sourceToPlay.volume = effectsVolume;
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

    //play the specified song
    public void playSong(string name)
    {
        //Debug.Log("trying to play song " + name);
        if(songs.ContainsKey(name))
        {
            AudioSource sourceToPlay = songs[name];
            sourceToPlay.volume = songsVolume;
            sourceToPlay.Play();
            ///Debug.Log("just played song " + sourceToPlay.name);
        }
    }

    //constantly update the master and songs volume. Effects volume is not updated since effects are transient and fleeting.
    private void Update()
    {
        AudioListener.volume = masterVolume;
        foreach(var a in songAudioSources)
        {
            if(a.isPlaying)
            {
                a.volume = songsVolume;
                //Debug.Log("song " + a.name + " is playing at volume " + a.volume.ToString());
            }
            else
            {
                //Debug.Log("song " + a.name + " is not playing");
            }
        }
    }
}