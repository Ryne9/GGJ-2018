using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class FabricatorController : MonoBehaviour, MachineController {

    MenuController menuController;
    GameObject mainCamera;
    GameObject canvas;
    InventoryController inventoryManager;
    int coroutineGoal;

    public RectTransform miniA;
    public RectTransform miniB;
    public RectTransform miniX;
    public RectTransform miniY;

    public AudioSource source;

    // Use this for initialization
    void Start () {
        menuController = GetComponent<MenuController>();
        mainCamera = GameObject.Find("Main Camera");
        inventoryManager = mainCamera.GetComponent<InventoryController>();
        GetComponent<PlayerProximityChecker>().triggerProx = 8;
        canvas = GameObject.Find("Canvas");
        source = GameObject.Find("fabric8r").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (menuController.focusStatus == MenuController.MenuStatus.activate)
        {
            //Insert button minigame stuff here.
        }
	}

    public void UseButton(string button, int row)
    {
        switch (button)
        {
            case "A":
                menuController.Unfocus();
                break;
            case "X":
                if (inventoryManager.GetBlips() > 0 && inventoryManager.GetResource(0) > 0)
                {
                    inventoryManager.AddBlips(-1);
                    menuController.focusStatus = MenuController.MenuStatus.activate;
                    coroutineGoal = 0;
                    menuController.cleanupSlate();
                    source.PlayOneShot(source.clip);
                    StartCoroutine("FabricatorMiniGame");
                }           
                break;
            case "Y":
                if (inventoryManager.GetBlips() > 0 && inventoryManager.GetResource(1) > 0)
                {
                    inventoryManager.AddBlips(-1);
                    menuController.focusStatus = MenuController.MenuStatus.activate;
                    coroutineGoal = 1;
                    menuController.cleanupSlate();
                    source.PlayOneShot(source.clip);
                    StartCoroutine("FabricatorMiniGame");
                }
                break;
            case "B":
                if (inventoryManager.GetBlips() > 0 && inventoryManager.GetResource(2) > 0)
                {
                    inventoryManager.AddBlips(-1);
                    menuController.focusStatus = MenuController.MenuStatus.activate;
                    coroutineGoal = 2;
                    menuController.cleanupSlate();
                    source.PlayOneShot(source.clip);
                    StartCoroutine("FabricatorMiniGame");
                }
                break;
        }
    }
    
    IEnumerator FabricatorMiniGame()
    {
        string[] sequence = new string[4];
        System.Random rnd = new System.Random();
        int lastButton = 0;
        for (int i = 0; i < sequence.Length; i++)
        {
            int button = rnd.Next(0, 4);            
            if (i > 0)
                if (lastButton == button)
                    button = (lastButton + 1) % 4;
            lastButton = button;
            if (button == 0)
                sequence[i] = "A";
            else if (button == 1)
                sequence[i] = "B";
            else if (button == 2)
                sequence[i] = "X";
            else
                sequence[i] = "Y";
        }

        string[] seqcop = sequence;
        print(sequence[0] + ", " + sequence[1] + ", " + sequence[2] + ", " + sequence[3]);

        if (menuController.isPlayer1Focused)
        {
            while (Input.GetButton("P1_A") || Input.GetButton("P1_B") || Input.GetButton("P1_X") || Input.GetButton("P1_Y"))
            {
                yield return null;
            }
        }
        else
        {
            while (Input.GetButton("P2_A") || Input.GetButton("P2_B") || Input.GetButton("P2_X") || Input.GetButton("P2_Y"))
            {
                yield return null;
            }
        }

        foreach (string s in sequence)
        {
            RectTransform[] buttons = generateMinigameUI(seqcop, seqcop.Length);

            if (menuController.isPlayer1Focused)
            {
                while (!Input.GetButtonDown("P1_" + s))
                {
                    yield return null;
                }
            }
            else
            {
                while (!Input.GetButtonDown("P2_" + s))
                {
                    yield return null;
                }
            }
            foreach (RectTransform rt in buttons)
                Destroy(rt.gameObject);
            if (seqcop.Length > 0)
            {
                string[] newseqcop = new string[seqcop.Length - 1];
                for (int i = 1; i < seqcop.Length; i++)
                    newseqcop[i - 1] = seqcop[i];
                seqcop = newseqcop;

            }
        }
        inventoryManager.RemoveResource(coroutineGoal);
        inventoryManager.AddResource(coroutineGoal + 3);
        menuController.Unfocus();
        source.PlayOneShot(source.clip);
    }

    private RectTransform[] generateMinigameUI(string[] s, int size)
    {
        RectTransform[] buttons = new RectTransform[size];
        Vector3 screenPos = mainCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        int tenCent = Screen.height / 2;
        int length = size * 50 + (size - 1) * 10;

        for (int i = 0; i < size; i++)
        {
            if (s[i] == "A")
                buttons[i] = Instantiate(miniA, canvas.transform);
            else if (s[i] == "B")
                buttons[i] = Instantiate(miniB, canvas.transform);
            else if (s[i] == "X")
                buttons[i] = Instantiate(miniX, canvas.transform);
            else
                buttons[i] = Instantiate(miniY, canvas.transform);

            buttons[i].anchoredPosition = new Vector2(screenPos.x - length / 2 + (60 * i) + 25, screenPos.y - (Screen.height / 2) - tenCent);

        }

        return buttons;
    }
}
