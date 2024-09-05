using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public GameObject cover;
    public Transform cameraTargetPosition;
    public Vector3 cameraTargetRotation;
    public float moveDuration = 1.0f;
    public float rotationDuration = 0.5f;
    public AudioSource audioSource; // Reference to the AudioSource
    public QAManager qaManager; // Reference to the QAManager

    public SubtitleManager subtitleManager; // Reference to the SubtitleManager

    private bool isCoverOpen = false;
    private bool isAtCamera = false;
    private bool isAnimating = false;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Quaternion initialCoverRotation;
    private Quaternion openCoverRotation;

    private Rigidbody rb;
    

    void Start()
    {
       initialCoverRotation = cover.transform.localRotation;
        openCoverRotation = initialCoverRotation * Quaternion.Euler(180, 0, 0); // Rotation axis degrees for the cover

        //Get Rigidbody component and set it to Kinematic

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        //Hide the QA canvas

        if (qaManager != null)
        {
            qaManager.HideQACanvas();
        }
        else
        {
            Debug.LogError("QAManager is not assigned!");
        } 
    }

    //Mouse movement

    void OnMouseDown()
    {
        if (!isAnimating)
        {
            if (!isAtCamera) // The mouse is at camera position
            {
                savedPosition = transform.position; 
                savedRotation = transform.rotation;
                StartCoroutine(MoveAndRotateToCamera()); // Rotates from the original position to the camera position
            }
            else
            {
                StartCoroutine(ReturnToSavedPosition()); // Returns to original position
            }
        }
    }

    //Moving and rotation animation of the mouse to the camera

    IEnumerator MoveAndRotateToCamera()
    {
        isAnimating = true;
        float elapsedTime = 0; // Indicates how long after the animation will start
        Vector3 startingPos = transform.position; // Stores starting position variables to use for transition
        Quaternion startingRot = transform.rotation; // Stores variables for rotation from current position to target position
        Quaternion targetRot = Quaternion.Euler(cameraTargetRotation); // Object's rotation

        while (elapsedTime < moveDuration) // Creates an animation loop
        {
            transform.position = Vector3.Lerp(startingPos, cameraTargetPosition.position, elapsedTime / moveDuration); // Linear movement of the game object from starting position to target position
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, elapsedTime / moveDuration); // Smooth rotation from the starting position to the target position
            elapsedTime += Time.deltaTime; // Progress of animation frame by frame
            yield return null; // Pauses coroutine until the next frame
        }

        transform.position = cameraTargetPosition.position; // Setting object position to camera target position
        transform.rotation = targetRot;

        StartCoroutine(RotateCover(openCoverRotation, rotationDuration)); // Rotation of the cover of the mouse
        audioSource.Play(); // The audio plays when the cover opens
        isAtCamera = true;
        isCoverOpen = true;
        isAnimating = false;

        // Trigger the delayed canvas show
        Invoke("ShowQACanvasAfterDelay", 3.0f);
    }

    // The mouse returns to the original position

    IEnumerator ReturnToSavedPosition()
    {
        isAnimating = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        StartCoroutine(RotateCover(initialCoverRotation, rotationDuration)); // Rotating the cover after the mouse reaches to the camera
        audioSource.Stop();
        yield return new WaitForSeconds(rotationDuration);

        if (qaManager != null)
        {
            qaManager.HideQACanvas();
        }

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPos, savedPosition, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Lerp(startingRot, savedRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = savedPosition;
        transform.rotation = savedRotation;

        isAtCamera = false;
        isCoverOpen = false;
        isAnimating = false;
    }

    // Rotating the cover

    IEnumerator RotateCover(Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0; // The time passed
        Quaternion startingRot = cover.transform.localRotation; // Starting point of rotation animation

        while (elapsedTime < duration)
        {
            cover.transform.localRotation = Quaternion.Lerp(startingRot, targetRotation, elapsedTime / duration); // Rotation from the starting position to target 
            elapsedTime += Time.deltaTime;
            yield return null; // Pausing the coroutine
        }

        cover.transform.localRotation = targetRotation;

        Debug.Log("RotateCover - Completed");

        if (!isCoverOpen)
        {
            // Showing subtitle when the cover is fully opened
            subtitleManager.ShowSubtitle("Hey, can you open the file I sent you?");
        }
    }

    //Displaying QA Canvas after 3 seconds delay

    void ShowQACanvasAfterDelay()
    {
        if (qaManager != null && isCoverOpen)
        {
            Debug.Log("3 seconds passed, showing Q&A Canvas");
            qaManager.ShowQACanvas();
        }
    }
}
