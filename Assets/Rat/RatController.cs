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
    private enum RatStates { wander, move, chase, dying };

    private RatStates state = RatStates.wander;

    public Transform playerTransform = null;

    private Transform _transform = null;
    private Renderer _renderer = null;
    private SpriteRenderer _sprRenderer = null;
    private Rigidbody2D _rb = null;

    [SerializeField] Sprite Idle;
    [SerializeField] Sprite Walk;
    [SerializeField] Sprite Dead;
    Animation Idle_an;
    Animation Walk_an;
    Animation Dead_an;

    [SerializeField, Range(0, 100)] private float despawnOffScreenTime = 1.0f;
    [SerializeField, Range(0, 2)]   private float RatMinSpeed = 1.0f;
    [SerializeField, Range(2, 10)]  private float RatMaxSpeed = 5.0f;
    [SerializeField, Range(1, 10)]  private float PlayerDetectDistance = 5.0f;
    [SerializeField, Range(0, 1)]   private float WanderSlowDown = 0.3f;
    [SerializeField]                private int tick_rate = 5;
    private float RandomSpeed = 1.0f;
    private ushort health = 2;
    private float despawnOffScreenTimer = 0.0f;
    private int ticks = 0;
    // Start is called before the first frame update
    void Start()
    {
        RandomSpeed = UnityEngine.Random.Range(RatMinSpeed, RatMaxSpeed);
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        _sprRenderer = GetComponent<SpriteRenderer>();
        float random_scale = RandomSpeed / RatMaxSpeed;
        transform.localScale = new Vector3(transform.lossyScale.x * random_scale, transform.lossyScale.y * random_scale, transform.lossyScale.z);
        rat_move.Right = true;
    }

    // Update is called once per frame
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
                    state = RatStates.wander;
                    if (ticks > tick_rate)
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
            //
            // animate death
            // when died
            // state = RatStates.dead
            //
            if (!Dead_an.enabled)
                Destroy(gameObject);
        }
        float speed = 0.0f;
        switch (state) {
            case RatStates.wander:
                speed = RandomSpeed * Time.deltaTime * WanderSlowDown;
                break;
            case RatStates.chase:
                speed = RandomSpeed * Time.deltaTime;
                break;
        }
        if (rat_move.Left || rat_move.Right)
        {
            if (rat_move.Right)
            {
                _sprRenderer.flipX = false;
                transform.Translate(speed, 0, 0);
            }
            else if (rat_move.Left)
            {
                _sprRenderer.flipX = true;
                transform.Translate(-speed, 0, 0);
            }
        }
    }
}
