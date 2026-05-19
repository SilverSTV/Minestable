using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Root.Input
{
    public class InputServiceSwitcher
    {
        private IInputService _current;

        public IInputService Current => _current;


        public void SwitchTo(IInputService next)
        {
            if (_current == next)
                return;
            
            _current?.OnDisable();

            _current = next;
            
            _current?.OnEnable();
        }
    }
}
