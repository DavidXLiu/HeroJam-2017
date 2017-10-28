using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShader : MonoBehaviour {

    public Shader hoverShader;
    public Shader selectedShader;
    public bool hoveredOver;
    public bool selected;
    public bool shaderEnabled;

	// Use this for initialization
	void Start () {
        hoveredOver = false;
	}

    // Updated called consecutively
    private void FixedUpdate()
    {
        if(gameObject.tag == "Human")
        {
            selected = gameObject.GetComponent<HumanMovement>().selected;
            shaderEnabled = gameObject.GetComponent<HumanMovement>().selectionEnabled;
        }
        else if(gameObject.tag == "Waste")
        {
            selected = gameObject.GetComponent<Waste>().selected;
            shaderEnabled = gameObject.GetComponent<Waste>().selectionEnabled;
        }
        else if(gameObject.tag == "Shovel")
        {
            selected = gameObject.GetComponent<Shovel>().selected;
            shaderEnabled = gameObject.GetComponent<Shovel>().selectionEnabled;
        }
        else if(gameObject.name == "Exit")
        {
            selected = gameObject.GetComponent<Exit>().selected;
        }

        if(!hoveredOver)
        {
            if (selected)
            {
                gameObject.GetComponent<Renderer>().material.shader = selectedShader;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnMouseOver()
    {
        Debug.Log("Hovered");
        hoveredOver = true;

        if(!selected && shaderEnabled)
        {
            gameObject.GetComponent<Renderer>().material.shader = hoverShader;
        }
    }

    private void OnMouseExit()
    {
        hoveredOver = false;
        if(!selected && shaderEnabled)
        {
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        }
    }
}
