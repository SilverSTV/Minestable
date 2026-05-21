using System;

namespace Game.Scripts.Configs
{
    public interface IEnumKey<TKey> where TKey : struct, Enum
    {
        TKey Id { get; }
    }
}
