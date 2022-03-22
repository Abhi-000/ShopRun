using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cart : MonoBehaviour
{
    public static Cart instance;
    public GameObject mainCart, priceTag,star,uiCanvas;
    int price;
    float itemSize;
    public static bool pausePlayer = false;
    public TextMeshProUGUI itemTotal;
    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            itemSize = other.transform.localScale.x;
            pausePlayer = true;
            price = other.transform.GetComponent<item>().itemCost;
            MeshRenderer[] allMeshes = other.transform.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < allMeshes.Length; i++)
            {
                Debug.Log(allMeshes[i].tag);
                if (allMeshes[i].CompareTag("Tag"))
                {
                    priceTag = allMeshes[i].gameObject;
                }
            }
            StartCoroutine(decreasePrize(other));
        }
    }
    IEnumerator decreasePrize(Collider other)
    {
        
        while (price > 0)
        {
            if (other.transform.localScale.x >= itemSize/2.5)
            {
                other.transform.localScale -= new Vector3(itemSize/10, itemSize / 10, itemSize / 10);
            }
            yield return new WaitForSeconds(0.1f);
            if (!PlayerManager.pm.gameOver)
            {
                price--;
                priceTag.transform.GetChild(0).transform.GetComponent<TextMeshPro>().text = "$" + price.ToString();
                PlayerManager.pm.DecreaseStack();
            }
        }
        if (price <= 0)
        {
            PlayerManager.pm.AddItemInCart(other.gameObject);
            Destroy(other.transform.gameObject);
            pausePlayer = false;
            itemTotal.text = PlayerManager.itemsInCart.Count.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT");
    }
    private void Update()
    {
        transform.position = mainCart.transform.position + new Vector3(0, 0, 3.5f);
    }
}