using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] segments;

    int zPos = 150;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SegmentGen());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SegmentGen()
    {
        Instantiate(segments[Random.Range(0, segments.Length)], new Vector3(0,0,player.transform.position.z+zPos), Quaternion.identity);
        yield return new WaitForSeconds(5);
    }

}
