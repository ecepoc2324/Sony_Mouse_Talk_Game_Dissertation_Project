using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QACanvasManager : MonoBehaviour
{
   public void OnYesButtonClicked()
    {
        // Load the Yes end scene
        SceneManager.LoadScene("EndSceneYes"); // Replace with the correct scene name
    }

    public void OnNoButtonClicked()
    {
        // Load the No end scene
        SceneManager.LoadScene("EndSceneNo"); // Replace with the correct scene name
    }
}
