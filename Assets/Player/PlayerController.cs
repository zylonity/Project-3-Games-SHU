using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public bool walk, holdTorch, ponchoOn, atacking = false;
    public GameObject gameOverGUI, playerGUI, pauseGUI;


    Rigidbody2D pRigidBody;
    SpriteRenderer spriteRenderer;
    public GameObject floorCheck;

    public int maxHealth = 10;
    public int playerHealth = 10;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField, Range(0, 100)]
    float maxMovementSpeed;

    [SerializeField, Range(0, 100)]
    float jumpHeight = 5f;

    void KillPlayer()
    {
        playerGUI.SetActive(false);
        pauseGUI.SetActive(true);
        Time.timeScale = 0;
    }



    // Start is called before the first frame update
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // assign camera player pos reference to player transform
        maxHealth = playerHealth;
        Debug.Assert(maxHealth != 0,"Health is 0 set it!");
        _animator = gameObject.GetComponent<Animator>();
        
    }
  
    // Update is called once per frame
    void Update()
    {
        // debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            DamagePlayer();
        }
        // Torch key
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!holdTorch)
            {
                Debug.Log("Holding torch");
                holdTorch = true;
                ponchoOn = false;
            }
            else
                holdTorch = false;
        }
        // Poncho key
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!ponchoOn)
            {
                Debug.Log("Poncho on");
                ponchoOn = true;
                holdTorch = false;
            }
            else
                ponchoOn = false;
        }
        // Attack key
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!atacking)
            {
                Debug.Log("Atacking");
                atacking = true;
                ponchoOn = false;
                holdTorch = false;
            }
        }


        // Update only when Game gamemode is active
        if (playerHealth >= 1)
        {
            float xAxis = Input.GetAxis("Horizontal");

            if (xAxis > 0.1 || xAxis < -0.1)
            {
                walk = true;
                pRigidBody.AddForce(new Vector2(xAxis * moveSpeed, 0));
                if (pRigidBody.velocity.x > maxMovementSpeed || pRigidBody.velocity.x < -maxMovementSpeed)
                {
                    pRigidBody.velocity = new Vector2(Mathf.Sign(pRigidBody.velocity.x) * maxMovementSpeed, pRigidBody.velocity.y);
                }

                if (xAxis > 0.1)
                    spriteRenderer.flipX = false;
                else if (xAxis < -0.1)
                    spriteRenderer.flipX = true;

            }
            else
                walk = false;
            float jumpAxis = Input.GetAxis("Jump");

            int layerMask = ~(1 << LayerMask.NameToLayer("Triggers"));
            //check if the player is touching the floor using a raycast
            RaycastHit2D hit = Physics2D.Raycast(floorCheck.transform.position, Vector2.down, 0.05f, layerMask);
            Debug.DrawRay(floorCheck.transform.position, Vector2.down * 0.05f, Color.green);
            if (hit.collider != null)
            {
                //print("touching");
                if (jumpAxis > 0.1) 
                {
                    pRigidBody.AddForce(new Vector2(0, jumpAxis * jumpHeight));
                }
            }
            if (playerHealth <= 0)
                KillPlayer();
            // Update animator
            _animator.SetBool("Walk", walk);
            _animator.SetBool("Poncho", ponchoOn);
            _animator.SetBool("Torch", holdTorch);
            _animator.SetBool("Atack", atacking);
            if(atacking)
                if(!_animator.IsInTransition(0))
                    atacking = false;
        }
    }
    public void DamagePlayer()
    {
        --playerHealth;
        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }

}

        