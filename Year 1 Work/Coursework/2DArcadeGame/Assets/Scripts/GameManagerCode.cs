using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCode : MonoBehaviour
{
    public long score = 0;
    public List<TextMeshProUGUI> scoreView;
    public TextMeshProUGUI scoreText;
    public Canvas gameplayUI;
    public Canvas gameOverUI;
    public GameObject gameOverUIBackground;

    class scoreBoard
    {
        public long highScoreStore;
        public List<long> scoreStore = new List<long>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //public long scoreStore1;
        //public long scoreStore2;
        //public long scoreStore3;
        //public long scoreStore4;
        //public long scoreStore5;
        //public long scoreStore6;
        //public long scoreStore7;
        //public long scoreStore8;
        //public long scoreStore9;
        //public long scoreStore10;
    }

    public List<GameObject> levelProps;
    public List<GameObject> floorsizes;

    public GameObject spawnPosition;
    Vector3 propSpawnPositionPositive;
    Vector3 propSpawnPositionNegative;
    Vector3 floorSpawnPosition;

    public bool playerDead = false;
    public bool canSpawn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        gameplayUI.enabled = true;
        gameOverUI.enabled = false;
        propSpawnPositionNegative = new Vector3 (spawnPosition.transform.position.x, spawnPosition.transform.position.y-3.5f, spawnPosition.transform.position.z);
        propSpawnPositionPositive = new Vector3(spawnPosition.transform.position.x, spawnPosition.transform.position.y + 3.5f, spawnPosition.transform.position.z);
        floorSpawnPosition = new Vector3(propSpawnPositionNegative.x + 20, propSpawnPositionNegative.y, propSpawnPositionNegative.z);
        StartCoroutine(LevelGenerator(false, 0));
        StartCoroutine(LevelGenerator(true, 2));
        StartCoroutine(FloorGenerator(false, 2f));
        StartCoroutine(FloorGenerator(true, 4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LevelGenerator(bool upsideDown, int initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        GameObject levelPropSelected;
        while (!playerDead)
        {
            levelPropSelected = levelProps[Random.Range(0, levelProps.Count)];
            if (upsideDown)
            {
                Instantiate(levelPropSelected, propSpawnPositionPositive, new Quaternion(0, 0, 180, 0));
            }
            else
            {
                Instantiate(levelPropSelected, propSpawnPositionNegative, new Quaternion(0, 0, 0, 0));
            }
            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator FloorGenerator(bool upsideDown, float initialDelay) 
    {
        yield return new WaitForSeconds(initialDelay);
        int currentHeight = 0;
        int previousHeight = 0;
        float ySpawn;
        int spawnAngle;
        if (upsideDown)
        {
            ySpawn = 3.5f;
            spawnAngle = 180;
        }
        else 
        {
            ySpawn = -3.5f;
            spawnAngle = 0;
        }
        yield return new WaitForSeconds(16);
        

        while (!playerDead)
        {
            if (canSpawn)
            {
                previousHeight = currentHeight;
                currentHeight = Random.Range(0, 3);
                if (currentHeight == 2 & previousHeight == 0)
                {
                    if (upsideDown)
                    {
                        Instantiate(floorsizes[3], new Vector3(39.5f, ySpawn, -10), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(floorsizes[2], new Vector3(39.5f, ySpawn, -10), Quaternion.identity);
                    }

                }
                else if (currentHeight == 2 && previousHeight == 1 || currentHeight == 2 & previousHeight == 2)
                {
                    Instantiate(floorsizes[1], new Vector3(22, ySpawn, -10), new Quaternion(0, 0, spawnAngle, 0));
                }
                else if (currentHeight == 1)
                {
                    Instantiate(floorsizes[0], new Vector3(22, ySpawn, -10), new Quaternion(0, 0, spawnAngle, 0));
                }

                if (upsideDown)
                {
                    propSpawnPositionPositive.y = 3.5f - currentHeight;
                }
                else
                {
                    propSpawnPositionNegative.y = -3.5f + currentHeight;
                }
                yield return new WaitForSeconds(16);

            }
        }
    }

    public void saveGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        scoreBoard toSave = new scoreBoard();

        if (File.Exists(path)){
            print("Helloo");
            scoreBoard loadedData = JsonUtility.FromJson<scoreBoard>(File.ReadAllText(path));
            for (int i = loadedData.scoreStore.Count - 1; i > 0; i--) 
            { 
                toSave.scoreStore[i] = loadedData.scoreStore[i-1]; 
            }

            if (score > loadedData.highScoreStore)
            {
                toSave.highScoreStore = score;
            }
            else
            {
                toSave.highScoreStore = loadedData.highScoreStore;
            }
        }
        //else
        /*{
            for (int i = loadedData.scoreStore.Count - 1; i > 0; i--)
            {
                toSave.scoreStore[i] = toSave.scoreStore[i - 1];
            }
        }*/
            //toSave.scoreStore10 = loadedData.scoreStore9;
            //toSave.scoreStore9 = loadedData.scoreStore8;
            //toSave.scoreStore8 = loadedData.scoreStore7;
            //toSave.scoreStore7 = loadedData.scoreStore6;
            //toSave.scoreStore6 = loadedData.scoreStore5;
            //toSave.scoreStore5 = loadedData.scoreStore4;
            //toSave.scoreStore4 = loadedData.scoreStore3;
            //toSave.scoreStore3 = loadedData.scoreStore2;
            //toSave.scoreStore2 = loadedData.scoreStore1;
            //toSave.scoreStore1 = score;
        toSave.scoreStore[0] = score;
        
        string jsonData = JsonUtility.ToJson(toSave);
        File.WriteAllText(Application.persistentDataPath + "/save.json", jsonData);
    }

    public IEnumerator LoadGameoverScreen()
    {
        yield return new WaitForSeconds(1);
        gameplayUI.enabled = false;
        gameOverUI.enabled = true;
        gameOverUIBackground.SetActive(true);
        string loadedJson = File.ReadAllText(Application.persistentDataPath + "/save.json");
        scoreBoard loadedData = JsonUtility.FromJson<scoreBoard>(loadedJson);
        int scoreLength = score.ToString().Length;
        for (int i = 0; i < scoreView.Count - 1; i++)
        {
            scoreView[i].text = i + ". ";
            for (int j = 0; j < (8 - scoreLength); j++)
            {
                scoreView[i].text += "0";
            }
            scoreView[i].text += loadedData.scoreStore[i];
        }
        scoreView[10].text = "";
        for (int i = 0; i < (8 - loadedData.highScoreStore.ToString().Length); i++)
        {
            scoreView[10].text += "0";
        }
        scoreView[10].text += loadedData.highScoreStore;
    }

    public void loadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
