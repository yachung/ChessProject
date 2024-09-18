
using VContainer;
using Fusion.Addons.FSM;
using Fusion;

public class StagePresenter : NetworkBehaviour
{
    [Inject] private readonly StageView view;
    [Inject] private readonly StageModel model;

    public void StageViewInitialize(StageStateBehaviour state)
    {
        view.progressBar.gameObject.SetActive(state.StateDuration > 0);
    }

    public void OnStageEnter(StageStateBehaviour state)
    {
        view.DisplayStageName(model.StageName);
    }

    public void UpdateProgressBar(float progress)
    {
        view.UpdateProgressBar(progress);
    }

    public bool IsLastRound()
    {
        return model.RoundIndex > 5;
    }
}
