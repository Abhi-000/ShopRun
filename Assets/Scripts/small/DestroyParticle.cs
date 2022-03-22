using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public float destroyTime = 1.5f;
    
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

}
