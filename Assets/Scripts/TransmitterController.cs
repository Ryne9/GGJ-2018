using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class TransmitterController : MonoBehaviour, MachineController {

    enum Row { T0, T1, T2 }

    MenuController menuController;
    GameObject mainCamera;
    GameObject canvas;
    GameObject rings;
    ParticleSystem part;
    InventoryController inventoryController;
    MissionController missionController;
    public int row;
    public int index = 0;
    public int[] resourceSubmission = new int[2];
    public AudioSource source;

	// Use this for initialization
	void Start () {
        row = (int) Row.T0;
        menuController = GetComponent<MenuController>();
        mainCamera = GameObject.Find("Main Camera");
        inventoryController = mainCamera.GetComponent<InventoryController>();
        canvas = GameObject.Find("Canvas");
        rings = GameObject.Find("rings");
        part = rings.GetComponent<ParticleSystem>();
        part.Stop();
        missionController = canvas.GetComponent<MissionController>();
        GetComponent<PlayerProximityChecker>().triggerProx = 11.5f;
        source = GameObject.Find("transmitter").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void UseButton(string button, int row)
    {
        switch (button)
        {
            case "A":
                SubmitResources();
                menuController.Unfocus();              
                break;
            case "X":
                if (inventoryController.GetResource(3 * row) <= 0)
                {
                    break;
                }
                if (index == 0)
                {
                    resourceSubmission[0] = 0 + (3 * row);
                    index++;
                } else
                {
                    resourceSubmission[1] = 0 + (3 * row);
                    menuController.cleanupSlate();
                    SubmitResources();
                }
                break;
            case "Y":
                if (inventoryController.GetResource(3 * row + 1) <= 0)
                {
                    break;
                }
                if (index == 0)
                {
                    resourceSubmission[0] = 1 + (3 * row);
                    index++;
                }
                else
                {
                    resourceSubmission[1] = 1 + (3 * row);
                    menuController.cleanupSlate();
                    SubmitResources();
                }
                break;
            case "B":
                if (inventoryController.GetResource(3 * row + 2) <= 0)
                {
                    break;
                }
                if (index == 0)
                {
                    resourceSubmission[0] = 2 + (3 * row);
                    index++;
                }
                else
                {
                    resourceSubmission[1] = 2 + (3 * row);
                    menuController.cleanupSlate();
                    SubmitResources();
                }
                break;
            case "Up":
                if (row < (int) Row.T2)
                    row++;
                break;
            case "Down":
                if (row > (int)Row.T0)
                    row++;
                break;
        }
    }

    private void SubmitResources()
    {
        index = 0;
        if (missionController.CompleteMission(resourceSubmission))
        {
            inventoryController.RemoveResource(resourceSubmission[0]);
            inventoryController.RemoveResource(resourceSubmission[1]);
            source.PlayOneShot(source.clip);
            StopCoroutine("FireTheLaser");
            StartCoroutine("FireTheLaser");
        }
    }

    IEnumerator FireTheLaser()
    {
        Light packet = GameObject.Find("Packet").GetComponent<Light>();
        packet.range = 20;
        packet.intensity = 0;
        var shape = part.shape;
        part.Play();
        while (shape.angle > 0)
        {
            shape.angle = shape.angle - 0.5f;
            packet.intensity += .25f;

            yield return null;
        }
        float delay = 20;
        while (delay > 0)
        {
            delay -= .2f;
            packet.intensity += Random.Range(-1, 1);
            yield return null;
        }
        part.Stop();
        shape.angle = 80;
    }
}
