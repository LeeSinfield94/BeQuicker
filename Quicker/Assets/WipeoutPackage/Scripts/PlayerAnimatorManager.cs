using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorManager : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimation(float speed)
        {
            
            animator.SetFloat("Speed", speed);
        }
    } 
}
