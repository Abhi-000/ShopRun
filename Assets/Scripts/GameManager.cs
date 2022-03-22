using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    
    public GameObject playButton, retryButton, nextButton,transition, itemSelected;
    public GameObject victoryPopUp;
    public bool old_mat = true;
    
    public Slider levelSlider;
    public int endZValue;
    public GameObject playerHolder;
    public ParticleSystem confetti;

    public Slider feverModeSlider;
    public Image feverModeBar;
    int feverMode_MaxValue = 40;
    int feverMode_Value = 0;
    [HideInInspector] public bool feverModeActive = false;
    public GameObject feverModeText;

    public Material whiteGlow;
    public GameObject[] bonusGrounds;
    public GameObject bonusTextup;

    [HideInInspector] public float bonusVal = 0;
    [HideInInspector] public int bonusSerialNum = 0;
    [HideInInspector] public Vector3 bonusEndPos = new Vector3(0, 3.5f, 241);

    [HideInInspector] public int totalScore = 0, score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalScoreText;
    public GameObject scorePanel;
    public GameObject totalScorePanel;

    public GameObject endScreen;
    public GameObject claimButton, noThanksButton;
    public TextMeshProUGUI endScreen_upText, endScreen_downText;

    public GameObject chequePop;
    public GameObject GroundConfetti;

    public GameObject retryPanel;
    public GameObject skipButton;
    static bool canSkip = false;
    bool canShowNoThanks = true;
    bool once = true;

    public GameObject musicON_button, musicOFF_button, vibrationON_button, vibrationOFF_button;

    public GameObject cashAudio, chequeAudio, shootAudio, blockAudio, failureAudio, successAudio, bonusWallAudio;

    [HideInInspector] public static bool VIBRATION_OFF = false;
    [HideInInspector] public static bool MUSIC_OFF = false;
    int vibr, musi;

    public bool tutorailLevel = false;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    private void Start()
    {
        Cart.pausePlayer = false;
        score = 0;
        totalScore = PlayerPrefs.GetInt("TotalScore");
        if (totalScore <= 0)
        {
            totalScore = 0;
            PlayerPrefs.SetInt("TotalScore", totalScore);
        }

        totalScoreText.text = totalScore.ToString();
        scoreText.text = score.ToString();

        totalScorePanel.SetActive(true);
        scorePanel.SetActive(false);
        bonusEndPos = bonusGrounds[0].transform.position;

        vibr = PlayerPrefs.GetInt("Vibration");
        if (vibr == 0)
        {
            VIBRATION_OFF = false;
            vibrationOFF_button.SetActive(false);
            vibrationON_button.SetActive(true);
        }
        else if (vibr == 1)
        {
            VIBRATION_OFF = true;
            vibrationOFF_button.SetActive(true);
            vibrationON_button.SetActive(false);
        }

        musi = PlayerPrefs.GetInt("Music");
        if (musi == 0)
        {
            MUSIC_OFF = false;
            musicOFF_button.SetActive(false);
            musicON_button.SetActive(true);
        }
        else if (musi == 1)
        {
            MUSIC_OFF = true;
            musicOFF_button.SetActive(true);
            musicON_button.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerHolder.transform.position.z < endZValue)
        {
            levelSlider.value = playerHolder.transform.position.z / endZValue;
        }
        else
        {
            levelSlider.value = 1;
        }
    }

    public void IncreaseFeverModeValue()
    {
        if (tutorailLevel)
        {
            return;
        }
        feverMode_Value++;
        if (feverMode_Value <= feverMode_MaxValue)
        {
            feverModeSlider.value = (float)feverMode_Value / (float)feverMode_MaxValue;
        }
        else if (feverMode_Value > feverMode_MaxValue)
        {
            if (once)
            {
                feverModeActive = true;
                feverModeText.SetActive(true);
                feverModeBar.color = new Color32(255, 98, 105, 255);
                InvokeRepeating("DecreaseFeverModeBar", 0.01f, 0.1f);
                Invoke("DeactivateFeverMode", 6f);
                once = false;
            }
        }
    }

    public void DeactivateFeverMode()
    {
        feverModeText.SetActive(false);
        feverModeSlider.value = 0;
        feverMode_Value = 0;
        feverModeActive = false;
        feverModeBar.color = new Color32(157, 255, 255, 255);
        once = true;
    }

    void DecreaseFeverModeBar()
    {
        if (feverModeActive)
        {
            feverModeSlider.value -= (float)1/60;
            if (feverModeSlider.value <= 0)
            {
                feverModeSlider.value = 0;
                CancelInvoke("DecreaseFeverModeBar");
            }
        }
        else
        {
            CancelInvoke("DecreaseFeverModeBar");
        }
    }

    public void LastBonusBox(GameObject bonusItem, int serial)
    {
        if (serial > bonusSerialNum)
        {
            itemSelected = bonusItem;
            //PlayerManager.pm.AddItemInCart(bonusItem);
            //bonusVal = bonusV;
            bonusSerialNum = serial;
            //bonusEndPos = bonusGrounds[bonusSerialNum - 1].transform.position;
            CancelInvoke("AddItemInCart");
            Invoke("AddItemInCart",3);
            CancelInvoke("BlinkBonusGround");
            Invoke("BlinkBonusGround", 3.0f);
        }
    }
    public void AddItemInCart()
    {
        Debug.Log("BONUS ITEM:" + itemSelected.GetComponent<item>().name);
        PlayerManager.itemsInCart.Add(itemSelected.GetComponent<item>().name);
        GameObject starSpawnned = Instantiate(Cart.instance.star, Vector2.zero, Quaternion.identity, Cart.instance.uiCanvas.transform);
        starSpawnned.SetActive(true);
        GameManager.gm.LoadRampWalkScene();
    }
    void BlinkBonusGround()
    {
        if (bonusSerialNum - 1 >= 0)
        {
        bonusGrounds[bonusSerialNum - 1].GetComponent<Bonus>().BlinkThisGround();
        GameObject temp = Instantiate(bonusTextup, bonusGrounds[bonusSerialNum - 1].transform.position + new Vector3(0, 2f, 0), Quaternion.identity);

        if (bonusVal == 1 || bonusVal == 2 || bonusVal == 3 || bonusVal == 4 || bonusVal == 5 || bonusVal == 6)
        {
            temp.GetComponentInChildren<TextMeshProUGUI>().text = 'x' + bonusVal.ToString() + ".0";
            endScreen_upText.text = score.ToString() + " X " + bonusVal.ToString() + ".0";
        }
        else
        {
            temp.GetComponentInChildren<TextMeshProUGUI>().text = 'x' + bonusVal.ToString();
            endScreen_upText.text = score.ToString() + " X " + bonusVal.ToString();
        }
        }
        // Invoke("ShowNextButton", 3f);
        StartCoroutine(EndPanel());

    }
    public void LoadRampWalkScene()
    {
        StartCoroutine(LoadRampScene());
    }
    IEnumerator LoadRampScene()
    {
        transition.SetActive(true);
        transition.GetComponent<Animator>().SetTrigger("start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("RampWalk");
    }
    public void CallEndPanel()
    {
        StartCoroutine(EndPanel());
    }

    IEnumerator EndPanel()
    {
        if (!tutorailLevel)
        {
            if (bonusVal == 0)
            {
                endScreen_upText.text = score.ToString() + " X " + bonusVal.ToString();
            }
            endScreen_downText.text = ((int)((float)score * (float)bonusVal)).ToString();
            score = ((int)((float)score * (float)bonusVal));
            yield return new WaitForSeconds(2.5f);
            endScreen.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            claimButton.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            if (canShowNoThanks)
            {
                noThanksButton.SetActive(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            endScreen.SetActive(false);
            endScreen_upText.text = "";
            endScreen_downText.text = "0";
            claimButton.SetActive(false);
            noThanksButton.SetActive(false);
            nextButton.SetActive(true);
            // endScreen.SetActive(true);
        }
    }

    public void Play()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        PlayerManager.pm.gameStarted = true;
        totalScorePanel.SetActive(false);
        if (!tutorailLevel)
        {
            scorePanel.SetActive(true);
            feverModeSlider.gameObject.SetActive(true);
        }
        playButton.SetActive(false);
        levelSlider.gameObject.SetActive(true);
        musicOFF_button.SetActive(false);
        musicON_button.SetActive(false);
        vibrationOFF_button.SetActive(false);
        vibrationON_button.SetActive(false);
    }

    public void GameComplete()
    {
        victoryPopUp.SetActive(true);
        confetti.Play();
    }
    
    public void ShowRetryButton()
    {
        // retryButton.SetActive(true);
        if (canSkip)
        {
            skipButton.SetActive(true);
        }
        else
        {
            skipButton.SetActive(false);
        }
        retryPanel.SetActive(true);
    }

    public void ShowNextButton()
    {
        canShowNoThanks = false;
        nextButton.SetActive(true);
    }

    public void Retry()
    {
        canSkip = true;
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Next()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        if (!tutorailLevel)
        {
            totalScore += score;
            PlayerPrefs.SetInt("TotalScore", totalScore);
            totalScoreText.text = totalScore.ToString();
        }
        
        feverModeSlider.gameObject.SetActive(false);
        LoadNextLevel.lnl.LoadLevel();
    }

    public void ClaimNowButton()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        // victoryPopUp.SetActive(false);
        nextButton.SetActive(true);
        score *= 2;
        // scoreText.text = score.ToString();
        endScreen_downText.text = score.ToString();
        claimButton.SetActive(false);
        noThanksButton.SetActive(false);
        ShowNextButton();
        // confetti.Stop();
        
    }

    public void SkipLevelButton()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        feverModeSlider.gameObject.SetActive(false);
        LoadNextLevel.lnl.LoadLevel();
    }

    public void MakeVibrationON()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        vibrationON_button.SetActive(true);
        vibrationOFF_button.SetActive(false);
        
        VIBRATION_OFF = false;
        vibr = 0;
        PlayerPrefs.SetInt("Vibration", vibr);
    }

    public void MakeVibrationOFF()
    {
        vibrationON_button.SetActive(false);
        vibrationOFF_button.SetActive(true);

        VIBRATION_OFF = true;
        vibr = 1;
        PlayerPrefs.SetInt("Vibration", vibr);
    }

    public void MakeMusicON()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        musicON_button.SetActive(true);
        musicOFF_button.SetActive(false);

        MUSIC_OFF = false;
        musi = 0;
        PlayerPrefs.SetInt("Music", musi);
    }

    public void MakeMusicOFF()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Light();
        }
        musicON_button.SetActive(false);
        musicOFF_button.SetActive(true);

        MUSIC_OFF = true;
        musi = 1;
        PlayerPrefs.SetInt("Music", musi);
    }

    
/*
    public void NoThanksButton()
    {
        victoryPopUp.SetActive(false);
        nextButton.SetActive(true);
        confetti.Stop();
    }
*/
}
