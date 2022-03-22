using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blocks : MonoBehaviour
{
    public int blockNumber; 
    public TextMeshProUGUI numberText;
    public GameObject splashEffect, blueSplash;
    public Material[] mat;
    public MeshRenderer otherObject;

    bool once = true;
    
    void Start()
    {
        // numberText = GetComponentInChildren<TextMeshProUGUI>();
        numberText.text = blockNumber.ToString();
        
        
        if (blockNumber >= 9 && blockNumber <= 10 )
            GetComponent<MeshRenderer>().material = mat[4];
        else if (blockNumber >= 7 && blockNumber <= 8)
            GetComponent<MeshRenderer>().material = mat[3];
        else if (blockNumber >= 5 && blockNumber <= 6)
            GetComponent<MeshRenderer>().material = mat[2];
        else if (blockNumber >= 3  && blockNumber <= 4)
            GetComponent<MeshRenderer>().material = mat[1];
        else if (blockNumber >= 0 && blockNumber <= 2)
            GetComponent<MeshRenderer>().material = mat[0];
        
        otherObject.material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (blockNumber <=0 && once)
        {
            once = false;
            // GameManager.gm.score++;
            // GameManager.gm.scoreText.text = GameManager.gm.score.ToString();
            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Medium();
            }
            if (!GameManager.MUSIC_OFF)
            {
                Instantiate(GameManager.gm.blockAudio, transform.position, Quaternion.identity);
            }
            
            Destroy(this.gameObject);
            
            Instantiate(splashEffect, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
            
        }
    }

    public void BombBlasted()
    {
        numberText.text = blockNumber.ToString();

            if (blockNumber >= 10)
                GetComponent<MeshRenderer>().material = mat[4];
            else if (blockNumber >= 7 && blockNumber <= 9)
                GetComponent<MeshRenderer>().material = mat[3];
            else if (blockNumber >= 6 && blockNumber <= 8)
                GetComponent<MeshRenderer>().material = mat[2];
            else if (blockNumber >= 3  && blockNumber <= 5)
                GetComponent<MeshRenderer>().material = mat[1];
            else if (blockNumber >= 0 && blockNumber <= 2)
                GetComponent<MeshRenderer>().material = mat[0];
        
        otherObject.material = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("ShootBrick"))
        {
            Destroy(other.gameObject);
            blockNumber --;
            numberText.text = blockNumber.ToString();

            if (blockNumber >= 10)
                GetComponent<MeshRenderer>().material = mat[4];
            else if (blockNumber >= 7 && blockNumber <= 9)
                GetComponent<MeshRenderer>().material = mat[3];
            else if (blockNumber >= 6 && blockNumber <= 8)
                GetComponent<MeshRenderer>().material = mat[2];
            else if (blockNumber >= 3  && blockNumber <= 5)
                GetComponent<MeshRenderer>().material = mat[1];
            else if (blockNumber >= 0 && blockNumber <= 2)
                GetComponent<MeshRenderer>().material = mat[0];
        
            otherObject.material = GetComponent<MeshRenderer>().material;
        }
    }
}
