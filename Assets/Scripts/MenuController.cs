using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public enum MenuStatus { unfocused, startPrompt, menu, activate };

    public bool isPlayer1Focused;
    public bool isPlayer2Focused;
    public MenuStatus focusStatus;
    public float focusProx;
    public bool isFocused;
    public GameObject player1;
    public GameObject player2;
    public Camera camera;
    public RectTransform aPrefab;
    public RectTransform aActual;

    public RectTransform o1Prefab;
    public RectTransform o2Prefab;
    public RectTransform o3Prefab;

    RectTransform o1l;
    RectTransform o1m;
    RectTransform o1r;

    public RectTransform slatePrefab;
    public RectTransform actualSlate;
    public RectTransform upPrefab;
    public RectTransform downPrefab;

    RectTransform aUp;
    RectTransform aDown;

    public bool slate = false;

    public MachineController machineController;

    public bool aButton = false;
    public bool grow = true;

    public float tenCent = Screen.height / 10f;

    public Color red = new Color(1, 0, 0);
    public Color green = new Color(0, 1, 0);
    public Color blue = new Color(0, 0, 1);

    private int row = 0;
    private bool rows;

    private bool arrowPressed;

    // Use this for initialization
    void Start()
    {
        isFocused = false;
        focusStatus = MenuStatus.unfocused;
        machineController = gameObject.GetComponent<MachineController>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Debug.Log("X: " + Screen.width + " Y: " + Screen.height);
        arrowPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (focusStatus)
        {
            case MenuStatus.startPrompt:
                if ((isPlayer1Focused || isPlayer2Focused) && !aButton)
                {
                    Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
                    aActual = Instantiate(aPrefab, GameObject.Find("Canvas").GetComponent<Transform>());
                    Debug.Log("X: " + screenPos.x + " Y: " + screenPos.y + " Z: " + screenPos.z);
                    aActual.anchoredPosition = new Vector2(screenPos.x - (Screen.width / 2), screenPos.y - (Screen.height / 2) - tenCent);
                    aButton = true;
                }
                if (aButton)
                {
                    RawImage img = aActual.GetComponent<RawImage>();
                    img.transform.localScale += (grow ? 0.05f : -0.05f) * Vector3.one;
                    if (img.transform.localScale.x > 2 || img.transform.localScale.x < 0.75f)
                        grow = !grow;
                }
                if (isPlayer1Focused)
                    if (Input.GetButtonDown("P1_A"))
                    {
                        focusStatus = MenuStatus.menu;
                        isPlayer2Focused = false;
                        player1.GetComponent<Player1_Movement>().canMove = false;
                        player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        player1.GetComponentInChildren<Player1_Combat>().canFire = false;
                        Destroy(aActual.gameObject);
                        aButton = false;
                        break;
                    }
                if (isPlayer2Focused)
                    if (Input.GetButtonDown("P2_A"))
                    {
                        focusStatus = MenuStatus.menu;
                        isPlayer1Focused = false;
                        player2.GetComponent<Player2_Movement>().canMove = false;
                        player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        player2.GetComponentInChildren<Player2_Combat>().canFire = false;
                        Destroy(aActual.gameObject);
                        aButton = false;
                    }
                if (!isPlayer1Focused && !isPlayer2Focused && aButton)
                {
                    Destroy(aActual.gameObject);
                    aButton = false;
                }
                if (slate)
                    cleanupSlate();
                break;
            case MenuStatus.menu:
                if (!slate)
                { //initializes the slates
                    row = 0;
                    rows = false;
                    Vector3 screen = camera.WorldToScreenPoint(transform.position);
                    actualSlate = Instantiate(slatePrefab, GameObject.Find("Canvas").GetComponent<Transform>());
                    actualSlate.anchoredPosition = new Vector2(screen.x, screen.y - Screen.height * .9f);
                    if (gameObject.name.Equals("fabric8r"))
                    {
                        o1l = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                        o1m = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                        o1r = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                    }
                    else if (gameObject.name.Equals("mut8r"))
                    {
                        o1l = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                        o1m = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                        o1r = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                    }
                    else
                    {
                        rows = true;
                        UpdateRow();
                        slate = true;
                        Debug.Log(rows + " " + slate);
                        break;
                    }

                    int midYOffset = 11;
                    int sideYOffset = -10;
                    int sideXOffset = 72;

                    o1l.anchoredPosition = new Vector2(screen.x - sideXOffset, screen.y - Screen.height * .9f + sideYOffset);
                    o1m.anchoredPosition = new Vector2(screen.x, screen.y - Screen.height * .9f + midYOffset);
                    o1r.anchoredPosition = new Vector2(screen.x + sideXOffset, screen.y - Screen.height * .9f + sideYOffset);

                    o1l.localScale = Vector3.one * .8f;
                    o1m.localScale = Vector3.one * .8f;
                    o1r.localScale = Vector3.one * .8f;

                    o1l.GetComponent<RawImage>().color = red;
                    o1m.GetComponent<RawImage>().color = blue;
                    o1r.GetComponent<RawImage>().color = green;
                    slate = true;
                }

                if (isPlayer1Focused)
                {
                    if (Input.GetButtonDown("P1_X"))
                    {
                        machineController.UseButton("X", row);
                    }
                    else if (Input.GetButtonDown("P1_Y"))
                    {
                        machineController.UseButton("Y", row);
                    }
                    else if (Input.GetButtonDown("P1_B"))
                    {
                        machineController.UseButton("B", row);
                    }
                    else if (Input.GetButtonDown("P1_A"))
                    {
                        machineController.UseButton("A", row);
                    }
                    else if (Input.GetAxisRaw("P1_DPadVertical") == -1 && !arrowPressed)
                    {
                        if (rows)
                            handleRows(1);
                        machineController.UseButton("Up", row);
                        arrowPressed = true;
                    }
                    else if (Input.GetAxisRaw("P1_DPadVertical") == 1 && !arrowPressed)
                    {
                        if (rows)
                            handleRows(-1);
                        machineController.UseButton("Down", row);
                        arrowPressed = true;
                    }
                    else if (Input.GetAxisRaw("P1_DPadVertical") == 0 && arrowPressed)
                    {
                        arrowPressed = false;
                    }

                }
                else
                {
                    if (Input.GetButtonDown("P2_X"))
                    {
                        machineController.UseButton("X", row);
                    }
                    else if (Input.GetButtonDown("P2_Y"))
                    {
                        machineController.UseButton("Y", row);
                    }
                    else if (Input.GetButtonDown("P2_B"))
                    {
                        machineController.UseButton("B", row);
                    }
                    else if (Input.GetButtonDown("P2_A"))
                    {
                        machineController.UseButton("A", row);
                    }
                    else if (Input.GetAxisRaw("P2_DPadVertical") == -1 && !arrowPressed)
                    {
                        if (rows)
                            handleRows(1);
                        machineController.UseButton("Up", row);
                        arrowPressed = true;
                    }
                    else if (Input.GetAxisRaw("P2_DPadVertical") == 1 && !arrowPressed)
                    {
                        if (rows)
                            handleRows(-1);
                        machineController.UseButton("Down", row);
                        arrowPressed = true;
                    }
                    else if (Input.GetAxisRaw("P2_DPadVertical") == 0 && arrowPressed)
                    {
                        arrowPressed = false;
                    }
                }
                break;
            case MenuStatus.activate:
                break;
        }
    }

    private void handleRows(int r)
    {
        if (r < 0 && row > 0)
        {
            row -= 1;
            UpdateRow();
        }

        if (r > 0 && row < 2)
        {
            row += 1;
            UpdateRow();
        }
    }

    public void cleanupSlate()
    {
        if (o1l != null)
        {
            Destroy(o1l.gameObject);
            Destroy(o1m.gameObject);
            Destroy(o1r.gameObject);
        }
        if (actualSlate != null)
            Destroy(actualSlate.gameObject);
        if (aUp)
            Destroy(aUp.gameObject);
        if (aDown)
            Destroy(aDown.gameObject);
        slate = false;
    }

    private void UpdateRow()
    {
        Vector3 screen = camera.WorldToScreenPoint(transform.position);

        if (o1l != null)
        {
            Destroy(o1l.gameObject);
            Destroy(o1m.gameObject);
            Destroy(o1r.gameObject);
        }

        if (aUp)
            Destroy(aUp.gameObject);
        if (aDown)
            Destroy(aDown.gameObject);

        if (row == 0 || row == 1)
        {
            if (row == 0)
            {
                o1l = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                o1m = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                o1r = Instantiate(o1Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                aUp = Instantiate(upPrefab, GameObject.Find("Canvas").GetComponent<Transform>());
                aUp.anchoredPosition = actualSlate.anchoredPosition;
            }

            if (row == 1)
            {
                o1l = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                o1m = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                o1r = Instantiate(o2Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
                aUp = Instantiate(upPrefab, GameObject.Find("Canvas").GetComponent<Transform>());
                aDown = Instantiate(downPrefab, GameObject.Find("Canvas").GetComponent<Transform>());
                aDown.anchoredPosition = actualSlate.anchoredPosition;
                aUp.anchoredPosition = actualSlate.anchoredPosition;
            }

            int midYOffset = 11;
            int sideYOffset = -10;
            int sideXOffset = 72;

            o1l.anchoredPosition = new Vector2(screen.x - sideXOffset, screen.y - Screen.height * .9f + sideYOffset);
            o1m.anchoredPosition = new Vector2(screen.x, screen.y - Screen.height * .9f + midYOffset);
            o1r.anchoredPosition = new Vector2(screen.x + sideXOffset, screen.y - Screen.height * .9f + sideYOffset);

            o1l.localScale = Vector3.one * .8f;
            o1m.localScale = Vector3.one * .8f;
            o1r.localScale = Vector3.one * .8f;

            o1l.GetComponent<RawImage>().color = red;
            o1m.GetComponent<RawImage>().color = blue;
            o1r.GetComponent<RawImage>().color = green;
        }
        else
        {
            o1l = Instantiate(o3Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
            o1m = Instantiate(o3Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
            o1r = Instantiate(o3Prefab, GameObject.Find("Canvas").GetComponent<Transform>());
            aDown = Instantiate(downPrefab, GameObject.Find("Canvas").GetComponent<Transform>());
            aDown.anchoredPosition = actualSlate.anchoredPosition;

            int midYOffset = 11;
            int sideYOffset = -10;
            int sideXOffset = 72;

            o1l.anchoredPosition = new Vector2(screen.x - sideXOffset, screen.y - Screen.height * .9f + sideYOffset);
            o1m.anchoredPosition = new Vector2(screen.x, screen.y - Screen.height * .9f + midYOffset);
            o1r.anchoredPosition = new Vector2(screen.x + sideXOffset, screen.y - Screen.height * .9f + sideYOffset);

            o1l.localScale = Vector3.one * .8f;
            o1m.localScale = Vector3.one * .8f;
            o1r.localScale = Vector3.one * .8f;

            o1l.GetComponentsInChildren<RawImage>()[0].color = red;
            o1l.GetComponentsInChildren<RawImage>()[1].color = blue;
            o1m.GetComponentsInChildren<RawImage>()[0].color = green;
            o1m.GetComponentsInChildren<RawImage>()[1].color = red;
            o1r.GetComponentsInChildren<RawImage>()[0].color = blue;
            o1r.GetComponentsInChildren<RawImage>()[1].color = green;
        }

    }

    public void PlayerAlert(bool player1, bool player2)
    {
        if (focusStatus == MenuStatus.unfocused || focusStatus == MenuStatus.startPrompt)
        {
            isPlayer1Focused = player1;
            isPlayer2Focused = player2;
        }

        if ((isPlayer1Focused || isPlayer2Focused) && focusStatus == MenuStatus.unfocused)
            focusStatus = MenuStatus.startPrompt;
    }

    public int GetStatus()
    {
        return (int)focusStatus;
    }

    public void SetPlayers(GameObject player1, GameObject player2)
    {
        this.player1 = player1;
        this.player2 = player2;
    }

    public void Unfocus()
    {
        focusStatus = MenuStatus.unfocused;
        if (isPlayer1Focused)
        {
            player1.GetComponent<Player1_Movement>().canMove = true;
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            player1.GetComponentInChildren<Player1_Combat>().canFire = true;
        }
        if (isPlayer2Focused)
        {
            player2.GetComponent<Player2_Movement>().canMove = true;
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            player2.GetComponentInChildren<Player2_Combat>().canFire = true;
        }
    }
}
