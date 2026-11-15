using Unity.VisualScripting;
using UnityEngine;

public class ObstacleDespawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            Destroy(other.transform.parent.gameObject);
        }
    }
}
