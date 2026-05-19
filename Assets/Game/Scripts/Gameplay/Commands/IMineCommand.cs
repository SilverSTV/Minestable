using System.Windows.Input;
using Game.Scripts.Gameplay;

namespace Game.Scripts.Root.Input
{
    public interface IMineCommand
    {
        void Execute(MineInteractionService service);
    }
}
