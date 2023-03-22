using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rat_prefab;
    [SerializeField] private Transform player_transform;
    private Transform mouse_transform;
    // Start is called before the first frame update
    void Start()
    {
        mouse_transform = rat_prefab.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 2) == 1)
            SpawnRat();
    }
    void SpawnRat()
    {
        mouse_transform.position = new UnityEngine.Vector3(0, 0, 0);
        if (Random.Range(0, 1) == 0)
            mouse_transform.position = new UnityEngine.Vector3(player_transform.position.x, player_transform.position.y, 0);
        else
            mouse_transform.position = new UnityEngine.Vector3(player_transform.position.x, player_transform.position.y, 0);
        Instantiate(rat_prefab, mouse_transform);
    }
}
