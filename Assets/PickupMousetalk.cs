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
        // Storing the mouse position and object offset
        mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; // Changing object's world position into screen coordinates
        offset = gameObject.transform.position - GetMouseWorldPos(); // Distance between object and mouse
    }

    void OnMouseDrag()
    {
        // Moving the object along with the mouse
        transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        // Getting the mouse position in world coordinates
        Vector3 mousePoint = Input.mousePosition; // Current position of the mouse
        mousePoint.z = mouseZCoord; // Ensuring mouse position stays at the same depth

        return Camera.main.ScreenToWorldPoint(mousePoint); // Changing 2D screen coordinates into 3D world position
    }
}
