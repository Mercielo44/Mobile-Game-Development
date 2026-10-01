using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    private int currentScore;
    public int winningScore;
    float loseTime = 20.0f;
    public GameObject winScreen;
    public GameObject loseScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

        loseTime -= Time.deltaTime;
        if (currentScore >= winningScore && loseTime > 0.0f)
        {
            winScreen.SetActive(true);
        }
        else if (currentScore < winningScore && loseTime <= 0.0f)
        {
            loseScreen.SetActive(true);
        }
        else if (currentScore < winningScore && loseTime > 0.0f)
        {
            winScreen.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Corn"))
        {
            Destroy(other.gameObject);
            currentScore += 1;
        }
    }
}
