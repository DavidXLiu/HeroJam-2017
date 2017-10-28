using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour {

    public GameObject humanConnected;
    public float distanceFromHuman;

    public bool selected;
    public bool selectionEnabled;

    // Use this for initialization
    void Start () {
        selected = false;
        selectionEnabled = true;
    }

    // Triggers on mouse click
    private void OnMouseDown()
    {
        if (selectionEnabled)
        {
            if (!selected)
            {
                foreach (GameObject shovel in GameObject.FindGameObjectsWithTag("Shovel"))
                {
                    shovel.GetComponent<Shovel>().selected = false;
                }
                foreach(GameObject waste in GameObject.FindGameObjectsWithTag("Waste"))
                {
                    waste.GetComponent<Waste>().selected = false;
                }

                selected = true;
            }
            else
            {
                selected = false;
            }
        }
    }

    // Update calls consecutively
    private void FixedUpdate()
    {
        if (humanConnected != null)
        {
            transform.position = humanConnected.transform.position + (humanConnected.transform.right *= distanceFromHuman);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
