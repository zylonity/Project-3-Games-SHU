using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public enum PlayerSpritesNm {Idle, Torch, Run, Jump};
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

    [SerializeField] Sprite[] PlayerSprites = new Sprite[4];
    // gamemode reference
    private GameController gm = null;
    // Start is called before the first frame update
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // assign camera player pos reference to player transform
        CameraController._camcont.playerTransform = this.transform;
        // assigne gamemode
        gm = GameController._gameController;
    }
    void OnDestroy()
    {
        // clear camera pointer if destroyed
        CameraController._camcont.playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Update only when Game gamemode is active
        if (gm != null && gm.current_state == GameController.MyGameState.Game)
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
    public void SetPlayerSprite(PlayerSpritesNm spr)
    {
        this.spriteRenderer.sprite = PlayerSprites[((int)spr)];
    }
}

        