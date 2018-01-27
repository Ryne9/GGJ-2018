using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class MissionController : MonoBehaviour {

    // Use this for initialization
    ArrayList inactiveMissions;
    public int totalMissionCount;
    public int activeMissionCount;
    ArrayList activeMissions;
    public Canvas canvas;

	void Start () {
        totalMissionCount = 15;
        activeMissionCount = 10;
        inactiveMissions = new ArrayList();
        activeMissions = new ArrayList();
        System.Random rnd = new System.Random();
        while (inactiveMissions.Count + activeMissions.Count < totalMissionCount)
        {
            int[] checklist = {rnd.Next(0, 8), rnd.Next(0, 8)};
            Mission newMission = new Mission(checklist);

            if (activeMissions.Count < activeMissionCount)
            {
                activeMissions.Add(newMission);
            } else
            {
                inactiveMissions.Add(newMission);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Mission m in activeMissions)
        {
            m.DecrementTime(Time.deltaTime);
            if (m.GetTimeLeft() <= 0)
            {
                print("YOU'RE OUT OF TIME!!!!");
            }
        }
	}

    public ArrayList GetActiveMissions()
    {
        return activeMissions;
    }

    public ArrayList GetInactiveMissions()
    {
        return inactiveMissions;
    }

    public bool CompleteMission(int[] resources)
    {
        foreach (Mission m in activeMissions)
        {
            if (m.CanBeCompleted(resources) && resources.Length == m.GetChecklist().Length)
            {
                activeMissions.Remove(m);
                if (inactiveMissions.Count > 0)
                {
                    activeMissions.Add(inactiveMissions[0]);
                    inactiveMissions.Remove(inactiveMissions[0]);
                }
                return true;
            }
        }
        return false;
    }
}
