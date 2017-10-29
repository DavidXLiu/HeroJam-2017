using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Code by: David Liu, Tiger Louck
 * MAKE SURE TO NAME THE TRIGGER FOR THE EDGE AS "EdgeTrigger"
 * MAKE SURE TO NAME THE EXIT FOR THE HUMANS AS "Exit"
 * Waste must be tagged as "Waste"
 * Shovels must be tagged as "Shovel"
 * Leave triggers must be tagged as "LeaveTrigger"
 * Leave triggers must be named in order with the number at the end of the name. Starts with 0
 */
public class HumanMovement : MonoBehaviour {

    public GameObject target;
    public GameObject attachedShovel;
    public GameObject attachedHuman;
    public GameObject edgeTrigger;
    public GameObject[] leaveTriggers;
    public GameObject exit;
    public Vector3 startPosition;
    public TextMesh timeText;
    public float startTime;
    public float timeInterval;
    public int pickupFrameTime;
    public int dropFrameTime;
    public int leaveFrameTime;
    public float moveSpeed;
    public float rotateSpeed;
    public float distanceFromHuman;

    public GameObject currentLeaveTrigger;
    public string currentState;
    public bool selected;
    public bool selectionEnabled;
    public bool hasShovel;

    private GameObject[] tempArray;
    private int pickupFrameCounter;
    private int dropFrameCounter;
    private int leaveFrameCounter;

	// Use this for initialization
	void Start () {
        edgeTrigger = GameObject.Find("EdgeTrigger");
        exit = GameObject.Find("Exit");
        leaveTriggers = GameObject.FindGameObjectsWithTag("LeaveTrigger");
        tempArray = new GameObject[leaveTriggers.Length];
        startTime = Time.time;
        timeText.text = GameObject.Find("Time").GetComponent<Text>().text.Substring(0, 8);
        for(int i = 0; i < leaveTriggers.Length; i++)
        {
            for(int j = 0; j < leaveTriggers.Length; j++)
            {
                if(leaveTriggers[j].name.EndsWith(i.ToString()))
                {
                    tempArray[i] = leaveTriggers[j];
                }
            }
        }
        leaveTriggers = tempArray;
        if(edgeTrigger == null)
        {
            Debug.Log(gameObject.name + "'s Edge Trigger is null");
        }
        if(exit == null)
        {
            Debug.Log(gameObject.name + "'s Exit is null");
        }
        if(leaveTriggers.Length == 0)
        {
            Debug.Log(gameObject.name + " has no Leave Triggers");
        }
        startPosition = transform.position;
        currentState = "Enter";
        GetComponent<Animator>().SetBool("idle", true);
        selected = false;
        selectionEnabled = false;
        hasShovel = false;
        GetComponent<Animator>().SetBool("HasShovel", false);
        currentLeaveTrigger = leaveTriggers[leaveTriggers.Length - 1];
    }

