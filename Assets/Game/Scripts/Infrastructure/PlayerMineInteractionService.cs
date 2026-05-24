using Game.Scripts.Gameplay;
using Game.Scripts.Gameplay.Characters.Player;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
    public class PlayerMineInteractionService
    {
        private readonly MineInteractionService _mineInteractionService;
        private readonly PlayerController _player;
        private readonly float _interactionRadius;

        public PlayerMineInteractionService(MineInteractionService mineInteractionService, PlayerController player,
            float interactionRadius)
        {
            _mineInteractionService = mineInteractionService;
            _player = player;
            _interactionRadius = interactionRadius;
        }

        public bool TryUsePrimaryAction(Vector2 pointerWorldPosition, int damage)
        {
            int x = Mathf.FloorToInt(pointerWorldPosition.x);
            int y = -Mathf.FloorToInt(pointerWorldPosition.y);

            Vector2 blockCenterWorld = new Vector2(x + 0.5f, -y + 0.5f);
            Vector2 playerPosition = _player.transform.position;
            
            _player.PlayPrimaryAction();

            if (!IsWithinInteractionRadius(playerPosition, blockCenterWorld))
                return false;

            _mineInteractionService.TryDamageBlock(x, y, damage);
            return true;
        }
        
        private bool IsWithinInteractionRadius(Vector2 playerPosition, Vector2 targetWorldPosition)
        {
            Vector2 delta = targetWorldPosition - playerPosition;
            return delta.sqrMagnitude <= _interactionRadius * _interactionRadius;
        }
    }
}
