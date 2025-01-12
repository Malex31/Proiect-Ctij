using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    
    public class PlayerController : KinematicObject
    {
        public int maxJumps = 2; 
        private int jumpCount;


        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        public float dashSpeed = 10;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;
        public bool isFacingRight = true;

        

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        //pt checkpoint
        public static Vector2 lastCheckPointPos=new Vector2(0,0);

        public bool IsAlive { get; private set; } = true;


        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            
            //checkpoint
            if (lastCheckPointPos == Vector2.zero)
            {
                Debug.LogWarning("Player has no checkpoint set, initializing at (0, 0).");
                lastCheckPointPos = new Vector2(0, 0);
            }


        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                {

                    jumpState = JumpState.PrepareToJump;
                    jumpCount = 1;
                }
                else if (jumpState == JumpState.InFlight && jumpCount < maxJumps && Input.GetButtonDown("Jump"))
                {
                    // Permite o a doua săritură
                    jumpState = JumpState.PrepareToJump;
                    jumpCount++;
                }

                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();

           
        }

        public void Die()
        {
            IsAlive = false;  
         
        }
        public void Respawn()
        {
            IsAlive = true;  
            
        }


        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                        jumpCount = 0;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && jumpState == JumpState.Jumping)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            }
               else if (jump && jumpState == JumpState.Dash)
            {
                    velocity.y = 0;
                    velocity.x = (move.x > 0 ? dashSpeed : -dashSpeed); // Dash spre dreapta sau stânga
                }
               
            
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed,
            Dash
        }
    }
}