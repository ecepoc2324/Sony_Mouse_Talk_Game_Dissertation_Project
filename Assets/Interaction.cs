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
    public AudioSource audioSource;
    public QAManager qaManager;

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
        openCoverRotation = initialCoverRotation * Quaternion.Euler(180, 0, 0);

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (qaManager != null)
        {
            qaManager.HideQACanvas();
        }
        else
        {
            Debug.LogError("QAManager is not assigned!");
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

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPos, cameraTargetPosition.position, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = cameraTargetPosition.position;
        transform.rotation = targetRot;

        StartCoroutine(RotateCover(openCoverRotation, rotationDuration));
        audioSource.Play();
        isAtCamera = true;
        isCoverOpen = true;
        isAnimating = false;

        // Trigger the delayed canvas show
        Invoke("ShowQACanvasAfterDelay", 3.0f);
    }

    IEnumerator ReturnToSavedPosition()
    {
        isAnimating = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        StartCoroutine(RotateCover(initialCoverRotation, rotationDuration));
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

        Debug.Log("RotateCover - Completed");

        if (!isCoverOpen)
        {
            // Show subtitle when the cover is fully opened
            subtitleManager.ShowSubtitle("Hey, can you open the file I sent you?");
        }
    }

    void ShowQACanvasAfterDelay()
    {
        if (qaManager != null && isCoverOpen)
        {
            Debug.Log("3 seconds passed, showing Q&A Canvas");
            qaManager.ShowQACanvas();
        }
    }
}
