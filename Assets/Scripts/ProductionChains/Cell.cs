using UnityEngine;

public class Cell : MonoBehaviour {
    private enum OccupationStatus {
        Free,
        Occupied,
    }

    private OccupationStatus _status;

    public bool IsOccupied => _status is OccupationStatus.Occupied;
    public bool IsFree => _status is OccupationStatus.Free;


    public void Prepare() {

    }

    public void Occupy() {
        _status = OccupationStatus.Occupied;
    }
}