    // Update is called based on call from last frame
    private void FixedUpdate()
    {
        // State machine
        if(currentState == "Enter")
        {
            Enter();
            GetComponent<Animator>().SetTrigger("Walk");

            if (transform.position.x >= currentLeaveTrigger.transform.position.x - 0.25f && transform.position.x <= currentLeaveTrigger.transform.position.x + 0.25f
                && transform.position.z >= currentLeaveTrigger.transform.position.z - 0.25f && transform.position.z <= currentLeaveTrigger.transform.position.z + 0.25f)
            {
                if (leaveTriggers[0] == currentLeaveTrigger)
                {
                    currentState = "Still";
                    selectionEnabled = true;
                }
                else
                {
                    for (int i = leaveTriggers.Length - 1; i >= 0; i--)
                    {
                        if (leaveTriggers[i] == currentLeaveTrigger)
                        {
                            currentLeaveTrigger = leaveTriggers[i - 1];
                            break;
                        }
                    }
                }
            }
        }
        else if(currentState == "Still")
        {
            selectionEnabled = true;
        }
        else if(currentState == "Find")
        {
            Find();
            GetComponent<Animator>().SetTrigger("Walk");
        }
        else if(currentState == "Pickup")
        {
            GetComponent<Animator>().SetTrigger("PickUp");
            pickupFrameCounter++;
            if(pickupFrameCounter >= pickupFrameTime)
            {
                if(hasShovel)
                {
                    Pickup();
                    currentState = "WalkToEdge";
                }
                else
                {
                    Pickup();
                    currentState = "Still";
                }
            }
        }
        else if(currentState == "WalkToEdge")
        {
            WalkToEdge();
            GetComponent<Animator>().SetTrigger("Walk");
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
            GetComponent<Animator>().SetTrigger("Walk");

            if (transform.position.x >= currentLeaveTrigger.transform.position.x - 0.25f && transform.position.x <= currentLeaveTrigger.transform.position.x + 0.25f
                && transform.position.z >= currentLeaveTrigger.transform.position.z - 0.25f && transform.position.z <= currentLeaveTrigger.transform.position.z + 0.25f)
            {
                if(leaveTriggers[leaveTriggers.Length - 1] == currentLeaveTrigger)
                {
                    currentState = "Leave";
                    leaveFrameCounter = 0;
                }
                else
                {
                    for(int i = 0; i < leaveTriggers.Length; i++)
                    {
                        if(leaveTriggers[i] == currentLeaveTrigger)
                        {
                            currentLeaveTrigger = leaveTriggers[i + 1];
                            break;
                        }
                    }
                }
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
        else if (currentState == "Unconscious")
        {
            Drop();
        }
        else if(currentState == "Rescued")
        {
            if (attachedHuman == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.rotation = attachedHuman.transform.rotation;
            transform.position = attachedHuman.transform.position + (attachedHuman.transform.forward *= distanceFromHuman);
        }
        else if(currentState == "Rescuing")
        {
            Drop();
            Rescuing();
            GetComponent<Animator>().SetTrigger("Walk");

            if (transform.position.x >= currentLeaveTrigger.transform.position.x - 0.25f && transform.position.x <= currentLeaveTrigger.transform.position.x + 0.25f
                && transform.position.z >= currentLeaveTrigger.transform.position.z - 0.25f && transform.position.z <= currentLeaveTrigger.transform.position.z + 0.25f)
            {
                if (leaveTriggers[leaveTriggers.Length - 1] == currentLeaveTrigger)
                {
                    currentState = "Leave";
                    leaveFrameCounter = 0;
                }
                else
                {
                    for (int i = 0; i < leaveTriggers.Length; i++)
                    {
                        if (leaveTriggers[i] == currentLeaveTrigger)
                        {
                            currentLeaveTrigger = leaveTriggers[i + 1];
                            break;
                        }
                    }
                }
            }
        }

        // Shovel animation
        if(hasShovel)
        {
            GetComponent<Animator>().SetBool("HasShovel", true);
        }

        //pass-out timer
        if (Time.time >= startTime + timeInterval && currentState != "Unconscious" && currentState != "Rescued" && currentState != "Rescuing" && currentState != "Leave")
        {
            currentState = "Unconscious";
            GameObject.Find("EventSystem").GetComponent<Score>().humansPoisoned++;
            selectionEnabled = true;
            GetComponent<Animator>().SetTrigger("Pass out");
        }

        // Selected human and object
        if(selected && currentState != "Unconscious")
        {
            if(exit.GetComponent<Exit>().selected)
            {
                selected = false;
                selectionEnabled = false;
                exit.GetComponent<Exit>().selected = false;
                currentState = "Return";
                currentLeaveTrigger = leaveTriggers[0];
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
        else if(selected && currentState == "Unconscious")
        {
            foreach(GameObject human in GameObject.FindGameObjectsWithTag("Human"))
            {
                if (human.GetComponent<HumanMovement>().selected && human != gameObject && human.GetComponent<HumanMovement>().currentState != "Unconcscious")
                {
                    attachedHuman = human;
                    human.GetComponent<HumanMovement>().attachedHuman = gameObject;
                    human.GetComponent<HumanMovement>().currentState = "Find";
                    human.GetComponent<HumanMovement>().currentLeaveTrigger = human.GetComponent<HumanMovement>().leaveTriggers[0];
                    selected = false;
                    selectionEnabled = false;
                    human.GetComponent<HumanMovement>().selected = false;
                    human.GetComponent<HumanMovement>().selectionEnabled = false;
                    human.GetComponent<HumanMovement>().Drop();
                }
            }
        }
    }

    // Collision
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == attachedHuman && currentState == "Find")
        {
            currentState = "Rescuing";
            attachedHuman.GetComponent<HumanMovement>().currentState = "Rescued";
        }
        else if(collider.gameObject == attachedShovel && currentState == "Find" && !hasShovel)
        {
            currentState = "Pickup";
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
            if (!selected && currentState != "Unconscious")
            {
                foreach (GameObject human in GameObject.FindGameObjectsWithTag("Human"))
                {
                    human.GetComponent<HumanMovement>().selected = false;
                }

                selected = true;
            }
            else if(!selected && currentState == "Unconscious")
            {
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
        if(attachedHuman != null)
        {
            Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(attachedHuman.transform.position.x, transform.position.y, attachedHuman.transform.position.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

            transform.Translate(Vector3.forward * moveSpeed);
        }
        else if(hasShovel)
        {
            Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

            transform.Translate(Vector3.forward * moveSpeed);
        }
        else
        {
            Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(attachedShovel.transform.position.x, transform.position.y, attachedShovel.transform.position.z) - transform.position);
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
            hasShovel = true;
            GetComponent<Animator>().SetBool("HasShovel", true);
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
        if(target != null && currentState == "Unconscious")
        {
            target.GetComponent<Waste>().humanConnected = null;
            target.GetComponent<Waste>().selectionEnabled = true;
        }
        else if(target != null)
        {
            target.GetComponent<Waste>().humanConnected = null;
            target.GetComponent<Rigidbody>().useGravity = true;
        }
        if((currentState == "Return" || currentState == "Rescuing" || currentState == "Unconscious") && attachedShovel != null)
        {
            attachedShovel.GetComponent<Shovel>().humanConnected = null;
            attachedShovel.GetComponent<Shovel>().selectionEnabled = true;
            attachedShovel = null;
            hasShovel = false;
        }
        else if(attachedHuman != null)
        {
            if(hasShovel)
            {
                attachedShovel.GetComponent<Shovel>().humanConnected = null;
                attachedShovel.GetComponent<Shovel>().selectionEnabled = true;
                attachedShovel = null;
                hasShovel = false;
            }
        }
    }

    // Method for returning the human back to starting position
    public void Return()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(currentLeaveTrigger.transform.position.x, transform.position.y, currentLeaveTrigger.transform.position.z) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

        transform.Translate(Vector3.forward * moveSpeed);
    }

    public void Rescuing()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(currentLeaveTrigger.transform.position.x, transform.position.y, currentLeaveTrigger.transform.position.z) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

        transform.Translate(Vector3.forward * moveSpeed);
    }

    public void Enter()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(new Vector3(currentLeaveTrigger.transform.position.x, transform.position.y, currentLeaveTrigger.transform.position.z) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateQuaternion, rotateSpeed);

        transform.Translate(Vector3.forward * moveSpeed);
    }
}
