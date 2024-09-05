using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QACanvasManager : MonoBehaviour
{
   public void OnYesButtonClicked()
    {
        // Loading the Yes end scene
        SceneManager.LoadScene("EndSceneYes"); 
    }

    public void OnNoButtonClicked()
    {
        // Loading the No end scene
        SceneManager.LoadScene("EndSceneNo"); 
    }
}
