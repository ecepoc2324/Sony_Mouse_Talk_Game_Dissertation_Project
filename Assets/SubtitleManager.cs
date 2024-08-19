using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText; // Reference to the Text or TextMeshPro component
    public float subtitleDuration = 5.0f; // Duration the subtitle is displayed

    void Start()
    {
        subtitleText.text = "Test Subtitle";
        subtitleText.text = ""; // Make sure the subtitle is initially empty
    }

    public void ShowSubtitle(string message)
    {
        Debug.Log("ShowSubtitle called with message: " + message);
        StartCoroutine(DisplaySubtitle(message));
    }

    private IEnumerator DisplaySubtitle(string message)
    {
        subtitleText.text = message;
        yield return new WaitForSeconds(subtitleDuration);
        subtitleText.text = ""; // Clear the subtitle after the duration
    }
}
