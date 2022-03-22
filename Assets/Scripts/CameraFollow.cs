using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [HideInInspector] public float smoothSpeed = 0.125f;
    
    
    void LateUpdate()
    {
        if (PlayerManager.pm.isKicked)
        {
            Vector3 desiredPosition = (GameManager.gm.bonusEndPos*2) + new Vector3(2.75f, 5.83f, -12.15f);
            //Vector3 desiredPosition = GameManager.gm.bonusEndPos * 2;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed/20);
            transform.position = smoothPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(11.472f, -10.463f, 0), smoothSpeed / 6);
        }
        else if (PlayerManager.pm.rotateCamera)
        {
            Vector3 desiredPosition = target.position + new Vector3(8f, 7.55f, -7f);
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed / 7);
            transform.position = smoothPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(9.234f, -40f, 0), smoothSpeed / 15);
            // new Vector3(9.234, -45.562, 0);
        }
        else if (PlayerManager.pm.endingStart)
        {
            Vector3 desiredPosition = target.position + new Vector3(8f, 7.55f, -7f);
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed / 7);
            transform.position = smoothPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(9.234f, -27.339f, 0), smoothSpeed / 7);
            // new Vector3(9.234, -45.562, 0);
        }
        else
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
            // transform.LookAt(target);
        }
    }
}
