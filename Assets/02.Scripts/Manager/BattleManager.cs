using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class BattleManager
{
    [Inject] private BattleController battleController;
    
    private PlayerField playerField;
    private TileManager tileManager;
    private ChampionManager championManager;

    public void Initialize(PlayerField playerField, TileManager tileManager, ChampionManager championManager)
    {
        this.playerField = playerField;
        this.tileManager = tileManager;
        this.championManager = championManager;
    }

    public void HandleBattleStateChange(bool isBattle)
    {
        if (isBattle)
        {
            StartBattle();
        }
        else
        {
            EndBattle();
        }
    }

    public void StartBattle()
    {
        playerField.IsBattle = true;
        battleController.StartBattle();
        Debug.Log("Battle started!");
    }

    public void EndBattle()
    {
        playerField.IsBattle = false;
        
        battleController.BattleEnd();

        tileManager.ClearAllTiles();
        championManager.RespawnChampions();
        Debug.Log("Battle ended!");
    }

    public void InitializeOpponentField(List<Tile> opponentTiles)
    {
        foreach (var tile in opponentTiles)
        {
            Vector2Int opponentCoord = Utils.TransformToOpponentCoordinate(tile.Coordinate, tileManager.TileRows, tileManager.TileColumns);

            if (tile.Champion != null)
            {
                tile.Champion.IsAwayTeam = true;
                tileManager.SpawnChampion(opponentCoord, tile.Champion, isOpponentField: true);
            }
        }
    }
}
