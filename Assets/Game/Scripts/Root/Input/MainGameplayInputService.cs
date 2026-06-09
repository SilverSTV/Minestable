using Game.Scripts.Gameplay.Characters.Player;
using UnityEngine;

namespace Game.Scripts.Root.Input
{
    public class MainGameplayInputService : IInputService
    {
        private IInputAdapter _inputAdapter;

        public PlayerCommands Commands { get; private set; } = new();

        public MainGameplayInputService(IInputAdapter inputAdapter)
        {
            _inputAdapter = inputAdapter;
        }

        public void OnEnable()
        {
            Debug.Log("MainGameplayInputService is enabled");
        }

        public void OnDisable()
        {
            Debug.Log("MainGameplayInputService is disabled");
        }

        public void Tick(float dt)
        {
            var state = _inputAdapter.BuildState();

            Commands.Move = state.Move;
            Commands.PointerWorldPosition = state.PointerWorld;
            if (state.JumpPressed)
                Commands.JumpPressed.Set();
            if (state.ToggleDebugModePressed)
                Commands.ToggleModePressed.Set();
            if(state.PrimaryActionPressed)
                Commands.PointerClick.Set();
        }
    }
}
