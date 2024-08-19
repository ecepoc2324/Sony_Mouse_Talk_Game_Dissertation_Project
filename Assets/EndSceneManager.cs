using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public void TryAgain()
    {
        // Reload the first scene (assuming the first scene is the main game)
        SceneManager.LoadScene(0); // Alternatively, you can use the scene name
    }
}
