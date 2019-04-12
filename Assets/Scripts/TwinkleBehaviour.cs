using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinkleBehaviour : MonoBehaviour {


    public Animator anim;

    private Transform parent;
    private twinkleController controller;
    private MeshRenderer meshRenderer;


	// Use this for initialization
	void Start () {

        parent = transform.parent;
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (parent)
        {
            controller = parent.GetComponent<twinkleController>();
        }

   
        
		
	}

    void Reposition()
    {
        if (parent)
        {
            float posX = Random.Range(-0.3f, 0.3f) + parent.transform.position.x;
            float posZ = Random.Range(-0.3f, 0.3f) + parent.transform.position.z;

            transform.position = new Vector3(posX, transform.position.y, posZ);

        
        }

       
    }





}
