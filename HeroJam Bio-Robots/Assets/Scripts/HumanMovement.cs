using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * MAKE SURE TO NAME THE TRIGGER FOR THE EDGE AS "EdgeTrigger"
 * MAKE SURE TO NAME THE EXIT FOR THE HUMANS AS "Exit"
 */
public class HumanMovement : MonoBehaviour {

    public GameObject target;
    public GameObject attachedShovel;
    public GameObject edgeTrigger;
    public GameObject exit;
    public Vector3 startPosition;
    public Time startTime;
    public float timeInterval;
    public int pickupFrameTime;
    public int dropFrameTime;
    public int leaveFrameTime;
    public float moveSpeed;
    public float rotateSpeed;

    public string currentState;
    public bool selected;
    public bool selectionEnabled;
    public bool hasShovel;

    private int pickupFrameCounter;
    private int dropFrameCounter;
    private int leaveFrameCounter;

	// Use this for initialization
	void Start () {
        edgeTrigger = GameObject.Find("EdgeTrigger");
        exit = GameObject.Find("Exit");
        if(edgeTrigger == null)
        {
            Debug.Log(gameObject.name + "'s Edge Trigger is null");
        }
        if(exit == null)
        {
            Debug.Log(gameObject.name + "'s Exit is null");
        }
        startPosition = transform.position;
        currentState = "Still";
        selected = false;
        selectionEnabled = true;
        hasShovel = false;
    }

    // Update is called based on call from last frame
    private void FixedUpdate()
    {
        // State machine
        if(currentState == "Still")
        {
            selectionEnabled = true;
        }
        else if(currentState == "Find")
        {
            Find();
        }
        else if(currentState == "Pickup")
        {
            Pickup();

            pickupFrameCounter++;
            if(pickupFrameCounter >= pickupFrameTime)
            {
                currentState = "WalkToEdge";
            }
        }
        else if(currentState == "WalkToEdge")
        {
            WalkToEdge();
        }
        else if(currentState == "Drop")
        {
            Drop();

            dropFrameCounter++;
            if(dropFrameCounter >= dropFrameTime)
            {
                currentState = "Still";
            }
        }
        else if(currentState == "Return")
        {
            Drop();
            Return();

            if(transform.position.x >= startPosition.x - 0.1f && transform.position.x <= startPosition.x + 0.1f
                && transform.position.z >= startPosition.z - 0.1f && transform.position.z <= startPosition.z + 0.1f)
            {
                transform.position = startPosition;
                currentState = "Leave";
                leaveFrameCounter = 0;
            }
        }
        else if(currentState == "Leave")
        {
            leaveFrameCounter++;
            if(leaveFrameCounter >= leaveFrameTime)
            {
                Destroy(gameObject);
            }
        }

        // Selected human and object
        if(selected)
        {
            if(exit.GetComponent<Exit>().selected)
            {
                selected = false;
                selectionEnabled = false;
                exit.GetComponent<Exit>().selected = false;
                currentState = "Return";
            }
            else if(hasShovel)
            {
                foreach (GameObject waste in GameObject.FindGameObjectsWithTag("Waste"))
                {
                    if (waste.GetComponent<Waste>().selected)
                    {
                        target = waste;
                        currentState = "Find";
                        selected = false;
                        selectionEnabled = false;
                        waste.GetComponent<Waste>().selected = false;
                        waste.GetComponent<Waste>().selectionEnabled = false;
                        break;
                    }
                }
            }
            else if(!hasShovel)
            {
                foreach (GameObject shovel in GameObject.FindGameObjectsWithTag("Shovel"))
                {
                    if(shovel.GetComponent<Shovel>().selected)
                    {
                        attachedShovel = shovel;
                        currentState = "Find";
                        selected = false;
                        selectionEnabled = false;
                        shovel.GetComponent<Shovel>().selected = false;
                        shovel.GetComponent<Shovel>().selectionEnabled = false;
                    }
                }
            }
        }
    }

    // Collision
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == attachedShovel && currentState == "Find" && !hasShovel)
        {
            currentState = "Pickup";
            hasShovel = true;
            pickupFrameCounter = 0;
        }
        else if(collider.gameObject == edgeTrigger && currentState == "WalkToEdge")
        {
            currentState = "Drop";
            dropFrameCounter = 0;
        }
        else if (collider.gameObject == target && currentState == "Find" && hasShovel)
        {
            currentState = "Pickup";
            pickupFrameCounter = 0;
        }
    }

    // Clicked on
    private void OnMouseDown()
    {
        if(selectionEnabled)
        {
            if (!selected)
            {
                foreach (GameObject human in GameObject.FindGameObjectsWithTag("Human"))
                {
                    human.GetComponent<HumanMovement>().selected = false;
                }

                selected = true;
            }
            else
            {
                selected = false;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        
	}

    // Method for sending the human to the target and starting the timer
    public void Find()
    {
        if(hasShovel)
        {
            Quaternion rotateQuaternion = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

            transform.Translate(Vector3.forward * moveSpeed);
        }
        else
        {
            Quaternion rotateQuaternion = Quaternion.LookRotation(attachedShovel.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

            transform.Translate(Vector3.forward * moveSpeed);
        }
    }

    // Method for picking up the target
    public void Pickup()
    {
        if(hasShovel)
        {
            target.GetComponent<Waste>().humanConnected = gameObject;
        }
        else
        {
            attachedShovel.GetComponent<Shovel>().humanConnected = gameObject;
        }
    }

    // Method for sending the human to throw away the waste off the edge
    public void WalkToEdge()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(edgeTrigger.transform.position.x, transform.position.y, transform.position.z) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

        transform.Translate(Vector3.forward * moveSpeed);
    }

    // Method for dropping the waste
    public void Drop()
    {
        if(target != null)
        {
            target.GetComponent<Waste>().humanConnected = null;
            target.GetComponent<Rigidbody>().useGravity = true;
        }
        if(currentState == "Return" && attachedShovel != null)
        {
            attachedShovel = null;
            attachedShovel.GetComponent<Shovel>().humanConnected = null;
            attachedShovel.GetComponent<Shovel>().selectionEnabled = true;
        }
    }

    // Method for returning the human back to starting position
    public void Return()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(startPosition - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

        transform.Translate(Vector3.forward * moveSpeed);
    }
}
