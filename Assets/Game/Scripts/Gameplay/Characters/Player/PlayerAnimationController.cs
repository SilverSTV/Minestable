using UnityEngine;

namespace Game.Scripts.Gameplay.Characters.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualRoot;

        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

        public void SetMovement(float horizontalSpeed, bool isGrounded)
        {
            _animator.SetFloat(MoveSpeedHash,Mathf.Abs(horizontalSpeed));
            _animator.SetBool(IsGroundedHash,isGrounded);

            if (horizontalSpeed > 0.01f)
                _visualRoot.localScale = new Vector3(-1f, 1f, 1f);
            else if (horizontalSpeed < -0.01f)
                _visualRoot.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
