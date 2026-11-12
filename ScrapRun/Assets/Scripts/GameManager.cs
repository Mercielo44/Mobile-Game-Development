using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] segments;
    int segementNum;

    int zPos = 900;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 3; i++) {
            spawnObstacle(200+(250*i));
        }
        StartCoroutine(SegmentGen());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SegmentGen()
    {
        while (true)
        {
            spawnObstacle(HovercarController.instance.transform.position.z + zPos);
            yield return new WaitForSeconds(3);
        }
    }

    void spawnObstacle(float zOffest) {
        segementNum = Random.Range(0, segments.Length);
        Instantiate(segments[segementNum], new Vector3(segments[segementNum].transform.position.x, segments[segementNum].transform.position.y, zOffest), Quaternion.identity);
    }

}
