using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QAManager : MonoBehaviour
{

    public Canvas QACanvas;

    void Start()
    {
        Debug.Log("QAManager: Start - Hiding QACanvas");
        HideQACanvas(); // Ensure the canvas is hidden at the start
    }

    public void ShowQACanvas()
    {
        Debug.Log("QAManager: ShowQACanvas - Activating QACanvas");
        QACanvas.gameObject.SetActive(true);
    }

    public void HideQACanvas()
    {
        Debug.Log("QAManager: HideQACanvas");
        QACanvas.gameObject.SetActive(false);
    }
}
