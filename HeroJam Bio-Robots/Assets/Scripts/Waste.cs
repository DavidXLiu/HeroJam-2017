using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Class that describes my life
 */
public class Waste : MonoBehaviour {

    public GameObject humanConnected;
    public float distanceFromHuman;
    public float yPositionForDespawn;

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
        if(selectionEnabled)
        {
            if (!selected)
            {
                foreach (GameObject waste in GameObject.FindGameObjectsWithTag("Waste"))
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
        if(humanConnected != null)
        {
            transform.position = humanConnected.transform.position + (humanConnected.transform.forward *= distanceFromHuman);
        }

        if(transform.position.y <= yPositionForDespawn)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
