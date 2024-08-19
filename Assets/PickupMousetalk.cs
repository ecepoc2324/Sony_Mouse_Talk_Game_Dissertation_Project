using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMousetalk : MonoBehaviour
{
    private bool isPickedUp = false;
    private Vector3 offset;
    private float mouseZCoord;

    void OnMouseDown()
    {
        // Store the mouse position and object offset
        mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // Move the object along with the mouse
        transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
