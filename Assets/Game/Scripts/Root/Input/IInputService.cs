using System.Collections.Generic;

namespace Game.Scripts.Root.Input
{
    public interface IInputService
    {
        void OnEnable();
        void OnDisable();

        void Tick(float dt);
    }
}
