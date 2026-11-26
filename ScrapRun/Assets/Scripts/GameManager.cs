using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject[] segments;
    public GameObject finishLine;
    public GameObject playerCamera;
    public GameObject upgradeMenuCamera;
    int segementNum;
    long checkPointCount;
    long score;
    int playerSpeed;
    public bool hitCheckPoint;

    int zPos = 900;

    void Awake()
    {
        if (gameManager)
        {
            Destroy(gameObject);
            return;
        }
        gameManager = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpeed = 100;
        for (int i = 0; i < 3; i++) {
            spawnObstacle(200+(250*i));
        }
        StartCoroutine(SegmentGen());

        finishLine.transform.position = new Vector3 (0,0,playerSpeed * 240/*(Random.Range(110, 170))*/);
        print(finishLine.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SegmentGen()
    {
        while (GameManager.gameManager.hitCheckPoint == false)
        {
            spawnObstacle(HovercarController.instance.transform.position.z + zPos);
            yield return new WaitForSeconds(3);
        }
    }

    void spawnObstacle(float zOffest) {
        segementNum = Random.Range(0, segments.Length);
        Instantiate(segments[segementNum], new Vector3(segments[segementNum].transform.position.x, segments[segementNum].transform.position.y, zOffest), Quaternion.identity);
    }

    public void switchCameras(GameObject camera1, GameObject camera2)
    {
        camera1.SetActive(false);
        camera1.GetComponent<AudioListener>().enabled = false;
        camera2.SetActive(true);
        camera2.GetComponent<AudioListener>().enabled = true;
    }
}
