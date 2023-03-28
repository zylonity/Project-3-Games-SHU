using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class RatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float probability;
    [SerializeField] private float spawnProbTime;
    [SerializeField, Range(0, 5)]  private float minDistance = 5.0f;
    [SerializeField, Range(5, 12)] private float maxDistance = 10.0f;

    private int probability_max = 0;
    float spawn_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        probability_max = (int)(1.0 / probability) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        spawn_timer += Time.deltaTime;
        if (spawn_timer > spawnProbTime)
        {
            spawn_timer = 0;
            if (Random.Range(0, probability_max) == 0)
                SpawnRat();
        }
    }
    void SpawnRat()
    {
        UnityEngine.Vector3 spawn_pos = new UnityEngine.Vector3(0,0,0);
        float random_distance = Random.Range(minDistance, maxDistance + 1);
        if (Random.Range(0, 2) == 0)
            spawn_pos = new UnityEngine.Vector3(playerTransform.position.x - random_distance, -4, 0);
        else
            spawn_pos = new UnityEngine.Vector3(playerTransform.position.x + random_distance, -4, 0);
        Instantiate(ratPrefab, spawn_pos, new UnityEngine.Quaternion(0,0,0,0), transform);
    }
}
