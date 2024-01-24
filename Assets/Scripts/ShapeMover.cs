using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Вычисляем новый номер ячейки для текущей части фигуры
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;

            // Если новая позиция выходит за границы игрового поля
            if (newPartCellId.x < 0 || newPartCellId.y < 0
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                // Возвращаем false
                return false;
            }
        }
        // Иначе возвращаем true
        return true;
    }
    // Скрипт изменения состояния игры
    public GameStateChanger GameStateChanger;

    // Скрипт игрового поля
    public GameField GameField;

    // Целевая фигура
    private Shape _targetShape;

    // Задержка движения вниз
    public float MoveDownDelay = 0.8f;

    // Таймер движения вниз
    private float _moveDownTimer = 0;

    public void MoveShape(Vector2Int deltaMove)
    {
        // Если перемещение на deltaMove невозможно
        if (!CheckMovePossible(deltaMove))
        {
            // Выходим из метода
            return;
        }
        // Проходим по всем частям фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Вычисляем новый номер ячейки для текущей части фигуры
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;

            // Вычисляем новую позицию для текущей части фигуры
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // Обновляем значение ячейки для текущей части фигуры
            _targetShape.Parts[i].CellId = newPartCellId;

            // Устанавливаем новую позицию для текущей части фигуры
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        HorizontalMove();
        VerticalMove();

        // НОВОЕ: Поворачиваем фигуру
        Rotate();

        if (CheckBottom())
        {
            GameStateChanger.SpawnNextShape();
        }
    }

    private void HorizontalMove()
    {
        // Если была нажата клавиша влево или A
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // Перемещаем фигуру влево
            MoveShape(Vector2Int.left);
        }
        // Иначе, если была нажата клавиша вправо или D
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // Перемещаем фигуру вправо
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        // Увеличиваем таймер на значение прошедшего времени
        _moveDownTimer += Time.deltaTime;

        // Если прошло достаточно времени или была нажата клавиша вниз или S
        if (_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // Обнуляем таймер
            _moveDownTimer = 0;

            // Перемещаем фигуру вниз
            MoveShape(Vector2Int.down);
        }
    }

    public void SetTargetShape(Shape targetShape)
    {
        _targetShape = targetShape;
    }

    private bool CheckBottom()
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Проверяем, находится ли текущая часть фигуры на нижней границе игрового поля (ячейка с индексом y, равным 0)
            if (_targetShape.Parts[i].CellId.y == 0)
            {
                // Если хотя бы одна часть фигуры находится на нижней границе игрового поля, возвращаем true
                return true;
            }
        }
        // Если ни одна часть фигуры не находится на нижней границе игрового поля, возвращаем false
        return false;
    }

    private void Rotate()
    {
        // Если нажата клавиша вверх или W
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // Поворачиваем фигуру
            _targetShape.Rotate();

            // Обновляем позицию фигуры при столкновении со стенами
            UpdateByWalls();

            // Обновляем позицию фигуры при столкновении с низом
            UpdateByBottom();

            // Устанавливаем позицию фигуры в ячейках
            SetShapeInCells();
        }
    }

    private void SetShapeInCells()
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Получаем текущую позицию части фигуры
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;

            // Получаем ближайший номер ячейки на игровом поле
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            // Получаем позицию ячейки на игровом поле
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // Устанавливаем номер ячейки для части фигуры
            _targetShape.Parts[i].CellId = newPartCellId;

            // Устанавливаем позицию части фигуры в новой ячейке
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void UpdateByWalls()
    {
        // Обновляем позицию фигуры относительно правой стены
        UpdateByWall(true);

        // Обновляем позицию фигуры относительно левой стены
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        // Проходим по всем частям фигуры по i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Если часть фигуры выходит за стену
            if (CheckWallOver(_targetShape.Parts[i], right))
            {
                // Проходим по всем частям фигуры по j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // Двигаем часть фигуры в противоположную сторону (влево или вправо) на одну ячейку
                    _targetShape.Parts[j].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
                }
            }
        }
    }


    private bool CheckWallOver(ShapePart part, bool right)
    {
        // Задаём нулевое расстояние до стены
        float wallDistance = 0;

        // Если проверяется правая стена
        if (right)
        {
            // Вычисляем расстояние между позицией части фигуры и правой стеной
            wallDistance = part.transform.position.x - (GameField.FirstCellPoint.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);

            // Округляем расстояние до ближайшего целого числа
            wallDistance = GetRoundedWallDistance(wallDistance);

            // Если расстояние не равно 0 и положительно
            if (wallDistance != 0 && wallDistance > 0)
            {
                // Возвращаем true, чтобы показать, что часть фигуры выходит за стену
                return true;
            }
        }
        // Иначе, если проверяется левая стена
        else
        {
            // Вычисляем расстояние между позицией части фигуры и левой стеной
            wallDistance = part.transform.position.x - GameField.FirstCellPoint.position.x;

            // Округляем расстояние до ближайшего целого числа
            wallDistance = GetRoundedWallDistance(wallDistance);

            // Если расстояние не равно 0 и отрицательно
            if (wallDistance != 0 && wallDistance < 0)
            {
                // Возвращаем true, чтобы показать, что часть фигуры выходит за стену
                return true;
            }
        }
        // Возвращаем false, когда ни одна часть фигуры не выходит за стену
        return false;
    }

    private float GetRoundedWallDistance(float distance)
    {
        // Задаём число для округления до двух знаков после запятой
        int roundValue = 100;

        // Округляем расстояние до указанного количества знаков после запятой
        distance = Mathf.Round(distance * roundValue);

        // Возвращаем округлённое значение расстояния
        return distance;
    }


    private void UpdateByBottom()
    {
        // Проходим по всем частям фигуры по i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Если часть фигуры выходит за пол
            if (CheckBottomOver(_targetShape.Parts[i]))
            {
                // Проходим по всем частям фигуры по j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // Двигаем часть фигуры на одну ячейку вверх
                    _targetShape.Parts[j].transform.position += Vector3.up * GameField.CellSize.y;
                }
            }
        }
    }

    private bool CheckBottomOver(ShapePart part)
    {
        // Вычисляем расстояние между позицией части фигуры и полом
        float wallDistance = part.transform.position.y - GameField.FirstCellPoint.position.y;

        // Округляем расстояние до ближайшего целого числа
        wallDistance = GetRoundedWallDistance(wallDistance);

        // Если расстояние не равно 0 и отрицательно
        if (wallDistance != 0 && wallDistance < 0)
        {
            // Возвращаем true, чтобы показать, что часть фигуры выходит за пол
            return true;
        }
        // Возвращаем false, когда ни одна часть фигуры не выходит за пол
        return false;
    }


}
