using UnityEngine;

public class DynamicCable : MonoBehaviour
{
    public Transform startObject; // The first object the cable is connected to
    public Transform endObject;   // The second object the cable is connected to

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (lineRenderer != null && startObject != null && endObject != null)
        {
            // Update the positions of the Line Renderer to connect the two objects
            lineRenderer.SetPosition(0, startObject.position);
            lineRenderer.SetPosition(1, endObject.position);
        }
    }
}
