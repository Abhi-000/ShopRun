using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameObject[] obstacles;
    public bool canMove = false;

    private void Update() {
        if (canMove)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].transform.localPosition = Vector3.MoveTowards(obstacles[i].transform.localPosition, new Vector3(0, 0, 0), 120 * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player"))
        {
            canMove = true;
        }
    }
}
