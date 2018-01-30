using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MutatorController : MonoBehaviour, MachineController {

    MenuController menuController;
    GameObject mainCamera;
    InventoryController inventoryManager;
    GameObject canvas;
    int[] input;
    int index = 0;

    public RectTransform miniA;
    public RectTransform miniB;
    public RectTransform miniX;
    public RectTransform miniY;

    // Use this for initialization
    void Start () {
        menuController = GetComponent<MenuController>();
        mainCamera = GameObject.Find("Main Camera");
        inventoryManager = mainCamera.GetComponent<InventoryController>();
        GetComponent<PlayerProximityChecker>().triggerProx = 8;
        input = new int[2];
        canvas = GameObject.Find("Canvas");
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
        if (inventoryManager.GetBlips() > 0)
        {
            switch (button)
            {
                case "A":
                    menuController.Unfocus();
                    break;
                case "X":
                    if (inventoryManager.GetBlips() <= 0 || inventoryManager.GetResource(3) <= 0)
                    {
                        break;
                    }
                    if (index == 0)
                    {
                        input[index] = 3;
                        index++;
                    }
                    else
                    {
                        input[1] = 3;
                        int combined = CombineResources(input[0], input[1]);
                        if (combined >= 0)
                        {
                            inventoryManager.AddBlips(-1);
                            menuController.focusStatus = MenuController.MenuStatus.activate;
                            menuController.cleanupSlate();
                            StartCoroutine("MutatorMiniGame");
                        }
                        else
                        {
                            menuController.Unfocus();
                        }
                        index = 0;
                    }
                    break;
                case "Y":
                    if (inventoryManager.GetBlips() <= 0 || inventoryManager.GetResource(4) <= 0)
                    {
                        break;
                    }
                    inventoryManager.AddBlips(-1);
                    if (index == 0)
                    {
                        input[index] = 4;
                        index++;
                    }
                    else
                    {
                        input[1] = 4;
                        int combined = CombineResources(input[0], input[1]);
                        if (combined >= 0)
                        {
                            inventoryManager.AddBlips(-1);
                            menuController.focusStatus = MenuController.MenuStatus.activate;
                            menuController.cleanupSlate();
                            StartCoroutine("MutatorMiniGame");
                        }
                        else
                        {
                            menuController.Unfocus();
                        }
                        index = 0;
                    }
                    break;
                case "B":
                    if (inventoryManager.GetBlips() <= 0 || inventoryManager.GetResource(5) <= 0)
                    {
                        break;
                    }
                    inventoryManager.AddBlips(-1);
                    if (index == 0)
                    {
                        input[index] = 5;
                        index++;
                    }
                    else
                    {
                        input[1] = 5;
                        int combined = CombineResources(input[0], input[1]);
                        if (combined >= 0)
                        {
                            inventoryManager.AddBlips(-1);
                            menuController.focusStatus = MenuController.MenuStatus.activate;
                            menuController.cleanupSlate();
                            StartCoroutine("MutatorMiniGame");
                        }
                        else
                        {
                            menuController.Unfocus();
                        }
                        index = 0;
                    }
                    break;
            }
        }        
    }

    public int CombineResources(int resource1, int resource2)
    {
        if (resource1 == resource2)
            return -1;
        return resource1 + resource2 - 1;
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

    IEnumerator MutatorMiniGame()
    {
        string[] sequence = new string[6];
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
        inventoryManager.RemoveResource(input[0]);
        inventoryManager.RemoveResource(input[1]);
        inventoryManager.AddResource(input[0] + input[1] - 1);
        menuController.Unfocus();
    }
}
