using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("Numbers")]
    public long score = 0;
    public int perClick = 1;
    public int upgradeBaseCost = 10;
    public int autoUpgradeBaseCost = 50;
    int upgradeLevel = 0;
    int upgradeCostMultiplier = 5;
    int autoCostMultiplier = 25;
    int autoLevel = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI perClickText;
    public TextMeshProUGUI upgradeButtonText;
    public TextMeshProUGUI autoButtonText;

    [Header("Audio")]
    public AudioSource clickSFX1;
    public AudioClip clickSFX1Clip;
    public AudioSource clickSFX2;
    public AudioClip clickSFX2Clip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ClickerFrequency());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        score += perClick;
        PlaySoundEffect(clickSFX1, clickSFX1Clip);
    }

    public void BuyUpgrade()
    {
        int cost = upgradeBaseCost + (upgradeLevel * upgradeCostMultiplier);
        if (score >= cost)
        {
            score -= cost;
            upgradeLevel += 1;
            perClick += 1;
        }
            

    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
        perClickText.text = "Click Power: " + perClick;
        upgradeButtonText.text = "Upgrade Cost: " + (upgradeBaseCost + upgradeLevel * upgradeCostMultiplier);
        autoButtonText.text = "Hire Auto Clicker | Cost: " + (autoUpgradeBaseCost + autoLevel * autoCostMultiplier);
    }

    int GetUpgradeCost()
    {
        double cost = upgradeBaseCost * System.Math.Pow(upgradeCostMultiplier, upgradeLevel);
        return Mathf.CeilToInt((float)cost);
    }

    public void BuyAutoClicker()
    {
        int cost = autoUpgradeBaseCost + (autoLevel * autoCostMultiplier);
        if (score >= cost)
        {
            score -= cost;
            autoLevel+= 1;
            UpdateScoreUI();
        }
    }

    void AutoTick()
    {
        if (autoLevel > 0)
        {
            score += autoLevel;
            PlaySoundEffect(clickSFX2, clickSFX2Clip);
            UpdateScoreUI();
        }
    }

    IEnumerator ClickerFrequency()
    {
        while (true)
        {
            AutoTick();
            yield return new WaitForSeconds(1.0f);
        }
    }

    void PlaySoundEffect(AudioSource sfxSource, AudioClip soundEffect)
    {
        sfxSource.PlayOneShot(soundEffect);
    }
}
