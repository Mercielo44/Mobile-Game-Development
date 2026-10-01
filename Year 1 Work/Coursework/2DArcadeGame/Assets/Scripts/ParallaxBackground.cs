using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Vector3 cameraStartPos;
    public Vector3 StartPos;
    public float scrollAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.position;
        cameraStartPos = transform.parent.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMotion = (transform.parent.position - cameraStartPos);
        cameraMotion.y = 0f;
        transform.position = StartPos + scrollAmount * cameraMotion;
    }
}
