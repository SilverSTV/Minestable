using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay;
using UnityEngine;

public class MineSpawner : MonoBehaviour 
{
    [SerializeField] private List<BlockSettings> _blockSettings;
    [SerializeField] private int _mineWidth,_mineHeight;
    [SerializeField] private BlockType _fillerBlockType;

    public MineField GenerateMine()
    {
        //Первоначальное заполнение
        var fillerBlockSettings = _blockSettings.FirstOrDefault(bs => bs._blockTypeId == _fillerBlockType);
        var fillerBlock = new WorldBlock
        {
            BlockType = _fillerBlockType,
            Durability = fillerBlockSettings!._maxDurability
        };

        var mine = new MineField(_mineWidth, _mineWidth, fillerBlock);
        return mine;
        
        //Заполнение каждым типом отдельно
    }
}
