using System.Collections.Generic;
using Game.Scripts.Gameplay.Characters.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Root.Input
{
    public class PlayerInputAdapter : MonoBehaviour, IInputAdapter
    {
        private Camera _camera;
        private PlayerController _player;

        private Vector2 _pointerScreen;
        private bool _primaryPressed;
        private bool _primaryHeld;
        private Vector2 _move;
        private bool _jumpPressed;
        private bool _toggleDebugModePressed;

        public void Initialize(Camera cam)
        {
            _camera = cam;
        }

        public void OnPrimaryAction(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
                _primaryPressed = true;

            _primaryHeld = ctx.ReadValueAsButton();
        }

        public void OnPointer(InputAction.CallbackContext ctx)
        {
            _pointerScreen = ctx.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            _move = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                //Debug.Log("JumpPressed");
                _jumpPressed = true;
            }
        }
        
        public void OnToggleDebugMode(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _toggleDebugModePressed = true;
            }
        }

        public InputState BuildState()
        {
            var world = _camera.ScreenToWorldPoint(_pointerScreen);

            var state = new InputState
            {
                PointerWorld = world,
                PrimaryActionPressed = _primaryPressed,
                PrimaryActionHeld = _primaryHeld,
                Move = _move,
                JumpPressed = _jumpPressed,
                JumpHeld = false,
                ToggleDebugModePressed = _toggleDebugModePressed
            };

            _primaryPressed = false;
            _jumpPressed = false;
            _toggleDebugModePressed = false;

            return state;
        }
    }
}
