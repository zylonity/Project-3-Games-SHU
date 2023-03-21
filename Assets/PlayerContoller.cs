using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    Rigidbody2D pRigidBody;
    SpriteRenderer spriteRenderer;

    public GameObject floorCheck;
    

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField, Range(0, 100)]
    float maxMovementSpeed;

    [SerializeField, Range(0, 100)]
    float jumpHeight = 5f;

    bool touchingFloor = false;

    
    // Start is called before the first frame update
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        if (xAxis > 0.1 || xAxis < -0.1)
        {
            pRigidBody.AddForce(new Vector2(xAxis * moveSpeed, 0));
            if (pRigidBody.velocity.x > maxMovementSpeed || pRigidBody.velocity.x < -maxMovementSpeed)
            {
                pRigidBody.velocity = new Vector2(Mathf.Sign(pRigidBody.velocity.x) * maxMovementSpeed, pRigidBody.velocity.y);
            }

            if (xAxis > 0.1)
                transform.localScale.Set(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            else if (xAxis < 0.1)
                transform.localScale.Set(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

        float jumpAxis = Input.GetAxis("Jump");
        
        //check if the player is touching the floor using a raycast
        RaycastHit2D hit = Physics2D.Raycast(floorCheck.transform.position, Vector2.down, 0.05f);
        Debug.DrawRay(floorCheck.transform.position, Vector2.down*0.05f, Color.green);
        if (hit.collider != null)
        {
            print("touching");
            if (jumpAxis > 0.1)
            {
                pRigidBody.AddForce(new Vector2(0, jumpAxis * jumpHeight));
            }
        }





    }
}

        