using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutDetector : MonoBehaviour
{

    public HutSpawner hSpawn;

    private void Start()
    {
        hSpawn = GameObject.Find("Test").transform.Find("HutSpawner").GetComponent<HutSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            hSpawn.canSpawnHut = true;
            Destroy(this.gameObject);
        }
    }
}
