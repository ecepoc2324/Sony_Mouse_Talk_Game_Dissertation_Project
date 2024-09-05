using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousetalkInteraction : MonoBehaviour
{
   public GameObject cover;
    public Transform cameraTargetPosition;
    public Vector3 cameraTargetRotation; // Euler angles for the rotation when held to ear
    public float moveDuration = 1.0f; // Time it takes to move to/from the camera
    public float rotationDuration = 0.5f; // Time it takes to open/close the cover
    public AudioSource audioSource;

    public QAManager qaManager;
    

    private bool isCoverOpen = false;
    private bool isAtCamera = false;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Quaternion initialCoverRotation;
    private Quaternion openCoverRotation;
    private bool isAnimating = false;

    private Rigidbody rb;

    void Start()
    {
        // Store the initial cover rotation
        initialCoverRotation = cover.transform.localRotation;
        openCoverRotation = initialCoverRotation * Quaternion.Euler(180, 0, 0); // Rotation axis degrees for the cover

        // Get the Rigidbody component and set it to kinematic
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Hide the canvas 

        Debug.Log("MousetalkInteraction: Hiding QACanvas at Start");
        if (qaManager != null)
        {
            qaManager.HideQACanvas();
        }
        else
        {
            Debug.LogError("MousetalkInteraction: qaManager is not assigned!");
        }
    }

    void OnMouseDown()
    {
        if (!isAnimating)
        {
            if (!isAtCamera)
            {
                savedPosition = transform.position;
                savedRotation = transform.rotation;
                StartCoroutine(MoveAndRotateToCamera());
            }
            else
            {
                StartCoroutine(ReturnToSavedPosition());
            }
        }
    }

    IEnumerator MoveAndRotateToCamera()
    {
        isAnimating = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(cameraTargetRotation);

        // Move and rotate to camera position
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPos, cameraTargetPosition.position, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = cameraTargetPosition.position;
        transform.rotation = targetRot;

        // Rotate the cover
        StartCoroutine(RotateCover(openCoverRotation, rotationDuration));
        audioSource.Play();
        isAtCamera = true;
        isCoverOpen = true;
        isAnimating = false;

        
    }

    IEnumerator ReturnToSavedPosition()
    {
        isAnimating = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        // Close the cover
        StartCoroutine(RotateCover(initialCoverRotation, rotationDuration));
        audioSource.Stop();
        yield return new WaitForSeconds(rotationDuration);

        if (qaManager != null)
        {
            qaManager.HideQACanvas();
        }
        else
        {
            Debug.LogError("MousetalkInteraction: qaManager is not assigned!");
        }

        

        // Move and rotate back to saved position
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

    IEnumerator RotateCover(Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0;
        Quaternion startingRot = cover.transform.localRotation;

        while (elapsedTime < duration)
        {
            cover.transform.localRotation = Quaternion.Lerp(startingRot, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cover.transform.localRotation = targetRotation;

        Debug.Log("MousetalkInteraction: RotateCover - Completed");
        Debug.Log($"MousetalkInteraction: isCoverOpen = {isCoverOpen}");

        if (!isCoverOpen)
        {
            Debug.Log("MousetalkInteraction: RotateCover - Cover opened, waiting 3 seconds to show Q&A Canvas");
            yield return new WaitForSeconds(3.0f);
            if (qaManager != null)
            {
                Debug.Log("MousetalkInteraction: 3 seconds passed, showing Q&A Canvas");
                qaManager.ShowQACanvas();
            }
            else
            {
                Debug.LogError("MousetalkInteraction: qaManager is not assigned!");
            }
        }
    }
}
