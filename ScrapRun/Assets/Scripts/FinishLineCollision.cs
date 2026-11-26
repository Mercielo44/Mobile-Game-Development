using System.Collections;
using UnityEngine;

public class FinishLineCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Finish");
            StartCoroutine(delay(1.5f));
        }
    }

    IEnumerator delay(float delayInSeconds) {
        yield return new WaitForSeconds(delayInSeconds);
        GameManager.gameManager.switchCameras(GameManager.gameManager.playerCamera, GameManager.gameManager.upgradeMenuCamera);
    }
}
