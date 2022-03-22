using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject splashEffect;
    float radius = 3f;

    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("ShootBrick"))
        {
            Destroy(other.gameObject);
            if (!GameManager.VIBRATION_OFF)
            {
                Taptic.Medium();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider nearbyObjects in colliders)
            {
                Blocks blk = nearbyObjects.GetComponent<Blocks>();
                if (blk != null)
                {
                    blk.blockNumber -= 20;
                    blk.BombBlasted();
                }
            }

            Collider[] colliders2 = Physics.OverlapSphere(transform.position, radius + 6f);
            foreach (Collider nearbyObjects in colliders2)
            {
                Blocks blk2 = nearbyObjects.GetComponent<Blocks>();
                if (blk2 != null)
                {
                    blk2.blockNumber -= 2;
                    blk2.BombBlasted();
                }
            }

            Instantiate(splashEffect, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
