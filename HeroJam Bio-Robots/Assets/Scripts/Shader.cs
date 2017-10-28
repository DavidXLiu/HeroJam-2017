using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShader : MonoBehaviour {

    public Shader shader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver()
    {
        gameObject.GetComponent<Renderer>().material.shader = shader;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
    }
}
