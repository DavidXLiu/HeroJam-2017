using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleShader : MonoBehaviour {

    public Shader invisibleShader;
    public Shader invisibleHoverShader;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.shader = invisibleShader;
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void OnMouseOver()
    {
        gameObject.GetComponent<Renderer>().material.shader = invisibleHoverShader;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.shader = invisibleShader;
    }
}
