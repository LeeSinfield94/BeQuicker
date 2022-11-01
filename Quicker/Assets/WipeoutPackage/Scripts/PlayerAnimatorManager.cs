using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimationSpeed(float speed)
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if(_animator)
                _animator.SetFloat("Speed", speed);
        }
    } 
}
