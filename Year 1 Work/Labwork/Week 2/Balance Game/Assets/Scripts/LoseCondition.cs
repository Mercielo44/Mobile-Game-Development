using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {Vector3 boxCoordinates = Camera.main.WorldToScreenPoint(transform.position);
        if (boxCoordinates.y <= 0.0f) {
            StartCoroutine (resetDelay());
        }
        
    }
    private IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
