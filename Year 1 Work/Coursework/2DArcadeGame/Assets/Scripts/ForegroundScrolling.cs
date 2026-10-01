using UnityEngine;

public class ForegroundScrolling : MonoBehaviour
{
    private float scrollSpeed = 0.35f;
    private Material scrollMaterial;
    private Vector2 offset;
    GameObject gameManager;
    private GameManagerCode gameManagerCode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollMaterial = GetComponent<SpriteRenderer>().material;
        offset = new Vector2(0, 0);

        gameManager = GameObject.Find("Game Manager");
        gameManagerCode = gameManager.GetComponent<GameManagerCode>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += (Time.deltaTime* scrollSpeed);
        scrollMaterial.mainTextureOffset = offset;
        if (gameManagerCode.playerDead)
        {
            scrollSpeed = 0;
        }
    }
}
