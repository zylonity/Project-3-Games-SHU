using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerModes {Idle, Torch, Run, Jump};
    public PlayerModes _playerMode = PlayerModes.Torch;
    Rigidbody2D pRigidBody;
    SpriteRenderer spriteRenderer;
    public static PlayerController _playerController = null;
    public GameObject floorCheck;

    public readonly int maxHealth = 10;
    public int playerHealth = 0;
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
    private void Awake()
    {
        _playerController = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // assign camera player pos reference to player transform
        CameraController._camcont.playerTransform = this.transform;
        // assigne gamemode
        gm = GameController._gameController;
        playerHealth = maxHealth;
        
    }
    void OnDestroy()
    {
        // clear camera pointer if destroyed
        CameraController._camcont.playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        // debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            DamagePlayer();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_playerMode != PlayerModes.Torch)
            {
                Debug.Log("Going into torch mode");
                _playerMode = PlayerModes.Torch;
            }
            else
            {
                Debug.Log("Going into Idle mode");
                _playerMode = PlayerModes.Idle;
            }
        }
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
                    spriteRenderer.flipX = false;
                else if (xAxis < -0.1)
                    spriteRenderer.flipX = true;

            }

            float jumpAxis = Input.GetAxis("Jump");

            int layerMask = ~(1 << LayerMask.NameToLayer("Triggers"));
            //check if the player is touching the floor using a raycast
            RaycastHit2D hit = Physics2D.Raycast(floorCheck.transform.position, Vector2.down, 0.05f, layerMask);
            Debug.DrawRay(floorCheck.transform.position, Vector2.down * 0.05f, Color.green);
            if (hit.collider != null)
            {
                print("touching");
                if (jumpAxis > 0.1) 
                {
                    pRigidBody.AddForce(new Vector2(0, jumpAxis * jumpHeight));
                }
            }
            if (playerHealth <= 0)
                gm.ChangeGameState(GameController.MyGameState.Over);
        }
    }
    public void DamagePlayer()
    {
        --playerHealth;
        if (playerHealth <= 0)
            gm.ChangeGameState(GameController.MyGameState.Over);
    }
    public void SetPlayerSprite(PlayerModes spr)
    {
        this.spriteRenderer.sprite = PlayerSprites[((int)spr)];
    }
}

        