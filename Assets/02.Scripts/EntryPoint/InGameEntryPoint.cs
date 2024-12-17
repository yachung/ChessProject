using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class InGameEntryPoint : BaseEntryPoint
{
    /// <summary>
    /// Root Scope
    /// </summary>
    [Inject] private readonly GameManager _gameManager;

    /// <summary>
    /// InGame Scope
    /// </summary>
    [Inject] private readonly GameStateManager _gameStateManager;

    public override void Start()
    {
        // GameStateManager를 GameManager에 전달
        _gameManager.SetGameStateManager(_gameStateManager);
        Debug.Log("GameStateManager connected to GameManager via EntryPoint.");
    }
}
