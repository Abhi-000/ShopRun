using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadNextLevel : MonoBehaviour
{
    public int levelInt;
    [HideInInspector]
    public static int levelIndex = 0;
    public static LoadNextLevel lnl;
    public static int LevelCount = 1;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI nextLvlNumberText;

    private void Awake()
    {
        if (lnl == null)
        {
            lnl = this;
        }
    }

    void Start()
    {
        // PlayerPrefs.SetInt("LevelNumber", 1);
        LevelCount = PlayerPrefs.GetInt("LevelNumber");
        // int a = LevelCount + 1;
        levelNumberText.text = LevelCount.ToString();
        nextLvlNumberText.text = (LevelCount + 1).ToString();
        levelInt = LevelCount;
        // levelNumberText.text = "250";
        // nextLvlNumberText.text = "251";
    }

    public void LoadLevel()
    {
        UpdateLevelCount();
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            levelIndex++;
            // SceneManager.LoadScene(levelIndex);
            SaveData(SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("LastScene"));
            
        }
        else
        {
            levelIndex = 1;
            // PlayerPrefs.SetInt("LastEnabledItem", 0);
            PlayerPrefs.SetInt("LastScene", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(PlayerPrefs.GetInt("LastScene"));
        }
    }

    void SaveData(int buildIndex)
    {
        // PlayerPrefs.SetInt("LastEnabledItem", 0);
        PlayerPrefs.SetInt("LastScene", buildIndex);
        PlayerPrefs.Save();
    }

    public void UpdateLevelCount()
    {

        LevelCount++;
        PlayerPrefs.SetInt("LevelNumber", LevelCount);
        levelNumberText.text = PlayerPrefs.GetInt("LevelNumber").ToString();
        
    }
}
