using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Code by: David Liu
 */
public class Score : MonoBehaviour {

    public Text endText;
    public int humansSpawned;
    public int humansPoisoned;

    private int debrisRemaining;
    private int humansRemaining;

	// Use this for initialization
	void Start () {
        debrisRemaining = GameObject.FindGameObjectsWithTag("Waste").Length;
        humansRemaining = GameObject.FindGameObjectsWithTag("Human").Length;
	}
	
	// Update is called once per frame
	void Update () {
        debrisRemaining = GameObject.FindGameObjectsWithTag("Waste").Length;
        humansRemaining = GameObject.FindGameObjectsWithTag("Human").Length;

        if (debrisRemaining == 0 && humansRemaining == 0)
        {
            endText.text = "Roof Cleared!\n\nLiquidators Used: " + humansSpawned + "\nLiquidators Poisoned: " + humansPoisoned;
            endText.enabled = true;
        }
    }
}
