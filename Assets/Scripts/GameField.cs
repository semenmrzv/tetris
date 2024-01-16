using UnityEngine;

public class GameField : MonoBehaviour


{
    private GameFieldCell GetCell(int x, int y)
    {
        // ≈сли номер €чейки некорректный
        if (x < 0 || y < 0 || x >= FieldSize.x || y >= FieldSize.y)
        {
            // ¬озвращаем пустое значение null
            return null;
        }
        // »наче возвращаем €чейку с заданными координатами в двухмерном массиве €чеек
        return _cells[x, y];
    }

    public Vector2 GetCellPosition(Vector2Int cellId)
    {
        // ѕолучаем €чейку по заданным координатам
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // ≈сли такой €чейки нет
        if (cell == null)
        {
            // ¬озвращаем нулевые значени€ (0, 0)
            return Vector2.zero;
        }
        // »наче возвращаем позицию €чейки
        return cell.GetPosition();
    }

    public void FillCellsPositions()
    {
        // —оздаЄм двухмерный массив €чеек с заданными размерами
        _cells = new GameFieldCell[FieldSize.x, FieldSize.y];

        // ѕроходим по первым координатам всех €чеек (i)
        for (int i = 0; i < FieldSize.x; i++)
        {
            // ѕроходим по вторым координатам всех €чеек (j)
            for (int j = 0; j < FieldSize.y; j++)
            {
                // ¬ычисл€ем позицию €чейки на основе позиции первой €чейки, размеров €чейки и значений i, j
                Vector2 cellPosition = (Vector2)FirstCellPoint.position + Vector2.right * i * CellSize.x + Vector2.up * j * CellSize.y;

                // —оздаЄм новую €чейку с вычисленной позицией
                GameFieldCell newCell = new GameFieldCell(cellPosition);

                // «аписываем созданную €чейку в соответствующую позицию двухмерного массива €чеек
                _cells[i, j] = newCell;
            }
        }
    }

    // ѕозици€ первой €чейки
    public Transform FirstCellPoint;

    // –азмер €чейки (по X и Y)
    public Vector2 CellSize;

    // –азмер пол€ (по X и Y)
    public Vector2Int FieldSize;

    // двухмерный массив из позиций каждой €чейки
    private GameFieldCell[,] _cells;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}




