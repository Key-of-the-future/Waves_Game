using UnityEngine;

public class PistolLine : MonoBehaviour
{
    public Transform lineOrigin;
    public LineRenderer lineRenderer;
    public float distance = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, lineOrigin.position);
        Vector3 endPosition = lineOrigin.position + lineOrigin.forward * distance;
        lineRenderer.SetPosition(1, endPosition);
    }
}
