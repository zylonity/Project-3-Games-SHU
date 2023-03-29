using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RatController : MonoBehaviour
{
    private enum RatStates { wander, chase, affected, dying };
    private RatStates state = RatStates.wander;
    private bool Right, move, outOfDangerArea = false; // movements

    private float RatCheckTimer = 0.0f;
    private float affectedTimer = 0.0f;
    private float despawnOffScreenTimer = 0.0f;

    [SerializeField, Range(0, 1)] private float RatCheckTime = 0.1f;
    [SerializeField, Range(0, 100)] private float despawnOffScreenTime = 1.0f;
    [SerializeField, Range(0, 2)] private float RatMinSpeed = 1.0f;
    [SerializeField, Range(2, 10)] private float RatMaxSpeed = 5.0f;
    [SerializeField, Range(1, 10)] private float PlayerDetectDistance = 5.0f;
    [SerializeField, Range(0, 1)] private float WanderSlowDown = 0.3f;
    [SerializeField] private int ticksTillDecideDirectionInWanderMode = 5;
    [SerializeField, Range(0, 3)] private float lightAffectedTime = 1.0f;


    // other cached components
    private PlayerController _player;
    private Transform _playerTransform = null;

    // this cached components
    private Transform _transform = null;
    private Renderer _renderer = null;
    private SpriteRenderer _sprRenderer = null;
    private Rigidbody2D _rb = null;
    private Animator _animator = null;

    private float RandomSpeed = 1.0f;
    private ushort health = 2;
    private int ticks = 0;
    // Start is called before the first frame update
    void Start()
    {
        // assign pointers
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerTransform = _player.transform;
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        _sprRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        RandomSpeed = UnityEngine.Random.Range(RatMinSpeed, RatMaxSpeed);
        float random_scale = RandomSpeed / RatMaxSpeed;
        transform.localScale = new Vector3(transform.lossyScale.x * random_scale, transform.lossyScale.y * random_scale, transform.lossyScale.z);
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // cant torture poor rattie anymore aren't you
        if (state != RatStates.dying)
        {

            if (collision.CompareTag("Dagger"))
            {
                Debug.Log("Rat killed by dagger \"Ya tebya porodil, ya tebya i ubyu!\"");
                state = RatStates.dying;
                collision.enabled = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (state != RatStates.dying)
        {
            bool bite = other.CompareTag("BiteTrigger");
            if (other.CompareTag("TorchCircle") || bite)
            {
                Debug.Log("Rat entered trigger");
                if (bite && state != RatStates.affected)
                    _player.DamagePlayer();
                state = RatStates.affected;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (state != RatStates.dying)
        {
            if (other.CompareTag("TorchCircle") || other.CompareTag("BiteTrigger"))
            {
                Debug.Log("Rat Entered torch area");
                outOfDangerArea = true;
            }
        }
    }
    void Update()
    {
        // mouse events
        if (_playerTransform != null && state != RatStates.dying)
        {
            RatCheckTimer += Time.deltaTime;
            // there is not much sense to check player position every frame so I did this timer
            if (RatCheckTimer > RatCheckTime)
            {
                ++ticks;
                RatCheckTimer = 0.0f;
                float dist = _playerTransform.position.x - transform.position.x;

                if (math.abs(dist) < PlayerDetectDistance)
                {
                    move = true;
                    if (state != RatStates.affected)
                    {
                        state = RatStates.chase;
                        if (dist > 0.0f)
                        {
                            Right = true;
                        }
                        else
                        {
                            Right = false;
                        }
                    }
                    else 
                    {
                        if (dist > 0.0f)
                        {
                            Right = false; 
                        }
                        else
                        {
                            Right = true;
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
                        int decide = UnityEngine.Random.Range(0, 3);
                        if (decide == 0)
                        {
                            move = true;
                            Right = true;
                        }
                        else if (decide == 1)
                        {
                            move = true;
                            Right = false;
                        }
                        else if (decide == 2)
                        {
                            move = false;
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
        if (move)
        {
            if (Right)
            {
                _sprRenderer.flipX = false;
                _rb.velocity = new Vector2(speed.x, speed.y);
            }
            else
            {
                _sprRenderer.flipX = true;
                _rb.velocity = new Vector2(-speed.x, speed.y);
            }
        }
        // animator updates
        _animator.SetBool("Move", move);
        _animator.SetBool("Dying", state == RatStates.dying);
    }
    private void LateUpdate()
    {
       if (state == RatStates.dying && _animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
       {
           Destroy(gameObject);
       }
    }
}
