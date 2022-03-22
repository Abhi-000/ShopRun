using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickShoot : MonoBehaviour
{
    public GameObject destroyEffect;

    void Start()
    {
        Destroy(this.gameObject, 2.0f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 40f);
    }

    void OnDestroy()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }
}
