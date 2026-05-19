using System;
using Game.Scripts.Root.Input;
using UnityEngine;

namespace Game.Scripts.Gameplay.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private float _groundCheckRadius = 0.15f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private PlayerAnimationController _animationController;
        [SerializeField] private Transform _cameraFollowPoint;

        private Rigidbody2D _rb;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public void Execute(PlayerCommands commands)
        {
            Vector2 commandsMove = commands.Move;
            
            Move(commandsMove);
            if (commands.JumpPressed.TryConsume())
            {
                Jump();
            }
            
            _animationController.SetMovement(commandsMove.x,CheckIsGrounded());
        }

        private void Move(Vector2 move)
        {
            var velocity = _rb.linearVelocity;
            velocity.x = move.x * _movementSpeed;
            _rb.linearVelocity = velocity;
        }

        private void Jump()
        {
            bool isGrounded = CheckIsGrounded(); 
            if (!isGrounded)
                return;
            
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private bool CheckIsGrounded() => Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
    }
}
