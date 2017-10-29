using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {

    public Text instructions;
    public Text startInfo;
    public Text title;
    public GameObject startButton;

    private bool isInstructions;

	// Use this for initialization
	void Start () {
        isInstructions = false;
	}

    public void ToggleInstructions()
    {
        if(isInstructions)
        {
            startInfo.enabled = true;
            title.enabled = true;
            startButton.SetActive(true);
            instructions.enabled = false;

            isInstructions = false;
        }
        else
        {
            instructions.enabled = true;
            startInfo.enabled = false;
            title.enabled = false;
            startButton.SetActive(false);

            isInstructions = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
