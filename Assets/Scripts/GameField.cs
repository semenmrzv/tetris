using UnityEngine;

public class GameField : MonoBehaviour


{// ���������� ������� ����� ������ ����
    public int InvisibleYFieldSize = 4;

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

    public Vector2 GetCellPosition(int x, int y)
    {
        // �������� ������ �� � �����������
        GameFieldCell cell = GetCell(x, y);

        // ���� ����� ������ ���
        if (cell == null)
        {
            // ���������� ������� �������� (0, 0)
            return Vector2.zero;
        }
        // ����� ���������� ������� ������
        return cell.GetPosition();
    }

    public Vector2Int GetNearestCellId(Vector2 position)
    {
        // ���������� � ���������� resultDistance ����������� ��������� �������� ����� ����������� �������� � ������� ����
        // �� ���� �� ������� ��������� ������ � ��������
        float resultDistance = float.MaxValue;

        // ���������� � ���������� resultX � resultY ����
        int resultX = 0, resultY = 0;

        // �������� �� ���� ��������� X �������� ����
        for (int i = 0; i < FieldSize.x; i++)
        {
            // �������� �� ���� ��������� Y �������� ����
            for (int j = 0; j < FieldSize.y; j++)
            {
                // �������� ������� ������ � ������������ i, j
                Vector2 cellPosition = GetCellPosition(i, j);

                // ��������� ���������� ����� ������� ������� � ���������� ��������
                float distance = (cellPosition - position).magnitude;

                // ���� ������� ���������� ������ resultDistance
                if (distance < resultDistance)
                {
                    // ���������� � resultDistance ����� �������� distance
                    resultDistance = distance;

                    // ���������� � resultX ����� �������� i
                    resultX = i;

                    // ���������� � resultY ����� �������� j
                    resultY = j;
                }
            }
        }
        // ���������� ����� ������ Vector2Int, ������� �������� ����� �������� ������
        return new Vector2Int(resultX, resultY);
    }

    // ������ ������ ������
    public void SetCellEmpty(Vector2Int cellId, bool value)
    {
        // �������� ������ �� ��������� �����������
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // ���� ����� ������ ���
        if (cell == null)
        {
            // ������� �� ������
            return;
        }
        // ������������� �������� ������� ������
        cell.SetIsEmpty(value);
    }
    // �������� �������� ������� ������
    public bool GetCellEmpty(Vector2Int cellId)
    {
        // �������� ������ �� ��������� �����������
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // ���� ����� ������ ���
        if (cell == null)
        {
            // ���������� false
            return false;
        }
        // ���������� �������� ������� ������
        return cell.GetIsEmpty();
    }
}




