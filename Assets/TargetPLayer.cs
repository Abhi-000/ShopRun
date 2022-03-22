using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPLayer : MonoBehaviour
{
    bool reached = false,over = false;
    public GameObject target;
    // Update is called once per frame
    void Update()
    {
        if (!reached && !over)
        {
            transform.position += new Vector3(0, 0, 1f * Time.deltaTime);
        }
        else if(!over)
        {
            transform.position -= new Vector3(0, 0, 1f * Time.deltaTime);
        }
        if(transform.position.z>=6.7)
        {
            reached = true;
        }
        if(transform.position.z<0 && reached)
        {
            over = true;
        }
    }
}
