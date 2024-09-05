using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Audio Object", menuName = "Assets/New Audio Object")]

public class AudioObject : ScriptableObject
{
    public AudioClip clip; // Reference to audio file
    public string subtitle; // For subtitle
}
