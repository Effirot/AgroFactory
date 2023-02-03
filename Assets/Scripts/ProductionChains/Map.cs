using UnityEngine;

public class Map {
    public delegate Cell CreateCell(Vector2 mapCoordinate);

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
                _mapCells[Index(x, y)] = _createCell(new Vector2(x, y));
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
        var rawPosition = _mapCells[Index(x, y)].transform.position;
        rawPosition.y = 0.0f;
        _mapCells[Index(x, y)].transform.position = rawPosition;
    }

    private void DeformCell(Vector2Int point, float remoteness) {
        DeformCell(point.x, point.y, remoteness);
    }

    private void DeformCell(int x, int y, float remoteness) {
        var rawPosition = _mapCells[Index(x, y)].transform.position;
        rawPosition.y = remoteness is not 0.0f ? 1.0f / remoteness * _maxHeight : _maxHeight;
        _mapCells[Index(x, y)].transform.position = rawPosition;
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
