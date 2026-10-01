using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int horizonSteps;
    float xDirection = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StepBasedMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StepBasedMovement()
    {
        while (true)
        {
            for (int i = 0; i < horizonSteps; i++)
            {
                yield return new WaitForSeconds(1);
                transform.position += new Vector3(xDirection, 0, 0);
            }
            yield return new WaitForSeconds(1);
            transform.position += new Vector3(0, -0.5f, 0);
            xDirection *= -1;
            
            }
        

    }
}
