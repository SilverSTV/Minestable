using System;
using Cinemachine;
using Game.Scripts.Gameplay.Characters.Player;
using Game.Scripts.Root.Input;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public sealed class CameraMoveController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private float moveSpeed = 10f;
    private void OnEnable()
    {
        _cinemachineBrain.enabled = false;
        _virtualCamera.enabled = false;
    }
    private void OnDisable()
    {
        _cinemachineBrain.enabled = true;
        _virtualCamera.enabled = true;
    }

    public void BindPlayer(Transform playerTransform)
    {
        _virtualCamera.Follow = playerTransform;
    }
    
    public void Move(Vector2 move)
    {
        if(move == Vector2.zero)
            return;
        
        Vector3 direction = new Vector3(move.x, move.y, 0f);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
