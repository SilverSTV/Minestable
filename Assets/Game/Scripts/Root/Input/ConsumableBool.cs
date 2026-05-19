namespace Game.Scripts.Utils
{
    public class ConsumableBool
    {
        private bool _value;

        public void Set()
        {
            _value = true;
        }

        public bool TryConsume()
        {
            if (!_value)
                return false;

            _value = false;

            return true;
        }
    }
}
