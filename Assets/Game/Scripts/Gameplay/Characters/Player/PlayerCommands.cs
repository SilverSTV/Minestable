
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Gameplay.Characters.Player
{
    public class PlayerCommands
    {
        public Vector2 Move;
        public ConsumableBool JumpPressed = new();
        public ConsumableBool PointerClick = new();
        public Vector2 PointerWorldPosition;
        public ConsumableBool ToggleModePressed = new();
    }
}
