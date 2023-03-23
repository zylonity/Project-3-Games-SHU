using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{
    [SerializeField] Sprite Idle;
    [SerializeField] Sprite Walk;
    [SerializeField] Sprite Dead;
    public Transform playerTransform;
    Animation Idle_an;
    Animation Walk_an;
    Animation Dead_an;
    private ushort health = 2;
    private enum RatStates { alive, chase, dying };
    private RatStates state = RatStates.alive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
