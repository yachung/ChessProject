

using Fusion;

public class BattleReadyState : StageStateBehaviour
{
    protected override bool CanEnterState()
    {
        bool result = base.CanEnterState();

        switch (Machine.ActiveState)
        {
            case SelectObjectState:
                result &= true;
                break;
            case BattleState:
                result &= stageModel.IsLastRound() ? false : true;
                break;
        }

        return result;
    }

    protected override void OnEnterState()
    {
        base.OnEnterState();

        stageModel.MatchingPlayer();
    }

    protected override void OnEnterStateRender()
    {
        base.OnEnterStateRender();

        _shopPresenter.ShowView();
        _shopPresenter.OnRefreshShop();
    }

    protected override void OnExitState()
    {
        base.OnExitState();

        foreach (var pair in stageModel.matchingPairs)
        {
            fieldManager.MovePlayerToField(pair.Value, pair.Key);
            
            PlayerField targetField = fieldManager.GetFieldByPlayerRef(pair.Key);
            PlayerField sourceField = fieldManager.GetFieldByPlayerRef(pair.Value);

            targetField.BattleInitializeForEnemy(sourceField.GetTiles(TileType.BattleTile));
            targetField.BattleInitializeForEnemy(sourceField.GetTiles(TileType.WaitTile));
        }
    }

    protected override void OnExitStateRender()
    {
        base.OnExitStateRender();
    }
}
