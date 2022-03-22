using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballObstacle : MonoBehaviour
{
    public GameObject ball;


    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player"))
        {
            ball.GetComponent<Animation>().Play();
        }
    }
}
