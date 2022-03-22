using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlade : MonoBehaviour
{
    
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * 60f);
    }
}
