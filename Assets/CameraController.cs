using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform = null;
    private GameController gm = null;
    public static CameraController _camcont = null;
    Vector3 standart_camera_pos;
    // Start is called before the first frame update
    void Awake()
    {
        _camcont = this;
        standart_camera_pos = this.transform.position;
    }
    private void Start()
    {
        gm = GameController._gameController;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gm != null && playerTransform != null)
        {
            if (gm.current_state == GameController.MyGameState.Game)
            {
                Vector3 new_transform = playerTransform.position;
                new_transform.y = standart_camera_pos.y;
                new_transform.z = standart_camera_pos.z;
                transform.position = new_transform;
            }
        }
        else
        {
            transform.position = standart_camera_pos;
        }
    }
}
