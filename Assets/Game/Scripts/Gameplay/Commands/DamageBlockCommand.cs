using Game.Scripts.Gameplay;

namespace Game.Scripts.Root.Input
{
    public class DamageBlockCommand : IMineCommand
    {
        public int x, y;
        public int damage;
        
        public void Execute(MineInteractionService service)
        {
            service.TryDamageBlock(x, y, damage);
        }
    }
}
