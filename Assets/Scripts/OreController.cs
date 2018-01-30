using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreController : MonoBehaviour {

    public string type;
    public int typeNum;

    public InventoryController cont;

	// Use this for initialization
	void Start () {

        cont = GameObject.Find("Main Camera").GetComponent<InventoryController>();

		if (type == null)
        {
            type = "blue";
        }
        Material m_Material = GetComponent<Renderer>().material;
        switch(type)
        {
            case "blue":
                m_Material.color = Color.blue;
                typeNum = 1;
                break;
            case "green":
                m_Material.color = Color.green;
                typeNum = 2;
                break;
            case "red":
                m_Material.color = Color.red;
                typeNum = 0;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            cont.AddResource(typeNum);
            Destroy(gameObject);
        }
    }
}
