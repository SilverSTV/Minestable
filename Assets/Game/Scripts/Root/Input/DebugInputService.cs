using System.Collections.Generic;
using Game.Scripts.Gameplay.Characters.Player;
using Game.Scripts.Root.UpdateSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts.Root.Input
{
    public class DebugInputService : IInputService
    {
        private IInputAdapter _adapter;

        public PlayerCommands Commands { get; private set; } = new();

        public DebugInputService(IInputAdapter adapter)
        {
            _adapter = adapter;
        }


        public void OnEnable()
        {
            Debug.Log("DebugInputService is enabled");
        }

        public void OnDisable()
        {
            Debug.Log("DebugInputService is disabled");
        }

        public void Tick(float dt)
        {
            var state = _adapter.BuildState();

            Commands.Move = state.Move;
            Commands.PointerWorldPosition = state.PointerWorld;
            if (state.PrimaryActionPressed)
                Commands.PointerClick.Set();
            if (state.ToggleDebugModePressed)
                Commands.ToggleModePressed.Set();
        }
    }
}
