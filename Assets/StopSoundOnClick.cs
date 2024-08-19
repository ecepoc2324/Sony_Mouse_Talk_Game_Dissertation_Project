using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class StopSoundOnClick : MonoBehaviour
{
    public GameObject audioSourceObject;
    public TextMeshProUGUI clickText;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (audioSourceObject != null)
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Audio Source Object is not assigned.");
        }

        if (clickText != null)
        {
            clickText.enabled = false; // Hide text initially
        }
        else
        {
            Debug.LogError("Click Text is not assigned.");
        }
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            if (clickText != null)
            {
                clickText.enabled = true; // Show text while audio is playing
            }
        }
        else
        {
            if (clickText != null)
            {
                clickText.enabled = false; // Hide text when audio stops
            }
        }
    }

    void OnMouseDown()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
