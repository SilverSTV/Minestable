using UnityEngine;

namespace Game.Scripts.Root.Input
{
    public struct InputState
    {
        public Vector2 PointerWorld;
        public bool PrimaryActionPressed;
        public bool PrimaryActionHeld;
        public Vector2 Move;
        public bool JumpPressed;
        public bool JumpHeld;
        public bool ToggleDebugModePressed;
    }
}
