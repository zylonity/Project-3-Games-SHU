using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform = null;
    Vector3 standart_camera_pos;
    // Start is called before the first frame update
    void Awake()
    {
        standart_camera_pos = transform.position;
    }
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 new_transform = playerTransform.position;
            if (new_transform.y < standart_camera_pos.y)
            {
                new_transform.y = standart_camera_pos.y;
            }
            new_transform.z = standart_camera_pos.z;
            transform.position = new_transform;
        }
    }
}
