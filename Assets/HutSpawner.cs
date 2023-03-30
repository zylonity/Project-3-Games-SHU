using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutSpawner : MonoBehaviour
{
    public int minSpawnTime = 2;
    public int maxSpawnTime = 3;
    private float timer;
    public GameObject hut;
    public BackgroundHandler handler;
    public bool canSpawnHut = true;


    void Update()
    {
        if (canSpawnHut == true)
        {
            StartCoroutine(SpawnHut());
        }
    }

    IEnumerator SpawnHut()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
        canSpawnHut = false;
        yield return new WaitForSeconds(timer);
        GameObject tempHut = Instantiate(hut);
        handler.ParentHut(tempHut);
    }


}
