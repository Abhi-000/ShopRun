using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pm;
    public bool gameOver = false;
    [HideInInspector] public int totalBricks = 0; 
    [HideInInspector] public bool stopShoot = false;
    [HideInInspector] public bool endingStart = false;
    [HideInInspector] public bool isKicked = false;
    [HideInInspector] public bool rotateCamera = false;
    public  static List<string> itemsInCart = new List<string>();
    List<GameObject> allItems = new List<GameObject>();
    public GameObject stackHolder, playerHolder;
    public GameObject stackBrick, shootBrick,cart;
    Vector3 stackLocation = Vector3.zero;
    public Transform shootPoint;
    private GameObject itemToAdd;
    GameObject[] stacks = new GameObject[100];
    int stackCount = 0, stackAdded = 0;
    public GameObject countingCanvas;

    public bool gameStarted = false;
    public float shootTime = 0.15f;

    GameObject[] c_canvas = new GameObject[100];
    GameObject old_canvas = null;
    TextMeshPro tempText;

    int j = 0;
    float slowdownFactor = 0.07f, slowdownLenght = 2f;
    float fValue;

    void Awake()
    {
        if (pm == null)
        {
            pm = this;
        }
        
    }
    
    void Start()
    {
        stackLocation = stackHolder.transform.position;
        item[] item = transform.GetComponentsInChildren<item>();
        for (int i = 0; i < item.Length; i++)
        {
            item[i].gameObject.SetActive(false);
            allItems.Add(item[i].gameObject);
        }
        if(SceneManager.GetActiveScene().name == "RampWalk")
        {
            doRampWalk();
            //transform.GetComponent<Animator>().SetBool("rampWalk", true);
        }
        else
        {
            itemsInCart.Clear();
        }
    }

    void Update()
    {
    }

    void ShootTheBrick()
    {
        if (stackCount > 0 && gameStarted && (!stopShoot))
        {
            if (!GameManager.gm.feverModeActive)
            {
                Instantiate(shootBrick, shootPoint.transform.position, Quaternion.identity);
                if (!GameManager.MUSIC_OFF)
                {
                    GameObject tempAud = Instantiate(GameManager.gm.shootAudio, transform.position, Quaternion.identity);
                }
                DecreaseStack();
            }
            else
            {
                Instantiate(shootBrick, shootPoint.transform.position, Quaternion.identity);
                Instantiate(shootBrick, shootPoint.transform.position, Quaternion.Euler(0, -7, 0));
                Instantiate(shootBrick, shootPoint.transform.position, Quaternion.Euler(0, 7, 0));
                if (!GameManager.MUSIC_OFF)
                {
                    GameObject tempAud = Instantiate(GameManager.gm.shootAudio, transform.position, Quaternion.identity);
                    tempAud.GetComponent<AudioSource>().pitch = 1.2f;
                }
                DecreaseStack();
            }
        }
    }

    public void DecreaseStack()
    {
        if (stackCount > 0)
        {
            stackCount--;
            stackLocation.y -= 0.2f;
            Destroy(stacks[stackCount].gameObject);
            totalBricks--;
        }
        else
        {
            if (!gameOver)
            {
                gameOver = true;
                GameManager.gm.ShowRetryButton();
                transform.GetComponent<Animator>().SetTrigger("cry");
                Debug.Log("GAME OVER");
            }
        }
    }

    void CountingTagChange()
    {
        // GameObject.FindGameObjectWithTag("CountCanvas").tag = "Old";
        if (old_canvas != null)
            Destroy(old_canvas);
        
        c_canvas[1].tag = "Old";
        old_canvas = c_canvas[1];
        c_canvas[1] = null;
        stackAdded = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
       
        if (other.transform.CompareTag("Brick"))
        {
            Destroy(other.gameObject);
            if (!GameManager.MUSIC_OFF)
            {
                Instantiate(GameManager.gm.cashAudio, other.transform.position, Quaternion.identity);
            }
            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Light();
            }
            GameManager.gm.IncreaseFeverModeValue();
            totalBricks ++;
            
            if (stackCount <= 0)
            {
                stackLocation = new Vector3(stackHolder.transform.position.x, stackLocation.y, stackHolder.transform.position.z);
                stacks[stackCount] = Instantiate(stackBrick, stackLocation, stackHolder.transform.rotation, stackHolder.transform);
                stacks[stackCount].transform.localPosition = new Vector3(0f, stacks[stackCount].transform.localPosition.y, 0f);
            }
            else
            {
                stackLocation = new Vector3(stacks[stackCount - 1].transform.position.x, stackLocation.y, stacks[stackCount - 1].transform.position.z);
                stacks[stackCount] = Instantiate(stackBrick, stackLocation, stackHolder.transform.rotation, stacks[stackCount - 1].transform);
                stacks[stackCount].transform.localPosition = new Vector3(0f, 0.015f, 0f);
                stacks[stackCount].transform.localScale = new Vector3(
                    stacks[stackCount].transform.localScale.x / 30f, 
                    stacks[stackCount].transform.localScale.y / 10f,
                    stacks[stackCount].transform.localScale.z / 30f);
            }
            stackCount ++;
            stackLocation.y += 0.2f;

            
            if (c_canvas.Length > 1)
            {
                
                for (j = 0; j < c_canvas.Length - 1; j++)
                {
                    Destroy(c_canvas[j]);
                }
                c_canvas[0] = c_canvas[j];
                j = 1;
            }
            stackAdded ++;
            c_canvas[j] =  Instantiate(countingCanvas, transform.position + new Vector3(0f, 5f, -2f), Quaternion.identity, playerHolder.transform);
            tempText = c_canvas[j].GetComponentInChildren<TextMeshPro>();
            tempText.text = "+" + stackAdded;
            CancelInvoke("CountingTagChange");
            Invoke("CountingTagChange", 0.6f);
        }

        if (other.transform.CompareTag("Block"))
        {
            GameManager.gm.Invoke("ShowRetryButton", 2.5f);
            
            gameStarted = false;
            GetComponent<Animator>().SetBool("die", true);
            GetComponent<CapsuleCollider>().enabled = false;

            GameObject bucket = GameObject.FindGameObjectWithTag("Bucket");
            bucket.transform.SetParent(null);
            bucket.GetComponent<Rigidbody>().isKinematic = false;

            for (int i = 0; i < stacks.Length; i++)
            {
                if (stacks[i] != null)
                {
                    stacks[i].transform.SetParent(null);
                    stacks[i].GetComponentInChildren<Rigidbody>().isKinematic = false;
                }
            }

            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Failure();
            }
            if (!GameManager.MUSIC_OFF)
            {
                Instantiate(GameManager.gm.failureAudio, transform.position, Quaternion.identity);
            }

        }
    }
    public void AddItemInCart(GameObject other)
    {
        MeshRenderer[] allMeshes = other.transform.GetComponentsInChildren<MeshRenderer>();
        for(int i=0;i< allMeshes.Length; i++)
        {
            if(allMeshes[i].transform.CompareTag("Tag"))
            {
                allMeshes[i].transform.gameObject.SetActive(false);
            }
        }
        other.tag = "Untagged";
        GameObject item = other;
        //GameObject item = Instantiate(other, cart.transform.position+new Vector3(0,2,0), Quaternion.identity,cart.transform);
        itemToAdd = item;
        itemsInCart.Add(item.GetComponent<item>().name);
        EquipItem();
        GameObject starSpawnned = Instantiate(Cart.instance.star, Vector2.zero, Quaternion.identity, Cart.instance.uiCanvas.transform);
        starSpawnned.SetActive(true);
        Invoke("freezeItem",1);
        item.AddComponent<Rigidbody>();
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        //item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        //item.transform.localScale = new Vector3(item.transform.localScale.x / 2.5f, item.transform.localScale.y / 2.5f, item.transform.localScale.z / 2.5f);
    }
    public void freezeItem()
    {
        Debug.Log("ITEM:" + itemToAdd.name);
        itemToAdd.transform.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.CompareTag("CorrectPosition"))
        {
            if (playerHolder.transform.position.x < 1 && playerHolder.transform.position.x >= -1)
                playerHolder.transform.position = new Vector3(0f, playerHolder.transform.position.y, playerHolder.transform.position.z);
            
            else if (playerHolder.transform.position.x < 3 && playerHolder.transform.position.x >= 1)
                playerHolder.transform.position = new Vector3(2f, playerHolder.transform.position.y, playerHolder.transform.position.z);
            
            else if (playerHolder.transform.position.x < 5 && playerHolder.transform.position.x >= 3)
                playerHolder.transform.position = new Vector3(4f, playerHolder.transform.position.y, playerHolder.transform.position.z);
            
            else if (playerHolder.transform.position.x < -1 && playerHolder.transform.position.x >= -3)
                playerHolder.transform.position = new Vector3(-2f, playerHolder.transform.position.y, playerHolder.transform.position.z);
            
            else if (playerHolder.transform.position.x < -3 && playerHolder.transform.position.x >= -5)
                playerHolder.transform.position = new Vector3(-4f, playerHolder.transform.position.y, playerHolder.transform.position.z);
        }

        if (other.transform.CompareTag("End"))
        {
            gameStarted = false;
            // GameManager.gm.Invoke("ShowNextButton", 3f);
            // GameManager.gm.Invoke("GameComplete", 0.5f);
            // GetComponent<Animator>().SetTrigger("victory");
            GameManager.gm.CallEndPanel();
        }

        if (other.transform.CompareTag("EndingStart"))
        {           
            PlayerController.pc.speed = 16f;
            //gameStarted = false;
            stopShoot = true;
            endingStart = true;
            stackHolder.transform.parent = playerHolder.transform;
            stackHolder.transform.eulerAngles = new Vector3(0, 180f, 0);
            stackHolder.transform.localPosition = new Vector3(0, 0.55f, 2.5f);
            //stackHolder.transform.localScale *= 2f;
            //cart.SetActive(false);
            //doRampWalk();
            //Invoke("KickTheStack",0.5f);
        }

        if (other.transform.CompareTag("Cheque"))
        {
            Destroy(other.gameObject);
            if (!GameManager.MUSIC_OFF)
            {
                Instantiate(GameManager.gm.chequeAudio, other.transform.position, Quaternion.identity);
            }
            
            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Medium();
            }
            
            Instantiate(GameManager.gm.chequePop, other.transform.position, Quaternion.identity);
            GameManager.gm.score++;
            GameManager.gm.scoreText.text = GameManager.gm.score.ToString();
        }
    }
    public void doRampWalk()
    {
        //cart.SetActive(false);
        transform.GetComponent<Animator>().SetBool("rampWalk", true);
        transform.GetComponent<Animator>().SetBool("run", false);
        item[] allItemsinChild = GetComponentsInChildren<item>();
        for (int i = 0; i < allItemsinChild.Length; i++)
        {
            allItemsinChild[i].gameObject.SetActive(false);
        }
        //item[] item = transform.GetComponentsInChildren<item>();
        EquipItem();
    }
    void EquipItem()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            for (int j = 0; j < itemsInCart.Count; j++)
            {
                if (allItems[i].GetComponent<item>().name == itemsInCart[j])
                {
                    allItems[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
    void KickTheStack()
    {
        stackHolder.transform.localPosition = Vector3.Lerp(stackHolder.transform.localPosition, new Vector3(0, 0.55f, 6f), 0.1f);
        gameStarted = false;
        rotateCamera = true;
        Invoke("DoSlowmotion", 0.2f);
        GetComponent<Animator>().SetBool("run", false);
        GetComponent<Animator>().SetTrigger("kick");
        Invoke("ThrowStack", 0.4f);

        Invoke("TrueVariable", 0.75f);
    }

    void TrueVariable()
    {
        isKicked = true;
    }

    void ThrowStack()
    {
        GameManager.gm.Invoke("BlinkBonusGround", 3.0f);
        
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Success();
        }
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                stacks[i].transform.SetParent(null);
                stacks[i].GetComponentInChildren<Rigidbody>().isKinematic = false;
                // stacks[i].GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 0, 10 * Time.deltaTime));
            }
        }

        for ( int i = stacks.Length - 1; i >= 0; i--)
        {
            if (stacks[i] != null)
            {
                stacks[i].GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 0, (50000 + (i * 5000)) * Time.deltaTime));
            }
        }
    }

    void DoSlowmotion()
    {
        // print("slow");
        Time.timeScale = slowdownFactor; // 0.07f float variable
        fValue = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Invoke("normalMotion", 0.13f);
        
    } 
    void normalMotion()
    {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fValue;
        
    }
    void DoFastmotion()
    {
        Time.timeScale = 7;
        fValue = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 2.5f;
        Invoke("normalMotion", 2f);
    }
}
