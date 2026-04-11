using Game.Scripts.Configs;
using Game.Scripts.Gameplay;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private MineView _mineView;
    [SerializeField] private int _seed;

    private MineGrid _mineGrid;
    private MineGenerator _mineGenerator;
    private MineService _mineService;

    private async void Start()
    {
        if (_blockDatabase == null)
        {
            Debug.LogError("GameManager requires a BlockDatabase reference.", this);
            enabled = false;
            return;
        }

        if (_mineView == null)
        {
            Debug.LogError("GameManager requires a MineView reference.", this);
            enabled = false;
            return;
        }

        _mineGrid = new MineGrid(200, 100);

        _mineGenerator = new MineGenerator(_blockDatabase);
        _mineGenerator.GenerateMine(_mineGrid, _seed, BlockType.Stone);

        _mineService = new MineService(_mineGrid, _blockDatabase, BlockType.Air);
        await _mineView.RenderAsync(_mineGrid, _blockDatabase);
    }
}
