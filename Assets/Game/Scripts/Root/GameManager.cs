using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay;
using Game.Scripts.Gameplay.Characters.Player;
using Game.Scripts.Gameplay.PlayerResources;
using Game.Scripts.Gameplay.View.UI;
using Game.Scripts.Root.Input;
using Game.Scripts.Root.UpdateSystem;
using Game.Scripts.View;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Scripts.Root
{
    public class GameManager : MonoBehaviour
    {
        //Mine
        private MineGrid _mineGrid;
        private MineGenerator _mineGenerator;
        private MineInteractionService _mineInteractionService;

        [Header("Mine generation")] [SerializeField]
        private BlockDatabase _blockDatabase;

        [SerializeField] private MineView _mineView;
        [SerializeField] private int _seed;
        [SerializeField] private CaveGenerationSettings _caveGenerationSettings;
        [SerializeField] private VeinSettingsDatabase _veinSettingsDatabase;
        
        

        //Input
        private DebugInputService _debugInput;
        private MainGameplayInputService _mainGameplayInput;
        private InputServiceSwitcher _inputServiceSwitcher;
        [Header("Input")] [SerializeField] private PlayerInputAdapter _inputAdapter;
        [SerializeField] private CameraMoveController _cameraMover;

        [Header("UI")] [SerializeField] private ResourcePanelView _resourcePanelView;

        [Header("Debug")] [SerializeField] private int _debugBlockDamage;

        [Header("Characters")] [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _playerParentObj;
        private PlayerController _playerController;


        private void Awake()
        {
            if (!TryMineInit()) return;

            InputInit();
            PlayerInit();
        }

        private void Update()
        {
            InputHandle();
        }

        private void FixedUpdate()
        {
            DebugModeExecute();
            MainGameplayModeExecute();
        }

        private bool TryMineInit()
        {
            if (_blockDatabase == null)
            {
                Debug.LogError("GameManager requires a BlockDatabase reference.", this);
                enabled = false;
                return false;
            }

            if (_mineView == null)
            {
                _mineView = FindAnyObjectByType<MineView>();
            }

            _mineGrid = new MineGrid(200, 100);

            _mineGenerator = new MineGenerator(_blockDatabase, _caveGenerationSettings,_veinSettingsDatabase);
            _mineGenerator.GenerateMine(_mineGrid, _seed, BlockType.Stone, BlockType.Dirt);

            var resourceStorage = new ResourceStorage();
            var resourceService = new ResourceService(resourceStorage);
            _mineInteractionService =
                new MineInteractionService(_mineGrid, _blockDatabase, BlockType.Air, resourceService);
            _mineView.RedrawAllAsync(_mineGrid, _blockDatabase);
            _resourcePanelView.Init(resourceService);
            return true;
        }

        private void InputInit()
        {
            if (_inputAdapter == null)
            {
                _inputAdapter = FindAnyObjectByType<PlayerInputAdapter>();
            }

            _inputAdapter.Initialize(Camera.main);
            _inputServiceSwitcher = new InputServiceSwitcher();
            _debugInput = new DebugInputService(_inputAdapter);
            _mainGameplayInput = new MainGameplayInputService(_inputAdapter);
            _inputServiceSwitcher.SwitchTo(_debugInput);
        }

        private void PlayerInit()
        {
            var player = Instantiate(_playerPrefab, _playerSpawnPoint.position, quaternion.identity, _playerParentObj);
            _playerController = player.GetComponent<PlayerController>();
            _cameraMover.BindPlayer(player.transform);
        }

        private void InputHandle()
        {
            _inputServiceSwitcher.Current?.Tick(Time.deltaTime);


            _mineView.ApplyChanges(_mineInteractionService.Changes);
            _mineInteractionService.ClearChanges();
        }

        private void DebugModeExecute()
        {
            if (_inputServiceSwitcher?.Current is not DebugInputService service)
                return;

            var commands = service.Commands;
            if (commands.PointerClick.TryConsume())
            {
                int x = Mathf.FloorToInt(commands.PointerWorldPosition.x);
                int y = Mathf.FloorToInt(commands.PointerWorldPosition.y);

                _mineInteractionService.TryDamageBlock(x, -y, _debugBlockDamage);
            }

            _cameraMover.Move(commands.Move);

            if (commands.ToggleModePressed.TryConsume())
            {
                _cameraMover.enabled = false;
                _inputServiceSwitcher.SwitchTo(_mainGameplayInput);
            }
        }

        private void MainGameplayModeExecute()
        {
            if (_inputServiceSwitcher?.Current is not MainGameplayInputService inputService)
                return;

            var commands = inputService.Commands;

            _playerController.Execute(commands);

            if (commands.ToggleModePressed.TryConsume())
            {
                _cameraMover.enabled = true;
                _inputServiceSwitcher.SwitchTo(_debugInput);
            }
        }
    }
}
