using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;

            if (newPartCellId.x < 0 || newPartCellId.y < 0
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                return false;
            }
            // �����: ���� ������ � ����� ������� ����� ������ ������ ������ �������
            else if (!GameField.GetCellEmpty(newPartCellId))
            {
                // ���������� false (������������� �����������)
                return false;
            }
        }
        return true;
    }

    // �����: �������� Try (��������) � ��������
    private bool TrySetShapeInCells()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            // �����: ���� ������, � ������� �� �������� ���������� ������, ��� ������
            if (!GameField.GetCellEmpty(newPartCellId))
            {
                // �����: ���������� false
                return false;
            }
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            _targetShape.Parts[i].CellId = newPartCellId;
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
        // �����: ���� ������ ����� � ��������� ������, ���������� true
        return true;
    }

    // ������ �������� ����
    public GameField GameField;

    // ������� ������
    private Shape _targetShape;

    // �������� �������� ����
    public float MoveDownDelay = 0.8f;

    // ������ �������� ����
    private float _moveDownTimer = 0;

    public void MoveShape(Vector2Int deltaMove)
    {
        // ���� ����������� �� deltaMove ����������
        if (!CheckMovePossible(deltaMove))
        {
            // ������� �� ������
            return;
        }
        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ��������� ����� ����� ������ ��� ������� ����� ������
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;

            // ��������� ����� ������� ��� ������� ����� ������
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // ��������� �������� ������ ��� ������� ����� ������
            _targetShape.Parts[i].CellId = newPartCellId;

            // ������������� ����� ������� ��� ������� ����� ������
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // ������ ��������� ��������� ����
    public GameStateChanger GameStateChanger;    // ������ ��������� ��������� ����

    private void Update()
    {
        // ���� ���� �� �������
        if (!_isActive)
        {
            // ������� �� ������
            return;
        }

        // �� ���������, ��� ���� � Update()

        // �����: ���������� ����� ������� ������ ��� ������
        SetShapePartCellsEmpty(true);

        HorizontalMove();
        VerticalMove();
        Rotate();

        // �����: ���������, �������� �� ������� ������ ������ ������� ����
        bool reachBottom = CheckBottom();

        // �����: ���������, ���� �� ������ ������ ��� ������� ������� �� ����
        bool reachOtherShape = CheckOtherShape();

        // �����: ���������� ����� ������� ������ ��� �����������
        SetShapePartCellsEmpty(false);

        if (reachBottom || reachOtherShape)
        {
            // ���� ������ ��������� �������
            if (CheckShapeTopOver())
            {
                // ��������� ����
                GameStateChanger.EndGame();
            }
            // �����
            else
            {
                // ��������� ����� ������
                GameStateChanger.SpawnNextShape();
            }
        }
    }

    private void HorizontalMove()
    {
        // ���� ���� ������ ������� ����� ��� A
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // ���������� ������ �����
            MoveShape(Vector2Int.left);
        }
        // �����, ���� ���� ������ ������� ������ ��� D
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // ���������� ������ ������
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        // ����������� ������ �� �������� ���������� �������
        _moveDownTimer += Time.deltaTime;

        // ���� ������ ���������� ������� ��� ���� ������ ������� ���� ��� S
        if (_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // �������� ������
            _moveDownTimer = 0;

            // ���������� ������ ����
            MoveShape(Vector2Int.down);
        }
    }



    public void SetTargetShape(Shape targetShape)
    {
        _targetShape = targetShape;
        _shapes.Add(targetShape);
    }

    private bool CheckBottom()
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���������, ��������� �� ������� ����� ������ �� ������ ������� �������� ���� (������ � �������� y, ������ 0)
            if (_targetShape.Parts[i].CellId.y == 0)
            {
                // ���� ���� �� ���� ����� ������ ��������� �� ������ ������� �������� ����, ���������� true
                return true;
            }
        }
        // ���� �� ���� ����� ������ �� ��������� �� ������ ������� �������� ����, ���������� false
        return false;
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // �����: ������ ����� ������ startCellIds 
            // ����������� ��� �������� ������� ������� ����� ������
            Vector2Int[] startCellIds = _targetShape.GetPartCellIds();

            _targetShape.Rotate();
            UpdateByWalls();
            UpdateByBottom();

            // �����: �������� ���������� ������ � ������ ����
            bool shapeSetted = TrySetShapeInCells();

            // �����: ���� ������ �� ���������� ����������
            if (!shapeSetted)
            {
                // �����: ���������� � �� �������� �������
                MoveShapeToCellIds(_targetShape, startCellIds);
            }
        }
    }

    private void SetShapeInCells()
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // �������� ������� ������� ����� ������
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;

            // �������� ��������� ����� ������ �� ������� ����
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            // �������� ������� ������ �� ������� ����
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // ������������� ����� ������ ��� ����� ������
            _targetShape.Parts[i].CellId = newPartCellId;

            // ������������� ������� ����� ������ � ����� ������
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void UpdateByWalls()
    {
        // ��������� ������� ������ ������������ ������ �����
        UpdateByWall(true);

        // ��������� ������� ������ ������������ ����� �����
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        // �������� �� ���� ������ ������ �� i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���� ����� ������ ������� �� �����
            if (CheckWallOver(_targetShape.Parts[i], right))
            {
                // �������� �� ���� ������ ������ �� j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // ������� ����� ������ � ��������������� ������� (����� ��� ������) �� ���� ������
                    _targetShape.Parts[j].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
                }
            }
        }
    }

    private bool CheckWallOver(ShapePart part, bool right)
    {
        // ����� ������� ���������� �� �����
        float wallDistance = 0;

        // ���� ����������� ������ �����
        if (right)
        {
            // ��������� ���������� ����� �������� ����� ������ � ������ ������
            wallDistance = part.transform.position.x - (GameField.FirstCellPoint.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance > 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ ������� �� �����
                return true;
            }
        }
        // �����, ���� ����������� ����� �����
        else
        {
            // ��������� ���������� ����� �������� ����� ������ � ����� ������
            wallDistance = part.transform.position.x - GameField.FirstCellPoint.position.x;

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance < 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ ������� �� �����
                return true;
            }
        }
        // ���������� false, ����� �� ���� ����� ������ �� ������� �� �����
        return false;
    }

    private float GetRoundedWallDistance(float distance)
    {
        // ����� ����� ��� ���������� �� ���� ������ ����� �������
        int roundValue = 100;

        // ��������� ���������� �� ���������� ���������� ������ ����� �������
        distance = Mathf.Round(distance * roundValue);

        // ���������� ���������� �������� ����������
        return distance;
    }

    private void UpdateByBottom()
    {
        // �������� �� ���� ������ ������ �� i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���� ����� ������ ������� �� ���
            if (CheckBottomOver(_targetShape.Parts[i]))
            {
                // �������� �� ���� ������ ������ �� j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // ������� ����� ������ �� ���� ������ �����
                    _targetShape.Parts[j].transform.position += Vector3.up * GameField.CellSize.y;
                }
            }
        }
    }

    private bool CheckBottomOver(ShapePart part)
    {
        // ��������� ���������� ����� �������� ����� ������ � �����
        float wallDistance = part.transform.position.y - GameField.FirstCellPoint.position.y;

        // ��������� ���������� �� ���������� ������ �����
        wallDistance = GetRoundedWallDistance(wallDistance);

        // ���� ���������� �� ����� 0 � ������������
        if (wallDistance != 0 && wallDistance < 0)
        {
            // ���������� true, ����� ��������, ��� ����� ������ ������� �� ���
            return true;
        }
        // ���������� false, ����� �� ���� ����� ������ �� ������� �� ���
        return false;
    }

    // ��������� ������� � ������ �������
    private bool CheckOtherShape()
    {
        // �������� �� ���� ������ ������� ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���� ������ ��� ������� ������� �� ������ (� ��� ��� ���� ����� ������ ������)
            if (!GameField.GetCellEmpty(_targetShape.Parts[i].CellId + Vector2Int.down))
            {
                // ���������� true
                return true;
            }
        }
        // ���������� false
        return false;
    }
    // ������ ��������� ������ ������� ������
    private void SetShapePartCellsEmpty(bool value)
    {
        // �������� �� ���� ������ ������� ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ������������� ��������� ������ (������ ��� ���) ��� ������ ����� ������
            GameField.SetCellEmpty(_targetShape.Parts[i].CellId, value);
        }
    }

    // ���������� ������ �� ��������� ������� �����
    private void MoveShapeToCellIds(Shape shape, Vector2Int[] cellIds)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < shape.Parts.Length; i++)
        {
            // ���������� i-��� ����� ������ �� ������� ������ � �������� i � ������� cellIds
            MoveShapePartToCellId(shape.Parts[i], cellIds[i]);
        }
    }
    // ���������� ����� ������ �� ��������� ������� ������
    private void MoveShapePartToCellId(ShapePart part, Vector2Int cellId)
    {
        // �������� ����� ������� ��� ����� ������ �� ������ �������� ������� ������
        Vector2 newPartPosition = GameField.GetCellPosition(cellId);

        // ����������� ����� ������ ����� ������� ������
        part.CellId = cellId;

        // ������������� ������� ����� ������ �� ������� ����
        part.SetPosition(newPartPosition);
    }

    // ���� ���������� ����
    private bool _isActive;

    public void SetActive(bool value)
    {
        // ����������� ���������� _isActive �������� value
        _isActive = value;
    }

    private bool CheckShapeTopOver()
    {
        // ��������� ������� ����� ������� ������ �� ������� ����
        float topCellYPosition = GameField.FirstCellPoint.position.y + (GameField.FieldSize.y - GameField.InvisibleYFieldSize - 2) * GameField.CellSize.y;

        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ��������� ���������� ����� �������� ����� ������ � ��������
            float wallDistance = _targetShape.Parts[i].transform.position.y - topCellYPosition;

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance > 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ �������� �������
                return true;
            }
        }
        // ���������� false, ����� �� ���� ����� ������ �� �������� �������
        return false;
    }

    // ������ ���� ����� �� ����
    private List<Shape> _shapes = new List<Shape>();

    public void DestroyAllShapes()
    {
        // ���������� ��� ������ � ������ � ���������� ������ ������
        foreach (Shape shape in _shapes)
        {
            Destroy(shape.gameObject);
        }

        // ������� ������ �����
        _shapes.Clear();
    }
}
