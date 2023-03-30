using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerController pl;

    private RectTransform _rectTansform;
    // Start is called before the first frame update
    private void Awake()
    {
        _rectTansform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pl != null) 
        {
            _rectTansform.localScale = new Vector3(1.0f * ((float)pl.playerHealth / (float)pl.maxHealth), 1.0f, 1.0f);
        }
    }
}
