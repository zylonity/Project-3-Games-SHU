using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCaller : MonoBehaviour
{

    public BackgroundHandler bgH;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            bgH.CreateHandler();
        }
    }
}
