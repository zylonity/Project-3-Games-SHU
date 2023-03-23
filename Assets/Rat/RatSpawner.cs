using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float probability;
    [SerializeField] private float spawnProbTime;
    [SerializeField] private int maxDistance;

    private int probability_max = 0;
    private Transform mouse_transform;
    float spawn_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        probability_max = (int)(1.0 / probability) + 1;
        mouse_transform = ratPrefab.transform;
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
        float random_distance = Random.Range(0, maxDistance + 1);
        if (Random.Range(0, 2) == 0)
            mouse_transform.position = new UnityEngine.Vector3(playerTransform.position.x - random_distance, playerTransform.position.y, 0);
        else
            mouse_transform.position = new UnityEngine.Vector3(playerTransform.position.x + random_distance, playerTransform.position.y, 0);
        ratPrefab.GetComponent<RatController>().playerTransform = playerTransform;
        Instantiate(ratPrefab, mouse_transform);
    }
}
