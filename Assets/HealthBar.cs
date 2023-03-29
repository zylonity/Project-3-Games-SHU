using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerController pl;

    private RectTransform _rectTansform;
    private Vector3 standartScale;
    // Start is called before the first frame update
    private void Awake()
    {
        _rectTansform = GetComponent<RectTransform>();
        standartScale = _rectTansform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (pl != null) 
        {
            if (pl.playerHealth != pl.maxHealth)
                _rectTansform.localScale = new Vector3(standartScale.x * ((float)pl.playerHealth / pl.maxHealth), standartScale.y ,standartScale.z);
        }
    }
}
