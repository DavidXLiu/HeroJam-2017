using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Code by: David Liu
 */
public class SpawnHuman : MonoBehaviour {

    public GameObject human;
    public GameObject spawnPosition;

	// Use this for initialization
	void Start () {

    }

    // Triggers when clicked
    private void OnMouseDown()
    {
        Instantiate(human, spawnPosition.transform.position, Quaternion.identity);
        GameObject.Find("EventSystem").GetComponent<Score>().humansSpawned++;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
