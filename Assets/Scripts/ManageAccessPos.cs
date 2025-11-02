using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAccessPos : MonoBehaviour
{
    GameObject SkinAccessAnimObj;
    Animator skinAccessAnimator;
    Animator playerAnimator;

    //string currentState;

    private void Awake()
    {
        SkinAccessAnimObj = transform.GetChild(0).gameObject;
        playerAnimator = transform.parent.GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (SkinChanger.GetSkinName() == "devil")
        {
            SkinAccessAnimObj.SetActive(true);
            skinAccessAnimator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SkinAccessAnimObj.activeSelf == true ) //&& !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(currentState)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
            {
                skinAccessAnimator.SetBool("isJumping", true);
                skinAccessAnimator.SetBool("isCharging", false);
                skinAccessAnimator.SetBool("isIdling", false);
                //currentState = "Jumping";
            }
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Charging"))
            {
                skinAccessAnimator.SetBool("isCharging", true);
                skinAccessAnimator.SetBool("isJumping", false);
                skinAccessAnimator.SetBool("isIdling", false);
                //currentState = "Charging";
            }
            else
            {
                skinAccessAnimator.SetBool("isIdling", true);
                skinAccessAnimator.SetBool("isJumping", false);
                skinAccessAnimator.SetBool("isCharging", false);
                //currentState = "Idling";
            }
        }
    }
}
