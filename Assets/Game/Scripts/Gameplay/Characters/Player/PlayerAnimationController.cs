using UnityEngine;

namespace Game.Scripts.Gameplay.Characters.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualRoot;

        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        private static readonly int PrimaryAction = Animator.StringToHash("PrimaryAction");

        public void UpdateLocomotion(float horizontalSpeed,float verticalSpeed, bool isGrounded)
        {
            _animator.SetFloat(MoveSpeedHash,Mathf.Abs(horizontalSpeed));
            _animator.SetFloat(VerticalSpeedHash, verticalSpeed);
            _animator.SetBool(IsGroundedHash,isGrounded);
        }

        public void PlayPrimaryAction()
        {
            _animator.SetTrigger(PrimaryAction);
        }
    }
}
