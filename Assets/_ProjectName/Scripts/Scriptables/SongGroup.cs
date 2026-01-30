using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongGroup", menuName = "SFX/SongGroup")]
public class SongGroup : ScriptableObject
{
    public string groupName;
    public List<AudioClip> instruments;
    public List<SongPartition> partitions;
}

[Serializable]
public class SongPartition
{
    public int id;
    public List<AudioClip> instruments;
    public List<Track> tracks;
}

[Serializable]
public class Track
{
    public AudioClip instrument;
    public float dB;
}
