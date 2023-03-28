using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] private GameObject _torchArea = null;
    private PlayerController _playerController = null;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        Debug.Assert( _torchArea != null, "Torch isn't assigned" );
        _torchArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerController.holdTorch)
            _torchArea.SetActive(true);
        else
            _torchArea.SetActive(false);
    }
}
