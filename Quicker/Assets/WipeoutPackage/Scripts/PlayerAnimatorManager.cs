using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimation(float speed)
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            animator.SetFloat("Speed", speed);
        }
    } 
}
