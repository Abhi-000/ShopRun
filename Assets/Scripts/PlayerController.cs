using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController pc;

    float h, width;
    bool onlyOnce = false;
    public GameObject playerHolder, mainPlayer;
    public float speed = 8f;
    
    Vector3 pos;
    Vector3  desiredPos;

    public Animator anim;
    public CharacterController cc;
    
    private void Awake() {
        if (pc == null)
        {
            pc = this;
        }
    }
    
    void Start()
    {
        width = Screen.width;
        anim = mainPlayer.GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        if (PlayerManager.pm.gameStarted && !Cart.pausePlayer)
        {
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            // cc.Move(new Vector3(0, 0, speed * Time.deltaTime));
            anim.SetBool("run", true);
            
            if (Input.GetMouseButton(0) || (PlayerManager.pm.totalBricks <= 0))
            {
                anim.SetBool("shoot", false);
            }
            else if (PlayerManager.pm.totalBricks > 0 && (!PlayerManager.pm.stopShoot))
            {
               // anim.SetBool("shoot", true);
            }
        

            if (Input.GetMouseButtonDown(0))
            {
                h = Input.mousePosition.x;
                pos = playerHolder.transform.localPosition;
            }
            if (Input.GetMouseButton(0) && (!PlayerManager.pm.stopShoot))
            {
                //  h = SimpleInput.GetAxis("Horizontal");
                if (!onlyOnce)
                {
                    // an.SetBool("run",true);
                    onlyOnce = true;
                    mainPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            

                float p = (h - Input.mousePosition.x) / width;
                p *= -15;
                desiredPos = pos + new Vector3(p, 0, 0);
                desiredPos.x = Mathf.Clamp(desiredPos.x, -4.2f, 4.2f);
                // playerHolder.transform.localPosition = new Vector3(desiredPos.x, desiredPos.y, playerHolder.transform.localPosition.z);
                // cc.Move(desiredPos - transform.position);
                
                
            }
            cc.Move(new Vector3(desiredPos.x - transform.position.x, desiredPos.y - transform.position.y, speed * Time.deltaTime));
        }
        else
        {
            anim.SetBool("run", false);
        }

    }

}
