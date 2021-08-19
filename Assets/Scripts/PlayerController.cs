using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerController : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Animator _animator;
        /// <summary>HitBox used for attacks</summary>
        [SerializeField]
        private AttackHitBox _attackHitBox;
        /// <summary>Last input movement from user</summary>
        private Vector2 _inputMovement;
        /// <summary>Current velocity</summary>
        private Vector3 _velocity;
        /// <summary>If player is currently moving</summary>
        private bool _isMoving;
        /// <summary>Disable movement when true</summary>
        private bool _canMove = true;
        /// <summary>Current attack phase</summary>
        private AttackPhase _attackPhase;

        private void Awake()
        {
            _attackHitBox.gameObject.SetActive(false);
        }

        /// <summary>Toggle Moving animation</summary>
        private void Update()
        {
            var oldIsMoving = _isMoving;
            _isMoving = !Mathf.Approximately(_velocity.x, 0.0f);

            if (oldIsMoving && !_isMoving)
            {
                _animator.SetBool("Moving", false);
            }
            else if (!oldIsMoving && _isMoving)
            {
                _animator.SetBool("Moving", true);
            }
        }

        /// <summary>Handle movement</summary>
        private void FixedUpdate()
        {
            if (_canMove)
            {
                _velocity = Vector3.right * _inputMovement.x;
                transform.position += _velocity * Time.fixedDeltaTime;
            }
            else
            {
                _velocity = Vector2.zero;
            }
        }

        #region Inputs
        /// <summary>Called when pressing Horizontal input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnHorizontal(InputValue value)
        {
            _inputMovement = value.Get<Vector2>();
        }

        /// <summary>Called when pressing Punch input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnPunch()
        {
            _animator.SetTrigger("Punch");
        }

        /// <summary>Called when pressing Kick input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnKick()
        {
            _animator.SetTrigger("Kick");
        }

        /// <summary>Called when pressing HighPunch input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnHighPunch()
        {
            _animator.SetTrigger("HighPunch");
        }
        #endregion

        #region AnimEvents
        /// <summary>Called by animation when an attack begins</summary>
        /// <remarks>This is used to block the player when attacking</remarks>
        private void OnAttackBegin()
        {
            _canMove = false;
        }

        /// <summary>Called by animation when an attack ends</summary>
        private void OnAttackEnd()
        {
            _canMove = true;
        }

        /// <summary>Called by animation when a new phase of attack begins</summary>
        private void OnAttackPhaseBegin()
        {
            // Create a new phase
            _attackPhase = new AttackPhase();
            _attackPhase.source = this;

            // Activate the hitbox
            _attackHitBox.attackPhase = _attackPhase;
            _attackHitBox.gameObject.SetActive(true);
        }

        /// <summary>Called by animation when the current phase of attack ends</summary>
        private void OnAttackPhaseEnd()
        {
            CancelAttackPhase();
        }
        #endregion

        /// <summary>Cancel the current attack phase</summary>
        /// <remarks>Called when the phase is done or hit by an enemy</remarks>
        private void CancelAttackPhase()
        {
            _attackPhase = null;
            _attackHitBox.attackPhase = null;
            _attackHitBox.gameObject.SetActive(false);
        }

        public void OnHitBy(AttackPhase attackPhase, AttackCollision attackCollision)
        {
            // Not implemented for this sample
            // Should cancel the current attack if possible
        }
    }
}