using UnityEngine;

public class Map {
    public delegate Cell CreateCell(Vector2Int mapCoordinate);

    private readonly CreateCell _createCell;
    private readonly Vector2Int _mapSize;
    private readonly int _radiusOfShapeDeformation;
    private readonly float _maxHeight;

    private Vector2Int _lastCursor;

    private Cell[] _mapCells;

    public Map(CreateCell createCell, Vector2Int mapSize, int radiusOfShapeDeformation, float maxHeight)
    {
        _createCell = createCell;
        _mapSize = mapSize;
        _radiusOfShapeDeformation = radiusOfShapeDeformation;
        _maxHeight = maxHeight;
    }

    public void GenerateMap() {
        _mapCells = new Cell[_mapSize.x * _mapSize.y];

        for (int x = 0; x < _mapSize.x; x++) {
            for (int y = 0; y < _mapSize.y; y++) {
                var index = Index(x, y);
                _mapCells[index] = _createCell(new Vector2Int(x, y));
                _mapCells[index].Init();
            }
        }
    }

    public void Update() {
        for (int x = 0; x < _mapSize.x; x++) {
            for (int y = 0; y < _mapSize.y; y++) {
                _mapCells[Index(x, y)].Update();
            }
        }
    }

    public void ChangeShape(Vector2Int cursorPosition) {
        RestoreShape(_lastCursor);
        DeformShape(cursorPosition);

        _lastCursor = cursorPosition;
    }

    public void OccupyCell(Vector2Int cursorPosition) {
        if (IsOutBound(cursorPosition)) {
            return;
        }

        _mapCells[Index(cursorPosition)].Occupy();
    }

    public void PrepareCell(Vector2Int cursorPosition, Vector2Int size) {
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                var point = cursorPosition + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    return;
                }

                _mapCells[Index(point)].Prepare();
            }
        }
    }

    public void HighLightCell(Vector2Int cursorPosition, Vector2Int size) {
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                var point = cursorPosition + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    return;
                }

                _mapCells[Index(point)].HighLight();
            }
        }
    }

    public MapBuild GetBuild(Vector2Int cursorPosition) {
        if (IsOutBound(cursorPosition)) {
            return default;
        }

        return _mapCells[Index(cursorPosition)].Build;
    }

    public bool IsOccupy(Vector2Int cursorPosition) {
        if (IsOutBound(cursorPosition)) {
            return true;
        }

        return _mapCells[Index(cursorPosition)].IsOccupied;
    }

    public bool IsFree(Vector2Int cursorPosition) {
        if (IsOutBound(cursorPosition)) {
            return true;
        }

        return _mapCells[Index(cursorPosition)].IsFree;
    }

    public bool IsSectorFree(Vector2Int cursorPosition, Vector2Int size) {
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                var point = cursorPosition + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    return false;
                }

                if (_mapCells[Index(point)].IsOccupied) {
                    return false;
                }
            }
        }

        return true;
    }

    public void OccupySector(Vector2Int cursorPosition, Vector2Int size, MapBuild build) {
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                var point = cursorPosition + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    return;
                }

                var index = Index(point);

                _mapCells[index].Occupy();
                _mapCells[index].SetBuild(build);
            }
        }
    }

    private void RestoreShape(Vector2Int center) {
        for (int x = -_radiusOfShapeDeformation; x <= _radiusOfShapeDeformation; x++) {
            for (int y = -_radiusOfShapeDeformation; y <= _radiusOfShapeDeformation; y++) {
                var point = center + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    continue;
                }

                RestoreCell(point);
            }
        }
    }

    private void DeformShape(Vector2Int center) {
        for (int x = -_radiusOfShapeDeformation; x <= _radiusOfShapeDeformation; x++) {
            for (int y = -_radiusOfShapeDeformation; y <= _radiusOfShapeDeformation; y++) {
                var point = center + new Vector2Int(x, y);

                if (IsOutBound(point)) {
                    continue;
                }

                var radius = Mathf.Sqrt(x * x + y * y);

                if (radius > _radiusOfShapeDeformation) {
                    continue;
                }

                DeformCell(point, radius);
            }
        }
    }

    private void RestoreCell(Vector2Int point) {
        RestoreCell(point.x, point.y);
    }

    private void RestoreCell(int x, int y) {
        _mapCells[Index(x, y)].Restore();
    }

    private void DeformCell(Vector2Int point, float remoteness) {
        DeformCell(point.x, point.y, remoteness);
    }

    private void DeformCell(int x, int y, float remoteness) {
        var percentageOfRemoteness = ((float)_radiusOfShapeDeformation - remoteness) / (float)_radiusOfShapeDeformation;
        _mapCells[Index(x, y)].Deform(percentageOfRemoteness);
    }

    private bool IsOutBound(Vector2Int point) {
        return IsOutBound(point.x, point.y);
    }

    private bool IsOutBound(int x, int y) {
        var outOfX = 0 > x || x >= _mapSize.x;
        var outOfY = 0 > y || y >= _mapSize.y;
        return outOfX || outOfY;
    }

    private int Index(Vector2Int point) {
        return point.x * _mapSize.y + point.y;
    }

    private int Index(int x, int y) {
        return x * _mapSize.y + y;
    }
}
