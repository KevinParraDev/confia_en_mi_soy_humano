using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXGroup", menuName = "SFX/SFXGroup")]
public class SFXGroup : ScriptableObject
{
    public string groupName;
    public List<AudioClip> clips;
}
