using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] private Canvas _playerUI;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert( _playerUI != null, "Player UI isn't assigned" );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
