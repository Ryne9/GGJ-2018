using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject corner1;
    public GameObject corner2;

    public List<GameObject> enemies;

    public float spawnTime = 7f;
    //This is the actual timer, but use the spawnTime variable to set the actual spawn time.
    public float timer;
    public int limit;

    //This variable is the radius within an enemy should not spawn if another enemy is present.
    //Feel free to make this zero if you don't care.
    public float safetyRadius;

    public GameObject enemy;
    public GameObject blueEnemy;
    public GameObject redEnemy;

    public AudioSource source;

    System.Random rnd;

    // Use this for initialization
    void Start()
    {
        source = GameObject.Find("Canvas").GetComponent<AudioSource>();

        corner1 = GameObject.Find("spawnCorner1");
        corner2 = GameObject.Find("spawnCorner2");
        timer = spawnTime;

        safetyRadius = 1f;
        limit = 10;

        enemies = new List<GameObject>();

        rnd = new System.Random();

        populate();

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        attemptSpawn();
    }
    
    public void kill(GameObject me)
    {
        enemies.Remove(me);
        Destroy(me);
        // This is where we spawn resource
    }

    void populate()
    {
        for (int i = 0; i <= limit; i++)
        {
            spawn();
        }
    }

    void spawn()
    {
        int result = rnd.Next(0, 3);
        string t = "green";
        GameObject s = enemy;

        switch (result)
        {
            case 0:
                t = "red";
                s = redEnemy;
                break;
            case 1:
                t = "blue";
                s = blueEnemy;
                break;
            case 2:
                t = "green";
                s = enemy;
                break;
        }
        Vector3 potentialSpawn = generateSpawnPoint();
        enemies.Add(Instantiate(s));
        enemies[enemies.Count - 1].transform.position = potentialSpawn;
        enemies[enemies.Count - 1].GetComponent<Enemy_Movement>().spawn = this;
        enemies[enemies.Count - 1].GetComponent<Enemy_Movement>().type = t;
        source.PlayOneShot(source.clip);
    }

    void attemptSpawn()
    {
        if (timer <= 0 && enemies.Count < limit)
        {
            int result = rnd.Next(0, 3);
            string t = "green";
            GameObject s = enemy;

            switch (result)
            {
                case 0:
                    t = "red";
                    s = redEnemy;
                    break;
                case 1:
                    t = "blue";
                    s = blueEnemy;
                    break;
                case 2:
                    t = "green";
                    s = enemy;
                    break;
            }
            Vector3 potentialSpawn = generateSpawnPoint();
            enemies.Add(Instantiate(s));
            enemies[enemies.Count - 1].transform.position = potentialSpawn;
            enemies[enemies.Count - 1].GetComponent<Enemy_Movement>().spawn = this;
            enemies[enemies.Count - 1].GetComponent<Enemy_Movement>().type = t;
            timer = spawnTime;
            source.PlayOneShot(source.clip);
        }
    }

    Vector3 generateSpawnPoint()
    {
        return new Vector3(
            Random.Range(Mathf.Min(getX(corner1), getX(corner2)), Mathf.Max(getX(corner1), getX(corner2))),
            45,
            Random.Range(Mathf.Min(getZ(corner1), getZ(corner2)), Mathf.Max(getZ(corner1), getZ(corner2))));
    }

    // This just gets the X position of a GameObject.
    private float getX(GameObject obj)
    {
        return obj.transform.position.x;
    }

    // This just gets the Z position of a GameObject.
    private float getZ(GameObject obj)
    {
        return obj.transform.position.z;
    }
}
