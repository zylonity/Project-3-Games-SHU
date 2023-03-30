using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundHandler : MonoBehaviour
{
    public PlayerController player;
    public GameObject bgHandler;
    public GameObject background;
    public HutSpawner hutSpawner;
    GameObject backgroundTwo;
    GameObject bgHandlerTwo;
    bool touched = false;
    bool firstColliderHit = false;
    bool passedFirstCollider = false;


    float bgMoveSpeed = 0.005f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7)
        {
            player.moveSpeed = 0;
            player.maxMovementSpeed = 0;
            touched = true;
        }

    }

    private void Start()
    {
        float xSize = background.GetComponent<Renderer>().bounds.size.x;

        backgroundTwo = Instantiate(background, new Vector3(background.transform.position.x + xSize, background.transform.position.y, background.transform.position.z), background.transform.rotation, bgHandler.transform);
    }


    void Update()
    {
        if (touched)
        {
            if (Input.GetKey("d"))
            {
                if(bgHandler != null)
                {
                    bgHandler.transform.position = new Vector3(bgHandler.transform.position.x - bgMoveSpeed, bgHandler.transform.position.y, bgHandler.transform.position.z);
                }
                
                if (bgHandlerTwo != null)
                {
                    bgHandlerTwo.transform.position = new Vector3(bgHandlerTwo.transform.position.x - bgMoveSpeed, bgHandlerTwo.transform.position.y, bgHandlerTwo.transform.position.z);
                }
            }
            if (Input.GetKey("a"))
            {
                player.moveSpeed = 5;
                player.maxMovementSpeed = 2;
                touched = false;
            }
        }
    }


    //Next set of backgrounds generation
    public void CreateHandler()
    {
        //Run only if it's the first background
        if (firstColliderHit == false)
        {
            bgHandlerTwo = Instantiate(bgHandler, new Vector3(backgroundTwo.transform.position.x + backgroundTwo.GetComponent<Renderer>().bounds.size.x, bgHandler.transform.position.y, bgHandler.transform.position.z), bgHandler.transform.rotation);
            firstColliderHit = true;
        }
        else if (firstColliderHit == true && passedFirstCollider == false)
        {
            Destroy(bgHandler.gameObject);
            backgroundTwo = bgHandlerTwo.transform.Find("Background(Clone)").gameObject;
            bgHandler = Instantiate(bgHandlerTwo, new Vector3(backgroundTwo.transform.position.x + backgroundTwo.GetComponent<Renderer>().bounds.size.x, bgHandler.transform.position.y, bgHandler.transform.position.z), bgHandler.transform.rotation);
            passedFirstCollider = true;
        }
        else if (firstColliderHit == true && passedFirstCollider == true)
        {
            Destroy(bgHandlerTwo.gameObject);
            backgroundTwo = bgHandler.transform.Find("Background(Clone)").gameObject;
            bgHandlerTwo = Instantiate(bgHandler, new Vector3(backgroundTwo.transform.position.x + backgroundTwo.GetComponent<Renderer>().bounds.size.x, bgHandler.transform.position.y, bgHandler.transform.position.z), bgHandler.transform.rotation);
            passedFirstCollider = false;
        }
       
    }
    

    public void ParentHut(GameObject hut)
    {
        hut.transform.parent = bgHandler.transform;
    }

}
