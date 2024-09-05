using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class StopSoundOnClick : MonoBehaviour
{
    public GameObject audioSourceObject;
    public TextMeshProUGUI clickText;

    private AudioSource audioSource; // Reference to AudioSource 
    // Start is called before the first frame update
    void Start()
    {
        if (audioSourceObject != null) // Checking if audio source object is assigned
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>(); // Assigning to audioSource
        }
        else
        {
            Debug.LogError("Audio Source Object is not assigned.");
        }

        if (clickText != null)
        {
            clickText.enabled = false; // Hiding text initially
        }
        else
        {
            Debug.LogError("Click Text is not assigned.");
        }
    }

    // Showing and hiding text

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            if (clickText != null)
            {
                clickText.enabled = true; // Showing text while audio is playing
            }
        }
        else
        {
            if (clickText != null)
            {
                clickText.enabled = false; // Hiding text when audio stops
            }
        }
    }

    void OnMouseDown()
    {
        if (audioSource != null && audioSource.isPlaying) // Checking if the audio is playing
        {
            audioSource.Stop();
        }
    }
}
