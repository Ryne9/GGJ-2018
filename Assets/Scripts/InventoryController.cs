using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    Inventory inventory;
    int maxResources = 50;
    int maxBlips = 50;

    GameObject canvas;
    GameObject textContainer;

    public RectTransform blips;
    public RectTransform ore1;
    public RectTransform ore2;
    public RectTransform ore3;
    public RectTransform text;

    RectTransform aBlips;

    RectTransform rO1;
    RectTransform bO1;
    RectTransform gO1;

    RectTransform rO2;
    RectTransform bO2;
    RectTransform gO2;

    RectTransform rO3;
    RectTransform bO3;
    RectTransform gO3;

    //Labels
    RectTransform rO1L;
    RectTransform bO1L;
    RectTransform gO1L;
    RectTransform rO2L;
    RectTransform bO2L;
    RectTransform gO2L;
    RectTransform rO3L;
    RectTransform bO3L;
    RectTransform gO3L;
    RectTransform blipsL;

    // Use this for initialization
    void Start()
    {
        inventory = new Inventory(maxResources, maxBlips);

        canvas = GameObject.Find("Canvas");

        textContainer = new GameObject();
        textContainer.transform.SetParent(canvas.transform);
        textContainer.transform.Translate(900, 500, 0);

        int height = 32 / 2;
        float padding = 0.075f;
        float widthScale = 0.75f;
        Vector2 scale = Vector2.one * 0.5f;

        Color red = new Color(1, 0, 0);
        Color blue = new Color(0, 0, 1);
        Color green = new Color(0, 1, 0);
        Color white = new Color(1, 1, 1);

        aBlips = Instantiate(blips, canvas.transform);

        //Init and color images
        rO1 = Instantiate(ore1, canvas.transform);
        bO1 = Instantiate(ore1, canvas.transform);
        gO1 = Instantiate(ore1, canvas.transform);

        rO1.GetComponent<RawImage>().color = red;
        bO1.GetComponent<RawImage>().color = blue;
        gO1.GetComponent<RawImage>().color = green;

        rO2 = Instantiate(ore2, canvas.transform);
        bO2 = Instantiate(ore2, canvas.transform);
        gO2 = Instantiate(ore2, canvas.transform);

        rO2.GetComponent<RawImage>().color = red;
        bO2.GetComponent<RawImage>().color = blue;
        gO2.GetComponent<RawImage>().color = green;

        rO3 = Instantiate(ore3, canvas.transform);
        bO3 = Instantiate(ore3, canvas.transform);
        gO3 = Instantiate(ore3, canvas.transform);

        rO3.GetComponentsInChildren<RawImage>()[0].color = red;
        rO3.GetComponentsInChildren<RawImage>()[1].color = blue;
        bO3.GetComponentsInChildren<RawImage>()[0].color = green;
        bO3.GetComponentsInChildren<RawImage>()[1].color = red;
        gO3.GetComponentsInChildren<RawImage>()[0].color = blue;
        gO3.GetComponentsInChildren<RawImage>()[1].color = green;

        //Position images
        rO1.anchoredPosition = new Vector2(Screen.width * widthScale, 0 - height);
        bO1.anchoredPosition = new Vector2(Screen.width * widthScale, -32 - height);
        gO1.anchoredPosition = new Vector2(Screen.width * widthScale, -64 - height);

        rO2.anchoredPosition = new Vector2(Screen.width * (widthScale + padding), 0 - height);
        bO2.anchoredPosition = new Vector2(Screen.width * (widthScale + padding), -32 - height);
        gO2.anchoredPosition = new Vector2(Screen.width * (widthScale + padding), -64 - height);

        rO3.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2), 0 - height);
        bO3.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2), -32 - height);
        gO3.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2), -64 - height);
        blips.anchoredPosition = new Vector2(Screen.width * 0.95f, -128);

        //Scale images
        rO1.localScale = scale;
        rO2.localScale = scale;
        rO3.localScale = scale;
        bO1.localScale = scale;
        bO2.localScale = scale;
        bO3.localScale = scale;
        gO1.localScale = scale;
        gO2.localScale = scale;
        gO3.localScale = scale;

        //init text
        rO1L = Instantiate(text, canvas.transform);
        bO1L = Instantiate(text, canvas.transform);
        gO1L = Instantiate(text, canvas.transform);
        rO2L = Instantiate(text, canvas.transform);
        bO2L = Instantiate(text, canvas.transform);
        gO2L = Instantiate(text, canvas.transform);
        rO3L = Instantiate(text, canvas.transform);
        bO3L = Instantiate(text, canvas.transform);
        gO3L = Instantiate(text, canvas.transform);
        blipsL = Instantiate(text, canvas.transform);

        //height -= Screen.height / 2 - 5;
        //widthScale += padding * 1.5f;
        //offset += (int) (Screen.width * .1f);
        height += 5;
        int offset = 0;
        widthScale += padding / 2;

        rO1L.anchoredPosition = new Vector2(Screen.width * widthScale + offset, 0 - height);
        bO1L.anchoredPosition = new Vector2(Screen.width * widthScale + offset, -32 - height);
        gO1L.anchoredPosition = new Vector2(Screen.width * widthScale + offset, -64 - height);

        rO2L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding) + offset, 0 - height);
        bO2L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding) + offset, -32 - height);
        gO2L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding) + offset, -64 - height);

        rO3L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2) + offset, 0 - height);
        bO3L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2) + offset, -32 - height);
        gO3L.anchoredPosition = new Vector2(Screen.width * (widthScale + padding * 2) + offset, -64 - height);
        blipsL.anchoredPosition = new Vector2(Screen.width * 0.9f, -64 - height - 50);

        rO1L.GetComponent<Text>().color = white;
        bO1L.GetComponent<Text>().color = white;
        gO1L.GetComponent<Text>().color = white;
        rO2L.GetComponent<Text>().color = white;
        bO2L.GetComponent<Text>().color = white;
        gO2L.GetComponent<Text>().color = white;
        rO3L.GetComponent<Text>().color = white;
        bO3L.GetComponent<Text>().color = white;
        gO3L.GetComponent<Text>().color = white;
        blipsL.GetComponent<Text>().color = white;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddBlips(int blips)
    {
        inventory.AddBlips(blips);
        UpdateUI();
    }

    public int GetBlips()
    {
        return inventory.GetBlips();
    }

    public void AddResource(int resource)
    {
        inventory.AddResource(resource);
        UpdateUI();
    }

    public void RemoveResource(int resourceType)
    {
        inventory.RemoveResource(resourceType);
        UpdateUI();
    }

    public int GetResource(int resource)
    {
        return inventory.GetResourceCount(resource);
    }

    public void UpdateUI()
    {
        rO1L.GetComponent<Text>().text = inventory.GetResourceCount(0).ToString();
        bO1L.GetComponent<Text>().text = inventory.GetResourceCount(1).ToString();
        gO1L.GetComponent<Text>().text = inventory.GetResourceCount(2).ToString();
        rO2L.GetComponent<Text>().text = inventory.GetResourceCount(3).ToString();
        bO2L.GetComponent<Text>().text = inventory.GetResourceCount(4).ToString();
        gO2L.GetComponent<Text>().text = inventory.GetResourceCount(5).ToString();
        rO3L.GetComponent<Text>().text = inventory.GetResourceCount(6).ToString();
        bO3L.GetComponent<Text>().text = inventory.GetResourceCount(7).ToString();
        gO3L.GetComponent<Text>().text = inventory.GetResourceCount(8).ToString();
        blipsL.GetComponent<Text>().text = GetBlips().ToString();
    }
}
