using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScript : MonoBehaviour
{
    public Vector3 SpawnPosition = new Vector3(-5, 4, 0);
    public GameObject[] EnemyType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int y = 0; y < EnemyType.Length; y++)
            for (int i = 0; i < 5; i++)
            {
                Instantiate(EnemyType[Random.Range(0,4)], new Vector3(SpawnPosition.x+i, SpawnPosition.y-y, SpawnPosition.z), Quaternion.identity );
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
