using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Code by: David Liu
 */
public class HumanText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

    // Update is called consecutively
    private void FixedUpdate()
    {
        Vector3 distance = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(distance);
    }

    // Update is called once per frame
    void Update () {
        
	}
}
