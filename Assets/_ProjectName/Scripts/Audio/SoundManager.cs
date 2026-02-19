using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    [SerializeField] private Transform sourcesParent;
    [SerializeField] private Transform sfxSourcesParent;
    private static AudioSource[] sources;
    private static AudioSource[] sfxPoolSources;
    private static AudioSource[] musicSources;
    [SerializeField] private List<SFXGroup> sfxGroups;
    [SerializeField] private List<SongGroup> songGroups;
    private Dictionary<AudioClip, AudioSource> activeSongSources = new();
    [SerializeField] private Transform musicSourcesParent;
    private Dictionary<string, AudioSource> sourceDictionary;
    private Dictionary<string, AudioClip> clipDictionary;
    private Dictionary<string, SFXGroup> sfxGroupDictionary;
    private Dictionary<string, SongGroup> songGroupDictionary;
    private Dictionary<string, AudioSource> activeLoopSFX = new Dictionary<string, AudioSource>();

    [SerializeField] private List<AudioClip> preloadedClips;
    public AudioMixer audioMixer;
    public string[] mixerGrpupParams;

    private const float MuteVolume = -70f;
    private const float DefaultVolume = -1.76f;
    private string currentSong = "";
    private const float underWaterEffectOn = 500f;
    private const float underWaterEffectOff = 5000f;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SoundManager is not initialized. Ensure it exists in the scene.");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (this != _instance)
            Destroy(gameObject);
    }

    public void Initialize()
    {
        sfxPoolSources = sfxSourcesParent.GetComponentsInChildren<AudioSource>();
        musicSources = musicSourcesParent.GetComponentsInChildren<AudioSource>();
        InitializeSongGroups();
        InitializeClipDictionary();
        InitializeSFXGroups();
        InitializeSongGroups();
    }

    private void InitializeSourceDictionary()
    {
        sourceDictionary = new Dictionary<string, AudioSource>();
        foreach (var source in sources)
        {
            if (!sourceDictionary.ContainsKey(source.gameObject.name))
                sourceDictionary.Add(source.gameObject.name, source);
        }
    }

    private void InitializeClipDictionary()
    {
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in preloadedClips)
        {
            if (!clipDictionary.ContainsKey(clip.name))
                clipDictionary.Add(clip.name, clip);
        }
    }

    private void InitializeSFXGroups()
    {
        sfxGroupDictionary = new Dictionary<string, SFXGroup>();
        foreach (var group in sfxGroups)
        {
            if (group != null && !string.IsNullOrEmpty(group.groupName))
            {
                if (!sfxGroupDictionary.ContainsKey(group.groupName))
                    sfxGroupDictionary.Add(group.groupName, group);
            }
        }
    }
    private void InitializeSongGroups()
    {
        songGroupDictionary = new Dictionary<string, SongGroup>();
        foreach (var group in songGroups)
        {
            if (group != null && !string.IsNullOrEmpty(group.groupName))
            {
                if (!songGroupDictionary.ContainsKey(group.groupName))
                    songGroupDictionary.Add(group.groupName, group);
            }
        }
    }

    public void LoadState()
    {
        Globals.SFXActivated = PlayerPrefs.GetInt("MuteEffects", 1) == 1;
        if (Globals.SFXActivated)
            ActiveAll();
        else
            DesactiveAll();
    }

    public void SetActiveMusic(bool state)
    {
        Globals.SFXActivated = state;
        //PlayerPrefs.SetInt("IsSoundActive", Convert.ToInt32(Globals.SFXActivated));
        //PlayerPrefs.Save();
    }

    public void PlaySFXByName(string groupName, float volume = 1f, float minPitch = 1f, float maxPitch = 1f)
    {
        if (!Globals.SFXActivated) return;

        if (sfxGroupDictionary.TryGetValue(groupName, out var group))
        {
            if (group.clips.Count == 0) return;

            AudioSource source = GetAvailableSource();
            if (source != null)
            {
                var clip = group.clips[Random.Range(0, group.clips.Count)];
                source.clip = clip;
                source.loop = false;
                source.volume = volume;
                source.pitch = Random.Range(minPitch, maxPitch); ;

                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"SFX group '{groupName}' not found.");
        }
    }
    public void PlayLoopSFXByName(string groupName, float volume = 1f)
    {
        if (!Globals.SFXActivated) return;

        if (activeLoopSFX.ContainsKey(groupName))
            return;

        if (sfxGroupDictionary.TryGetValue(groupName, out var group))
        {
            if (group.clips.Count == 0) return;

            AudioSource source = GetAvailableSource();
            if (source == null) return;

            var clip = group.clips[Random.Range(0, group.clips.Count)];

            source.clip = clip;
            source.loop = true;
            source.volume = volume;
            source.Play();

            activeLoopSFX.Add(groupName, source);
        }
        else
        {
            Debug.LogWarning($"SFX group '{groupName}' not found.");
        }
    }
    public void StopLoopSFXByName(string groupName)
    {
        if (activeLoopSFX.TryGetValue(groupName, out var source))
        {
            source.Stop();
            source.loop = false;
            source.clip = null;

            activeLoopSFX.Remove(groupName);
        }
    }
    public void StopAllLoopSFX()
    {
        foreach (var kvp in activeLoopSFX)
        {
            kvp.Value.Stop();
            kvp.Value.loop = false;
            kvp.Value.clip = null;
        }

        activeLoopSFX.Clear();
    }
    public void PlaySFXByNameIndex(string groupName, int index, float volume = 1)
    {
        if (!Globals.SFXActivated) return;

        if (sfxGroupDictionary.TryGetValue(groupName, out var group))
        {
            if (group.clips.Count == 0) return;

            AudioSource source = GetAvailableSource();
            if (source != null)
            {
                var clip = group.clips[index];
                source.clip = clip;
                source.loop = false;
                source.volume = volume;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"SFX group '{groupName}' not found.");
        }
    }
    public void PlaySongByName(string groupName, int partitionId, float volume = 1, float fadeDuration = 1f)
    {
        if (!Globals.MusicActivated) return;

        if (!songGroupDictionary.TryGetValue(groupName, out var group))
        {
            Debug.LogWarning($"Song group '{groupName}' not found.");
            return;
        }

        if (currentSong != groupName)
        {
            currentSong = groupName;
            CrossfadeToNewSongGroup(group, fadeDuration);
        }

        var partition = group.partitions.Find(p => p.id == partitionId);
        if (partition == null)
        {
            Debug.LogWarning($"Partition ID {partitionId} not found in song '{groupName}'.");
            return;
        }

        foreach (var entry in activeSongSources)
        {
            float targetVolume = partition.instruments.Contains(entry.Key) ? volume : 0f;
            StartCoroutine(FadeVolume(entry.Value, targetVolume, fadeDuration));
        }
    }
    private void CrossfadeToNewSongGroup(SongGroup newGroup, float fadeDuration)
    {
        var previousSources = new Dictionary<AudioClip, AudioSource>(activeSongSources);
        activeSongSources.Clear();

        foreach (var clip in newGroup.instruments)
        {
            var source = GetAvailableMusicSource();
            if (source != null)
            {
                source.clip = clip;
                source.loop = true;
                source.volume = 0f;
                source.Play();
                activeSongSources[clip] = source;

                StartCoroutine(FadeVolume(source, 1f, fadeDuration));
            }
        }

        foreach (var kvp in previousSources)
        {
            StartCoroutine(FadeAndStop(kvp.Value, 0f, fadeDuration));
        }
    }
    private IEnumerator FadeAndStop(AudioSource source, float targetVolume, float duration)
    {
        float start = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVolume, elapsed / duration);
            yield return null;
        }

        source.volume = targetVolume;
        if (targetVolume <= 0f)
        {
            source.Stop();
            source.clip = null;
        }
    }
    private IEnumerator FadeVolume(AudioSource source, float target, float duration)
    {
        float start = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        source.volume = target;
    }
    private AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPoolSources)
        {
            if (!source.isPlaying)
                return source;
        }
        Debug.LogWarning("No available AudioSource found.");
        return null;
    }
    private AudioSource GetAvailableMusicSource()
    {
        foreach (var source in musicSources)
        {
            if (!source.isPlaying)
                return source;
        }
        Debug.LogWarning("No available AudioSource found for music.");
        return null;
    }

    public void PauseSoundByName(string name)
    {
        if (sourceDictionary.TryGetValue(name, out var source))
            source.Pause();
    }

    public void StopSoundByName(string name)
    {
        if (sourceDictionary.TryGetValue(name, out var source))
        {
            source.loop = false;
            source.Stop();
        }
    }

    public void ChangeSoundSpeedByName(string name, float speed)
    {
        if (sourceDictionary.TryGetValue(name, out var source))
            source.pitch = speed;
    }

    public void ChangeSoundVolumeByName(string name, float volume)
    {
        if (sourceDictionary.TryGetValue(name, out var source))
            source.volume = volume;
    }

    public void ActiveAll()
    {
        foreach (var source in sources)
        {
            source.mute = false;
        }
        SetMixerGroupVolume(false);
    }

    public void DesactiveAll()
    {
        foreach (var source in sources)
        {
            source.mute = true;
        }
        SetMixerGroupVolume(true);
    }

    private void SetMixerGroupVolume(bool mute)
    {
        float targetVolume = mute ? MuteVolume : DefaultVolume;
        foreach (var param in mixerGrpupParams)
        {
            audioMixer.SetFloat(param, targetVolume);
        }
    }

    public void SetMixerSFXVolume(float volume)
    {
        float value = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(Constants.AUDIO_MIXER_SFX_VOLUME, dB);
    }
    public void SetMixerMusicVolume(float volume)
    {
        float value = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(Constants.AUDIO_MIXER_MUSIC_VOLUME, dB);
    }
    public void SetUnderWaterEffect(bool active)
    {
        float db = active ? underWaterEffectOn : underWaterEffectOff;
        audioMixer.SetFloat(Constants.AUDIO_MIXER_UNDERWATER_EFFECT, db);
    }
    public void SetMuteToAll(bool state)
    {
        foreach (var source in sources)
        {
            source.mute = state;
        }
    }

    public void StopAll()
    {
        foreach (var source in sources)
        {
            source.loop = false;
            source.Stop();
        }
    }
}