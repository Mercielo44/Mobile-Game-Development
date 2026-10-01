using Unity.VisualScripting;
using UnityEngine;

public class PelletCode : MonoBehaviour
{
    GameObject gameManager;
    private GameManagerCode gameManagerCode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        gameManagerCode = gameManager.GetComponent <GameManagerCode> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManagerCode.score += 5;
        int scoreLength = gameManagerCode.score.ToString().Length;
        gameManagerCode.scoreText.text = "Score: ";
        for (int i = 0; i < (8-scoreLength); i++) 
        {
            gameManagerCode.scoreText.text += "0";
        }
        gameManagerCode.scoreText.text += gameManagerCode.score;
        Destroy(gameObject);
    }
}
