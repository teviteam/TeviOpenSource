using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animHandler : MonoBehaviour {

	// Use this for initialization
	

    public void TriggerAnim()
    {
        gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Play"));
        //gameObject.GetComponent<Animator>().ResetTrigger(Animator.StringToHash("Play"));


        
        /*
        gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Play"));
        gameObject.GetComponent<Animator>().ResetTrigger(Animator.StringToHash("Play"));
    */
    } 
    public void revParAni()
    {
        if (!gameObject.GetComponent<Animator>().GetBool(Animator.StringToHash("Rev")))
        {
            gameObject.GetComponent<Animator>().SetBool(Animator.StringToHash("Rev"), true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool(Animator.StringToHash("Rev"), false);
        }
    }
    
}
