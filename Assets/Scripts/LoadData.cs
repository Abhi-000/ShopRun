using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    int sceneIndex;

    private void Start() 
    {
        LoadSavedData();
        // PlayerPrefs.DeleteAll();
    }


    public void LoadSavedData()
    {
        if (!PlayerPrefs.HasKey("LastScene"))
        {
            PlayerPrefs.SetInt("LastScene", 0);
            PlayerPrefs.SetInt("LevelNumber", 1);
            Debug.Log("No key found. Loading first level.");
            sceneIndex = (1 + PlayerPrefs.GetInt("LastScene"));
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            sceneIndex = PlayerPrefs.GetInt("LastScene");
            if(sceneIndex == 0)
            {
                sceneIndex = 1;
            }
            SceneManager.LoadScene(sceneIndex);
            Debug.Log("Key found, Loading level" + PlayerPrefs.GetInt("LastScene").ToString());
        }
    }

}
