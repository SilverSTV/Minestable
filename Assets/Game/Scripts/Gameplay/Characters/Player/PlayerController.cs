using System;
using Game.Scripts.Root.Input;
using UnityEngine;

namespace Game.Scripts.Gameplay.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private float _groundCheckDistance = 0.05f;
        [SerializeField] private float _groundCheckHeight = 0.08f;
        [SerializeField] private float _groundNormalMinY = 0.5f;
        [SerializeField] private float _groundCheckWidthInset = 0.05f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _airMovementMultiplier = 0.5f;
        [SerializeField] private float _wallCheckDistance = 0.05f;
        [SerializeField] private float _wallCheckWidth = 0.08f;
        [SerializeField] private float _wallCheckHeightInset = 0.05f;


        [Header("Aim")] 
        [SerializeField] private float _headAimSmooth;
        [SerializeField] private float _headAimClamp;
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private Transform _headPivot;

        [Header("Animations")]
        [SerializeField] private PlayerAnimationController _animationController;

        private Rigidbody2D _rb;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public void Execute(PlayerCommands commands)
        {
            UpdateMovement(commands);
            UpdateAim(commands);
            
        }

        private void UpdateMovement(PlayerCommands commands)
        {
            Vector2 commandsMove = commands.Move;
            _isGrounded = CheckIsGrounded();

            Move(commandsMove);
            if (commands.JumpPressed.TryConsume())
            {
                Jump();
            }

            _animationController.UpdateLocomotion(commandsMove.x, _rb.linearVelocityY, _isGrounded);
        }

        private void UpdateAim(PlayerCommands commands)
        {
            Vector2 pointerWorldPosition = commands.PointerWorldPosition;

            float characterX = _visualRoot.position.x;
            bool lookRight = pointerWorldPosition.x > characterX;

            _visualRoot.localScale = lookRight
                ? new Vector3(-1f, 1f, 1f)
                : new Vector3(1f, 1f, 1f);

            Vector2 origin = _headPivot.position;
            Vector2 direction = pointerWorldPosition - origin;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction.Normalize();

            Vector2 facingDirection = lookRight ? Vector2.right : Vector2.left;
            float targetAngle = Vector2.SignedAngle(facingDirection, direction);
            targetAngle = Mathf.Clamp(targetAngle, -_headAimClamp, _headAimClamp);
            targetAngle = lookRight ? -targetAngle : targetAngle;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            _headPivot.localRotation = Quaternion.Slerp(
                _headPivot.localRotation,
                targetRotation,
                Time.deltaTime * _headAimSmooth);
        }

        public void PlayPrimaryAction()
        {
            _animationController.PlayPrimaryAction();
        }

        private void Move(Vector2 move)
        {
            var velocity = _rb.linearVelocity;
            float moveX = move.x;

            if (_isGrounded)
            {
                velocity.x = Mathf.Abs(moveX) < 0.01f ? 0f : moveX * _movementSpeed;
                _rb.linearVelocity = velocity;
                return;
            }

            bool isTouchingWall = CheckWallContact(moveX);
            if (isTouchingWall && Mathf.Abs(moveX) > 0.01f)
            {
                velocity.x = 0f;
                _rb.linearVelocity = velocity;
                return;
            }

            if (Mathf.Abs(moveX) < 0.01f)
            {
                _rb.linearVelocity = velocity;
                return;
            }

            float currentHorizontalVelocity = velocity.x;
            bool hasHorizontalInertia = Mathf.Abs(currentHorizontalVelocity) > 0.01f;
            bool changesDirection = hasHorizontalInertia && Mathf.Sign(moveX) != Mathf.Sign(currentHorizontalVelocity);

            if (!hasHorizontalInertia || changesDirection)
                velocity.x = moveX * (_movementSpeed * _airMovementMultiplier);

            _rb.linearVelocity = velocity;
        }

        private void Jump()
        {
            if (!_isGrounded)
                return;

            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private bool CheckIsGrounded()
        {
            var bounds = _bodyCollider.bounds;

            float castWidth = Mathf.Max(0.01f, bounds.size.x - _groundCheckWidthInset * 2f);
            var castSize = new Vector2(castWidth, _groundCheckHeight);

            var castOrigin = new Vector2(bounds.center.x, bounds.min.y + _groundCheckHeight * 0.5f);

            RaycastHit2D hit = Physics2D.BoxCast(
                castOrigin,
                castSize,
                0f,
                Vector2.down,
                _groundCheckDistance,
                _groundLayer);

            return hit.collider != null && hit.normal.y >= _groundNormalMinY;
        }

        private bool CheckWallContact(float moveX)
        {
            if (Mathf.Abs(moveX) < 0.01f)
                return false;

            Bounds bounds = _bodyCollider.bounds;

            float direction = Mathf.Sign(moveX);
            Vector2 castDirection = direction > 0f ? Vector2.right : Vector2.left;

            float castHeight = Mathf.Max(0.01f, bounds.size.y - _wallCheckHeightInset * 2f);
            Vector2 castSize = new Vector2(_wallCheckWidth, castHeight);

            Vector2 castOrigin = new Vector2(
                direction > 0f ? bounds.max.x - _wallCheckWidth * 0.5f : bounds.min.x + _wallCheckWidth * 0.5f,
                bounds.center.y);

            RaycastHit2D hit = Physics2D.BoxCast(
                castOrigin,
                castSize,
                0f,
                castDirection,
                _wallCheckDistance,
                _groundLayer);

            return hit.collider != null;
        }
    }
}
