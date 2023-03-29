using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private string currentAnimation = "";
    private Animator _animator = null;
    private PlayerController _pC = null;
    // animations
    private const string ATTACK = "Atack";
    private const string IDLE = "Idle";
    private const string WALK = "Walk";
    private const string TORCH_IDLE = "Idle Torch";
    private const string PONCHO_IDLE = "Idle Poncho";
    private const string PONCHO_WALK = "Walk Poncho";
    private const string TORCH_WALK = "Walk Torch";
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _pC = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // nice animation handling not that fucking shit in animator
        if(_pC.atacking)
            ChangeAnimationState(ATTACK);
        else if (!_pC.walk && !_pC.holdTorch && !_pC.ponchoOn)
            ChangeAnimationState(IDLE);
        else if (!_pC.walk && !_pC.holdTorch && _pC.ponchoOn)
            ChangeAnimationState(PONCHO_IDLE);
        else if (!_pC.walk && _pC.holdTorch && !_pC.ponchoOn)
            ChangeAnimationState(TORCH_IDLE);
        else if(_pC.walk && !_pC.holdTorch && !_pC.ponchoOn)
            ChangeAnimationState(WALK);
        else if (_pC.walk && _pC.holdTorch && !_pC.ponchoOn)
            ChangeAnimationState(TORCH_WALK);
        else if (_pC.walk && !_pC.holdTorch && _pC.ponchoOn)
            ChangeAnimationState(PONCHO_WALK);
    }

    private void SetAnimation() 
    {

    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
