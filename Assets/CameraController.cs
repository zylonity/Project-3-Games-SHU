using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform = null;
    private GameController gm = null;
    public static CameraController _camcont = null;
    private Camera _camera = null;
    Vector3 standart_camera_pos;
    private float standart_camera_size;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
        _camcont = this;
        standart_camera_pos = this.transform.position;
        standart_camera_size = _camera.orthographicSize;
    }
    private void Start()
    {
        gm = GameController._gameController;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm != null && playerTransform != null)
        {
            Vector3 new_transform = playerTransform.position;
            if (new_transform.y < standart_camera_pos.y)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, standart_camera_size, 0.001f);
                new_transform.y = standart_camera_pos.y;
            }
            else
            {
                float new_camera_size = (1 + new_transform.y - standart_camera_pos.y) + standart_camera_size;
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, new_camera_size, 0.001f);
            }
            new_transform.z = standart_camera_pos.z;
            transform.position = new_transform;
        }
    }
}
