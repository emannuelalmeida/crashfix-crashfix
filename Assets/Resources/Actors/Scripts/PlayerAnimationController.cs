using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Actors
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private PlayerActor _playerActor;
        private Animator _animator;
        private PlayerState _lastKnownState;
        
        private readonly Dictionary<PlayerState, int> _triggerMap = new Dictionary<PlayerState, int>();

        PlayerAnimationController()
        {
            _triggerMap[PlayerState.IDLE] = Animator.StringToHash("idle");
            _triggerMap[PlayerState.MOVING] = Animator.StringToHash("walk");
            _triggerMap[PlayerState.INTERACTING] = Animator.StringToHash("action");
            _triggerMap[PlayerState.CHEERING] = Animator.StringToHash("victory");
            _triggerMap[PlayerState.DEFEAT] = Animator.StringToHash("defeat");
            _lastKnownState = PlayerState.IDLE;
        }

        private void Awake()
        {
            _playerActor = GetComponent<PlayerActor>();
            _playerActor.PlayerStateEvents.AddListener(UpdateAnimation);
            _animator = GetComponentInChildren<Animator>();
        }

        void UpdateAnimation(PlayerState state)
        {
            if (_lastKnownState != state)
            {
                _animator.SetTrigger(_triggerMap[state]);
                _lastKnownState = state;
            }
        }
    }
}