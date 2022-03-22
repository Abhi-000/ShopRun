using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public GameObject text1, text2;
    public GameObject countDown;
    bool once1 = true, once2 = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (transform.CompareTag("Step1") && once1)
            {
                PlayerManager.pm.gameStarted = false;
                text1.SetActive(true);
                Invoke("DisableText1", 4.0f);
                once1 = false;
            }
            else if (transform.CompareTag("Step2") && once2)
            {
                PlayerManager.pm.gameStarted = false;
                text2.SetActive(true);
                Invoke("DisableText2", 4.0f);
                once2 = false;
            }
        }
    }

    void DisableText1()
    {
        text1.SetActive(false);
        countDown.SetActive(true);
        Invoke("StopCountDown", 3.3f);
    }
    void DisableText2()
    {
        text2.SetActive(false);
        countDown.SetActive(true);
        Invoke("StopCountDown", 3.3f);
    }
    void StopCountDown()
    {
        countDown.SetActive(false);
        PlayerManager.pm.gameStarted = true;
    }
}


