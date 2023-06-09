using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Atacking : MonoBehaviour
{
    private Animator _animator = null;
    // cached components
    [SerializeField] private GameObject _dagger = null;
    [SerializeField, Range(3, 10)] private float daggerSlashHeight = 4.0f;
    private bool playerStartedAtack = false;
    private Vector3 _colliderInitPos = Vector3.zero;
    private PlayerController _playerController = null;
    private Collider2D _daggerCollider = null;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        Debug.Assert(_dagger!= null);
        _dagger.SetActive(false);
        _daggerCollider = _dagger.GetComponent<Collider2D>();
        _colliderInitPos = _daggerCollider.offset;
        _animator = GetComponent<Animator>();   
    }
    
    // Update is called once per frame
    void Update()
    {
        // player started atack
        if (_playerController.atacking && !playerStartedAtack)
        {
            _daggerCollider.offset = _colliderInitPos;
            if (!_playerController.right)
            {
                _daggerCollider.offset = new Vector3(-_colliderInitPos.x, _colliderInitPos.y, _colliderInitPos.z);
            }
            _dagger.SetActive(true);
            playerStartedAtack = true;
        }
        else 
        {

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Atack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                _playerController.atacking = false;
                _animator.SetBool("Atack", false);
            }
        }
        // player finished atack
        if (!_playerController.atacking && playerStartedAtack) 
        {
            _dagger.SetActive(false);
            _daggerCollider.enabled = true;
            playerStartedAtack = false;
        }
    }
}
