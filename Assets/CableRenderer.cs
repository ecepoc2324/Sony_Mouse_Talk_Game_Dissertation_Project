using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRenderer : MonoBehaviour
{
     public Transform cableStart;
    public Transform cableEnd;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
      lineRenderer = GetComponent<LineRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
      if (lineRenderer != null && cableStart != null && cableEnd != null)
        {
            lineRenderer.SetPosition(0, cableStart.position);
            lineRenderer.SetPosition(1, cableEnd.position);
        }  
    }
}
