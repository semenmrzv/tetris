using UnityEngine;

public class GameField : MonoBehaviour


{
    private GameFieldCell GetCell(int x, int y)
    {
        // ���� ����� ������ ������������
        if (x < 0 || y < 0 || x >= FieldSize.x || y >= FieldSize.y)
        {
            // ���������� ������ �������� null
            return null;
        }
        // ����� ���������� ������ � ��������� ������������ � ���������� ������� �����
        return _cells[x, y];
    }

    public Vector2 GetCellPosition(Vector2Int cellId)
    {
        // �������� ������ �� �������� �����������
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // ���� ����� ������ ���
        if (cell == null)
        {
            // ���������� ������� �������� (0, 0)
            return Vector2.zero;
        }
        // ����� ���������� ������� ������
        return cell.GetPosition();
    }

    public void FillCellsPositions()
    {
        // ������ ���������� ������ ����� � ��������� ���������
        _cells = new GameFieldCell[FieldSize.x, FieldSize.y];

        // �������� �� ������ ����������� ���� ����� (i)
        for (int i = 0; i < FieldSize.x; i++)
        {
            // �������� �� ������ ����������� ���� ����� (j)
            for (int j = 0; j < FieldSize.y; j++)
            {
                // ��������� ������� ������ �� ������ ������� ������ ������, �������� ������ � �������� i, j
                Vector2 cellPosition = (Vector2)FirstCellPoint.position + Vector2.right * i * CellSize.x + Vector2.up * j * CellSize.y;

                // ������ ����� ������ � ����������� ��������
                GameFieldCell newCell = new GameFieldCell(cellPosition);

                // ���������� ��������� ������ � ��������������� ������� ����������� ������� �����
                _cells[i, j] = newCell;
            }
        }
    }

    // ������� ������ ������
    public Transform FirstCellPoint;

    // ������ ������ (�� X � Y)
    public Vector2 CellSize;

    // ������ ���� (�� X � Y)
    public Vector2Int FieldSize;

    // ���������� ������ �� ������� ������ ������
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




