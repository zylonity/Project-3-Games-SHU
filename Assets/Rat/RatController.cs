using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RatController : MonoBehaviour
{
    private class Move 
    {
        public bool Right = false;
        public bool Left = false;
    } 
    private Move rat_move = new Move();
    [SerializeField, Range(0,1)] private float RatCheckTime = 0.1f;
    public float RatCheckTimer = 0.0f;
    // torchok is affected by torch
    private enum RatStates { wander, chase, affected, dying };
    private bool outOfDangerArea = false;
    private float affectedTimer = 0.0f;
    private RatStates state = RatStates.wander;

    private PlayerController _player;
    private Transform playerTransform = null;

    private Transform _transform = null;
    private Renderer _renderer = null;
    private SpriteRenderer _sprRenderer = null;
    private Rigidbody2D _rb = null;

    [SerializeField, Range(0, 100)] private float despawnOffScreenTime = 1.0f;
    [SerializeField, Range(0, 2)]   private float RatMinSpeed = 1.0f;
    [SerializeField, Range(2, 10)]  private float RatMaxSpeed = 5.0f;
    [SerializeField, Range(1, 10)]  private float PlayerDetectDistance = 5.0f;
    [SerializeField, Range(0, 1)]   private float WanderSlowDown = 0.3f;
    [SerializeField]                private int   ticksTillDecideDirectionInWanderMode = 5;
    [SerializeField, Range(0, 3)]   private float lightAffectedTime = 1.0f;
    private float RandomSpeed = 1.0f;
    private ushort health = 2;
    private float despawnOffScreenTimer = 0.0f;
    private int ticks = 0;
    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerController._playerController;
        playerTransform = _player.transform;
        RandomSpeed = UnityEngine.Random.Range(RatMinSpeed, RatMaxSpeed);
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        _sprRenderer = GetComponent<SpriteRenderer>();
        float random_scale = RandomSpeed / RatMaxSpeed;
        transform.localScale = new Vector3(transform.lossyScale.x * random_scale, transform.lossyScale.y * random_scale, transform.lossyScale.z);
        rat_move.Right = true;
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        bool bite = other.CompareTag("BiteTrigger");
        if (other.CompareTag("TorchCircle") || bite)
        {
            Debug.Log("Rat entered trigger");
            if (bite && state != RatStates.affected)
                _player.DamagePlayer();
            if (state != RatStates.dying)
                state = RatStates.affected;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TorchCircle") || other.CompareTag("BiteTrigger"))
        {
            Debug.Log("Rat Entered torch area");
            if(state!= RatStates.dying)
                outOfDangerArea = true;
        }
    }
    void Update()
    {
        // mouse events
        if (playerTransform != null && state != RatStates.dying)
        {
            RatCheckTimer += Time.deltaTime;
            // there is not much sense to check player position every frame so I did this timer
            if (RatCheckTimer > RatCheckTime)
            {
                ++ticks;
                RatCheckTimer = 0.0f;
                float dist = playerTransform.position.x - transform.position.x;

                if (math.abs(dist) < PlayerDetectDistance)
                {
                    if (state != RatStates.affected)
                    {
                        state = RatStates.chase;
                        if (dist > 0.0f)
                        {
                            rat_move.Right = true;
                            rat_move.Left = false;
                        }
                        else
                        {
                            rat_move.Right = false;
                            rat_move.Left = true;
                        }
                    }
                    else 
                    {
                        if (dist > 0.0f)
                        {
                            rat_move.Right = false; 
                            rat_move.Left = true;
                        }
                        else
                        {
                            rat_move.Right = true;
                            rat_move.Left = false;
                        }
                    }
                }
                else
                {
                    state = RatStates.wander;
                    if (ticks > ticksTillDecideDirectionInWanderMode)
                    {
                        ticks = 0;
                        // if wandering chouse random direction per tick
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            rat_move.Right = true;
                            rat_move.Left = false;
                        }
                        else
                        {
                            rat_move.Right = false;
                            rat_move.Left = true;
                        }
                    }
                }
                if (outOfDangerArea)
                {
                    affectedTimer += Time.deltaTime;
                    if (affectedTimer > lightAffectedTime)
                    {
                        affectedTimer = 0;
                        outOfDangerArea = false;
                        state = RatStates.wander;
                    }
                }
            }
        }
        if (!_renderer.isVisible)
        {
            despawnOffScreenTimer += Time.deltaTime;
            if(despawnOffScreenTimer > despawnOffScreenTime)
                Destroy(gameObject);
        }
        if (health <= 0)
        {
                Destroy(gameObject);
        }
        Vector2 speed = new Vector2(0.0f, 0.0f);
        switch (state) 
        {
            case RatStates.wander:
                speed.x = RandomSpeed * WanderSlowDown;
                break;
            case RatStates.chase:
                speed.x = RandomSpeed;
                break;
            case RatStates.affected:
                speed.x = RandomSpeed * 2.0f;
                break;
        }
        if (rat_move.Left || rat_move.Right)
        {
            if (rat_move.Right)
            {
                _sprRenderer.flipX = false;
                _rb.velocity = new Vector2(speed.x, speed.y);
            }
            else if (rat_move.Left)
            {
                _sprRenderer.flipX = true;
                _rb.velocity = new Vector2(-speed.x, speed.y);
            }
        }
    }
}
