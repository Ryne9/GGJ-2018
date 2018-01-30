using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightController : MonoBehaviour {

    MissionController missionController;
    float totalTime;
    float passedTime;
    Light light;

	// Use this for initialization
	void Start () {
        missionController = GameObject.Find("Canvas").GetComponent<MissionController>();
        totalTime = (missionController.GetActiveMissions().Count + missionController.GetInactiveMissions().Count) * 6f;
        passedTime = 0f;
        light = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        float increment = passedTime / totalTime;
        transform.rotation = Quaternion.Euler(180.0f * increment, 90.0f, 0.0f);
        passedTime += Time.deltaTime;

        if (increment <= .1)
            light.color = new Color(1, 1, increment * 10);
        else if (increment >= .9)
            light.color = new Color(1, 1, (9.1f - 9.0f*increment));
        else
            light.color = Color.white;
	}
}
