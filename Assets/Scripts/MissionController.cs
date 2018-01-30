using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Assets.Scripts;

public class MissionController : MonoBehaviour
{

    // Use this for initialization
    ArrayList inactiveMissions;
    public int totalMissionCount;
    public int activeMissionCount;
    ArrayList activeMissions;
    public int numFailed;

    public RectTransform headerPrefab;
    public RectTransform ore1Prefab;
    public RectTransform ore2Prefab;
    public RectTransform ore3Prefab;
    public RectTransform timerPrefab;

    private int xOffset = 15;
    private int yOffset = -42;
    private int headerWidth = 128;
    private int headerHeight = 64;
    private int oreWidth = 64;
    private int oreHeight = 64;

    private RectTransform[,] rts;

    void Start()
    {
        totalMissionCount = 15;
        activeMissionCount = 5;
        inactiveMissions = new ArrayList();
        activeMissions = new ArrayList();
        numFailed = 0;
        System.Random rnd = new System.Random();
        while (inactiveMissions.Count + activeMissions.Count < totalMissionCount)
        {
            int[] checklist = { rnd.Next(0, 9), rnd.Next(0, 9) };
            Mission newMission = new Mission(checklist);
            inactiveMissions.Add(newMission);
            
        }

        inactiveMissions.Sort();
        for (int i = 0; i < 5; i++)
        {
            activeMissions.Add(inactiveMissions[0]);
            inactiveMissions.RemoveAt(0);
        }

        rts = new RectTransform[0, 0];

        setMissionUI();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Debug.Log(activeMissions.Count);
        //    CompleteMission(((Mission)activeMissions[0]).GetChecklist());
        //    Debug.Log(activeMissions.Count);
        //}
        bool timeout = false;

        ArrayList toRemove = new ArrayList();

        for (int i = 0; i < activeMissions.Count; i++)
        {
            float scale = ((Mission)activeMissions[i]).GetTimeLeft() / 60f;
            rts[i, 3].localScale = new Vector3(scale, 1, 1);
            rts[i, 3].GetComponent<RawImage>().color = new Color(1f - scale, scale, 0);
        }

        foreach (Mission m in activeMissions)
        {
            m.DecrementTime(Time.deltaTime);
            if (m.GetTimeLeft() <= 0)
            {
                numFailed++;
                toRemove.Add(m);                
            }
        }
        foreach (Mission m in toRemove)
        {
            activeMissions.Remove(m);
            if (inactiveMissions.Count > 0)
            {
                activeMissions.Add(inactiveMissions[0]);
                inactiveMissions.Remove(inactiveMissions[0]);
            }
            timeout = true;
        }
        if (timeout)
            setMissionUI();

        if (activeMissions.Count == 0)
            SceneManager.LoadScene("Game_Over");
    }

    public void setMissionUI()
    {
        if (rts.Length > 0)
            foreach (RectTransform rt in rts) {
                Destroy(rt.gameObject);
            }

        if (activeMissions.Count > 0)
        {
            rts = new RectTransform[activeMissions.Count, 4];

            for (int i = 0; i < activeMissions.Count; i++)
            {
                RectTransform header = Instantiate(headerPrefab);
                header.SetParent(gameObject.transform);

                header.anchoredPosition = new Vector2(xOffset + headerWidth / 2 + i * (headerWidth + 10), yOffset);

                int[] elements = ((Mission)activeMissions[i]).GetChecklist();

                RectTransform leftElement = findElement(elements[0]);
                RectTransform rightElement = findElement(elements[1]);
                RectTransform timer = Instantiate(timerPrefab, gameObject.transform);
                timer.anchoredPosition = new Vector2(xOffset + 10 + headerWidth / 2 + i * (headerWidth + 10), -10);
                leftElement.SetParent(gameObject.transform);
                rightElement.SetParent(gameObject.transform);
                leftElement.anchoredPosition = new Vector2(xOffset + oreWidth / 2 + i * (headerWidth + 10) + 8, yOffset);
                rightElement.anchoredPosition = new Vector2(xOffset + oreWidth / 2 + i * (headerWidth + 10) + 64, yOffset);
                rts[i, 0] = header;
                rts[i, 1] = leftElement;
                rts[i, 2] = rightElement;
                rts[i, 3] = timer;
            }
        }
    }

    public RectTransform findElement(int e)
    {
        RectTransform element;
        Color red = new Color(1, 0, 0);
        Color blue = new Color(0, 0, 1);
        Color green = new Color(0, 1, 0);

        if (e / 3 == 0)
        {
            element = Instantiate(ore1Prefab);
            RawImage image = element.GetComponentInParent<RawImage>();
            switch(e % 3)
            {
                case 0:
                    image.color = red;
                    break;
                case 1:
                    image.color = blue;
                    break;
                case 2:
                    image.color = green;
                    break;
            }
        } else if (e / 3 == 1)
        {
            element = Instantiate(ore2Prefab);
            RawImage image = element.GetComponentInParent<RawImage>();
            image.color = red;
            switch (e % 3)
            {
                case 0:
                    image.color = red;
                    break;
                case 1:
                    image.color = blue;
                    break;
                case 2:
                    image.color = green;
                    break;
            }
        } else {
            element = Instantiate(ore3Prefab);
            RawImage[] images = element.GetComponentsInChildren<RawImage>();
            switch (e % 3)
            {
                case 0:
                    images[0].color = red;
                    images[1].color = blue;
                    break;
                case 1:
                    images[0].color = green;
                    images[1].color = red;
                    break;
                case 2:
                    images[0].color = blue;
                    images[1].color = green;
                    break;
            }
        }

        return element;
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
        bool completed = false;
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
                completed = true;
                break;
            }
        }

        if (completed)
            setMissionUI();

        return completed;
    }
}
