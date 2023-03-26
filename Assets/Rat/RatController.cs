using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    private class Move 
    { 
        public bool Right = false;
        public bool Left = false;
        public void ClearMoves() 
        { 
            Right = false;
            Left = false;
        }
    } 
    private Move rat_move = new Move();
    [SerializeField, Range(0,1)] private float RatCheckTime = 0.1f;
    private float RatCheckTimer = 0.0f;
    private enum RatStates { alive, chase, dying };
    private RatStates state = RatStates.alive;
    public Transform playerTransform;
    private Renderer _renderer;
    private Rigidbody2D _rb;

    [SerializeField] Sprite Idle;
    [SerializeField] Sprite Walk;
    [SerializeField] Sprite Dead;
    Animation Idle_an;
    Animation Walk_an;
    Animation Dead_an;

    [SerializeField, Range(0, 100)] private float despawnOffScreenTime = 1.0f;
    [SerializeField, Range(0, 10)]  private float RatSpeed = 1.0f;
    private ushort health = 2;
    private float despawnOffScreenTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            RatCheckTimer += Time.deltaTime;
            if (RatCheckTimer > RatCheckTime)
            {
                RatCheckTimer = 0.0f;

                if (playerTransform.position.x - this.transform.position.x > 0.0f)
                    rat_move.Right = true;
                else
                    rat_move.Right = false;
                if (playerTransform.position.x - this.transform.position.x < 0.0f)
                    rat_move.Left = true;
                else
                    rat_move.Left = false;
            }
        }
        if (rat_move.Right)
            _rb.AddForce(new Vector2(RatSpeed, 0));
        else if (rat_move.Left)
            _rb.AddForce(new Vector2(-RatSpeed, 0));
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
    }
}
