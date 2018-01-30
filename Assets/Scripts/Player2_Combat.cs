using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_Combat : MonoBehaviour
{

    LineRenderer line;
    ParticleSystem particles;

    AudioSource source;

    public bool canFire;

    // Use this for initialization
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
        particles = gameObject.GetComponent<ParticleSystem>();
        particles.Stop();
        canFire = true;
        source = GameObject.Find("Player2").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire && Input.GetButtonDown("P2_X"))
        {
            source.PlayOneShot(source.clip);
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }

    IEnumerator FireLaser()
    {

        line.enabled = true;
        particles.Play();
        while (canFire && Input.GetButton("P2_X"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            line.SetPosition(0, ray.origin);

            if (Physics.Raycast(ray, out hit, 15, 1 << 8))
            {
                hit.collider.gameObject.GetComponent<Enemy_Movement>().takeDamage(1);
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.SetPosition(1, ray.GetPoint(15));
            }

            yield return null;
        }

        line.enabled = false;
        particles.Stop();
    }
}
