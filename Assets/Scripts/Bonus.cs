using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bonus : MonoBehaviour
{
    public TextMeshProUGUI text_1, text_2;
    public GameObject ground,bonusItem;
    public float bonusValue;
    public int serialNumber;
    Material groundMaterial;

    public bool isLast = false;

    void Start()
    {
        groundMaterial = ground.GetComponent<MeshRenderer>().material;
        
       /* if (bonusValue == 1 || bonusValue == 2 || bonusValue == 3 || bonusValue == 4 || bonusValue == 5 || bonusValue == 6)
        {
            text_1.text = 'x' + bonusValue.ToString() + ".0";
            text_2.text = 'x' + bonusValue.ToString() + ".0";
        }
        else
        {
            text_1.text = 'x' + bonusValue.ToString();
            text_2.text = 'x' + bonusValue.ToString();
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("StackBrick"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            //text_2.text = null;
            GameManager.gm.LastBonusBox(bonusItem, serialNumber);
            Instantiate(GameManager.gm.GroundConfetti,
                new Vector3(-6f, 0, transform.position.z + 5f), Quaternion.Euler(-90, 0, 0));
            Instantiate(GameManager.gm.GroundConfetti,
                new Vector3(6f, 0, transform.position.z + 5f), Quaternion.Euler(-90, 0, 0));
            
            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Light();
            }
            if (!GameManager.MUSIC_OFF)
            {
                Instantiate(GameManager.gm.bonusWallAudio, transform.position, Quaternion.identity);
            }

            if (isLast)
            {
                Invoke("MoreConfetti", 2f);
                Invoke("MoreConfetti", 4f);
            }
           // Invoke("AddItemInCart", 5f);
        }
    }
    

    void MoreConfetti()
    {
        Instantiate(GameManager.gm.GroundConfetti,
            new Vector3(-7.5f, 0, transform.position.z + 5f), Quaternion.Euler(-90, 0, 0));
        Instantiate(GameManager.gm.GroundConfetti,
            new Vector3(7.5f, 0, transform.position.z + 5f), Quaternion.Euler(-90, 0, 0));
        Instantiate(GameManager.gm.GroundConfetti,
            new Vector3(0f, 0, transform.position.z + 5f), Quaternion.Euler(-90, 0, 0));
    }

    public void BlinkThisGround()
    {
        StartCoroutine(Glow());
    }

    IEnumerator Glow()
    {
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Heavy();
        }
        if (!GameManager.MUSIC_OFF)
        {
            Instantiate(GameManager.gm.successAudio, transform.position, Quaternion.identity);
        }
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = GameManager.gm.whiteGlow;
        yield return new WaitForSeconds(0.15f);
        ground.GetComponent<MeshRenderer>().material = groundMaterial;
    }
}
