using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

    public Text time;
    public int secondFrameCount = 60;
    public int hour;
    public int minute;
    public int second;

    private int frameCounter;

	// Use this for initialization
	void Start () {
        hour = int.Parse(time.text.Split(':')[0]);
        minute = int.Parse(time.text.Split(':')[1]);
        second = int.Parse(time.text.Split(':')[2].Split(' ')[0]);
	}

    // Update called consecutively
    private void FixedUpdate()
    {
        frameCounter++;

        if(frameCounter >= 60)
        {
            second++;
            frameCounter = 0;
        }
        if (second >= 60)
        {
            minute++;
            second = 0;
        }
        if(minute >= 60)
        {
            hour++;
            minute = 0;
        }

        time.text = hour.ToString().PadLeft(2, '0') + ":" + minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0') + time.text.Substring(8);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
