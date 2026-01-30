using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    private Dictionary<string, AudioSource> sourceDictionary;

    public AudioMixer audioMixer;
    public List<AudioSource> musicSources;

    private const string MusicVolumeParam = "MusicVolume";

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("MusicManager is not initialized. Ensure it exists in the scene.");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (this != _instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        sourceDictionary = new Dictionary<string, AudioSource>();
        foreach (var source in musicSources)
        {
            if (!sourceDictionary.ContainsKey(source.gameObject.name))
                sourceDictionary.Add(source.gameObject.name, source);
        }

        LoadState();
    }

    public void LoadState()
    {
        Globals.MusicActivated = PlayerPrefs.GetInt("MuteMusic", 1) == 1;
        if (Globals.MusicActivated)
            ActiveAll();
        else
            DesactiveAll();
    }

    public void SetActiveMusic(bool state)
    {
        Globals.MusicActivated = state;
        //PlayerPrefs.SetInt("IsMusicActive", Convert.ToInt32(Globals.MusicActivated));
        //PlayerPrefs.Save();

        if (state)
            ActiveAll();
        else
            DesactiveAll();
    }

    public void PlayMusicByName(string name, bool isLoop = false, float volume = 1.0f)
    {
        if (!Globals.MusicActivated) return;

        if (sourceDictionary.TryGetValue(name, out var source))
        {
            if (!source.isPlaying)
            {
                source.loop = isLoop;
                source.volume = volume;
                source.Play();
            }
        }
        else
            Debug.LogWarning($"Music with name {name} not found.");
    }

    public void ActiveAll()
    {
        Globals.MusicActivated = true;
        SetMixerVolume(0f);
    }

    public void DesactiveAll()
    {
        Globals.MusicActivated = false;
        SetMixerVolume(-80f);
    }

    public void SetMixerVolume(float volume)
    {
        audioMixer.SetFloat(MusicVolumeParam, volume);
    }

    public void StopAll()
    {
        foreach (var source in musicSources)
        {
            if (source.isPlaying)
                source.Stop();
        }
    }

    public void Conclude()
    {

    }
}
