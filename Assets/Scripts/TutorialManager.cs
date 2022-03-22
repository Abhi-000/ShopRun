using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.GetInt("isTut") == 0)
        {
            PlayerPrefs.SetInt("isTut", 1);
            GameManager.gm.tutorailLevel = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
