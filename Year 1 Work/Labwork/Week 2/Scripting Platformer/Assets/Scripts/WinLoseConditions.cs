using UnityEngine;

public class WinLoseConditions : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public float loseTime = 30.0f;
    public float winHeight = 4.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        loseTime -= Time.deltaTime;     
        if (transform.position.y >= winHeight && loseTime > 0.0f)
        {
            winScreen.SetActive(true);
        }
        else if (loseTime <= 0.0f)
        {
            loseScreen.SetActive(true);
        }
        else if (transform.position.y < winHeight && loseTime > 0.0f)
        {  
            winScreen.SetActive(false);
        }
    }
}
