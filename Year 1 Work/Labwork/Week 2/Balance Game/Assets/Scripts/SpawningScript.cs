using System.Drawing;
using UnityEditor;
using UnityEngine;

public class SpawningScript : MonoBehaviour
{
    public GameObject SpawnObject0;
    public GameObject SpawnObject1;
    public GameObject SpawnObject2;
    public int score;
    private Camera cam;
    Vector2 mousePos = new Vector2();
    Vector3 point = new Vector3();
    Event currentEvent = Event.current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {score = 0;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));
        point.z = 0.0f;
        int prefabValue = Random.Range(0, 2);
        if (Input.GetMouseButtonDown(0))
        {
            score += 1;
            if (prefabValue == 0)
            {
                Instantiate(SpawnObject0, point, Quaternion.identity);
            }
            else if (prefabValue == 1)
            {
                Instantiate(SpawnObject1, point, Quaternion.identity);
            }
            else if (prefabValue == 2)
            {
                Instantiate(SpawnObject2, point, Quaternion.identity);
            }
        }


    }
    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }
}
