using System.Collections.Generic;

public class BuildSystem {
    private enum BuildingStatus {
        Wait,
        SelectFactory,
        FindPlace,
        Build,
    }

    public enum StageMove {
        Next,
        Previous,
    }

    private readonly Dictionary<(BuildingStatus, StageMove), BuildingStatus> _stages;

    private BuildingStatus _stage;

    public BuildSystem() {
        _stages = new() {
            { (BuildingStatus.Wait, StageMove.Next), BuildingStatus.SelectFactory },
            { (BuildingStatus.SelectFactory, StageMove.Next), BuildingStatus.FindPlace },
            { (BuildingStatus.FindPlace, StageMove.Next), BuildingStatus.Build },
            { (BuildingStatus.Build, StageMove.Previous), BuildingStatus.FindPlace },
        };

        ResetStages();
    }

    public bool IsNeedSelect => _stage is BuildingStatus.SelectFactory;
    public bool IsFindPlace => _stage is BuildingStatus.FindPlace;
    public bool CanBuild => _stage is BuildingStatus.Build;

    public void MoveNextBuildStage() {
        if (_stages.TryGetValue((_stage, StageMove.Next), out var newStage)) {
            _stage = newStage;
        }
    }

    public void MovePreviousBuildStage() {
        if (_stages.TryGetValue((_stage, StageMove.Previous), out var newStage)) {
            _stage = newStage;
        }
    }

    public void CancelBuild() {
        ResetStages();
    }

    public void ResetStages() {
        _stage = BuildingStatus.Wait;
    }
}
