using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlatformMovementSpeed : MonoBehaviour
{
    public bool floor;
    public float speed = 8;
    Vector3 currentPos;
    float timeTracker;

    GameObject gameManager;
    private GameManagerCode gameManagerCode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPos = transform.position;

        gameManager = GameObject.Find("Game Manager");
        gameManagerCode = gameManager.GetComponent<GameManagerCode>();
    }

    // Update is called once per frame
    void Update() 
    {
        currentPos.x = currentPos.x - (speed * Time.deltaTime);
        transform.position = currentPos;


        if (transform.position.x < 0.001f && floor)
        {
            StartCoroutine(FloorPause(speed));
            floor = false;
        }
        if (transform.position.x < -50)
        {
            Destroy(gameObject);
        }
        if (gameManagerCode.playerDead)
        {
            speed = 0;
        }
        timeTracker += Time.deltaTime;
    }


    private IEnumerator FloorPause(float saveSpeed)
    {
        speed = 0;
        yield return new WaitForSeconds(16f - timeTracker);
        timeTracker = 0;
        speed = saveSpeed;

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (gameObject.tag == "Collidable")
    //    {
    //        gameManagerCode.playerDead = true;
    //        speed = 0;
    //        gameManagerCode.save();
    //        gameManagerCode.StartCoroutine(gameManagerCode.LoadGameoverScreen());
    //    }
    //}
}
