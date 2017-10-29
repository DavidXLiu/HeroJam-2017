using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Code by: David Liu
 */
public class Exit : MonoBehaviour {

    public bool selected;

	// Use this for initialization
	void Start () {
        selected = false;
	}

    // Triggers on mouse click
    private void OnMouseDown()
    {
        selected = !selected;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
