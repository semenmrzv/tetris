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
            // НОВОЕ: Если ячейка в новой позиции части фигуры занята другой фигурой
            else if (!GameField.GetCellEmpty(newPartCellId))
            {
                // Возвращаем false (невозможность перемещения)
                return false;
            }
        }
        return true;
    }

    // НОВОЕ: Добавили Try (пытаться) в названии
    private bool TrySetShapeInCells()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            // НОВОЕ: Если ячейка, в которую мы пытаемся установить фигуру, уже занята
            if (!GameField.GetCellEmpty(newPartCellId))
            {
                // НОВОЕ: Возвращаем false
                return false;
            }
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            _targetShape.Parts[i].CellId = newPartCellId;
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
        // НОВОЕ: Если фигура вошла в свободные ячейки, возвращаем true
        return true;
    }

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


    // Скрипт изменения состояния игры
    public GameStateChanger GameStateChanger;    // Скрипт изменения состояния игры

    private void Update()
    {
        // Если игра не активна
        if (!_isActive)
        {
            // Выходим из метода
            return;
        }

        // Всё остальное, что было в Update()

        // НОВОЕ: Обозначаем блоки текущей фигуры как пустые
        SetShapePartCellsEmpty(true);

        HorizontalMove();
        VerticalMove();
        Rotate();

        // НОВОЕ: Проверяем, достигла ли текущая фигура нижней границы поля
        bool reachBottom = CheckBottom();

        // НОВОЕ: Проверяем, есть ли другие фигуры под текущей фигурой на поле
        bool reachOtherShape = CheckOtherShape();

        // НОВОЕ: Обозначаем блоки текущей фигуры как заполненные
        SetShapePartCellsEmpty(false);

        if (reachBottom || reachOtherShape)
        {
            // Если фигура коснулась потолка
            if (CheckShapeTopOver())
            {
                // Завершаем игру
                GameStateChanger.EndGame();
            }
            // Иначе
            else
            {
                // Запускаем новую фигуру
                GameStateChanger.SpawnNextShape();
            }
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
        _shapes.Add(targetShape);
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
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // НОВОЕ: Создаём новый массив startCellIds 
            // Присваиваем ему значения текущих позиций ячеек фигуры
            Vector2Int[] startCellIds = _targetShape.GetPartCellIds();

            _targetShape.Rotate();
            UpdateByWalls();
            UpdateByBottom();

            // НОВОЕ: Пытаемся установить фигуру в ячейки поля
            bool shapeSetted = TrySetShapeInCells();

            // НОВОЕ: Если фигуру не получилось установить
            if (!shapeSetted)
            {
                // НОВОЕ: Возвращаем её на исходную позицию
                MoveShapeToCellIds(_targetShape, startCellIds);
            }
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

    // Проверяем касание с другой фигурой
    private bool CheckOtherShape()
    {
        // Проходим по всем частям текущей фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Если ячейка под текущей фигурой не пустая (в ней уже есть часть другой фигуры)
            if (!GameField.GetCellEmpty(_targetShape.Parts[i].CellId + Vector2Int.down))
            {
                // Возвращаем true
                return true;
            }
        }
        // Возвращаем false
        return false;
    }
    // Меняем состояние блоков текущей фигуры
    private void SetShapePartCellsEmpty(bool value)
    {
        // Проходим по всем частям текущей фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Устанавливаем состояние ячейки (пустая или нет) для каждой части фигуры
            GameField.SetCellEmpty(_targetShape.Parts[i].CellId, value);
        }
    }

    // Перемещаем фигуру на указанные позиции ячеек
    private void MoveShapeToCellIds(Shape shape, Vector2Int[] cellIds)
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < shape.Parts.Length; i++)
        {
            // Перемещаем i-тую часть фигуры на позицию ячейки с индексом i в массиве cellIds
            MoveShapePartToCellId(shape.Parts[i], cellIds[i]);
        }
    }
    // Перемещаем часть фигуры на указанную позицию ячейки
    private void MoveShapePartToCellId(ShapePart part, Vector2Int cellId)
    {
        // Получаем новую позицию для части фигуры на основе заданной позиции ячейки
        Vector2 newPartPosition = GameField.GetCellPosition(cellId);

        // Присваиваем части фигуры новую позицию ячейки
        part.CellId = cellId;

        // Устанавливаем позицию части фигуры на игровом поле
        part.SetPosition(newPartPosition);
    }

    // Флаг активности игры
    private bool _isActive;

    public void SetActive(bool value)
    {
        // Присваиваем переменной _isActive значение value
        _isActive = value;
    }

    private bool CheckShapeTopOver()
    {
        // Вычисляем позицию самой верхней ячейки на игровом поле
        float topCellYPosition = GameField.FirstCellPoint.position.y + (GameField.FieldSize.y - GameField.InvisibleYFieldSize - 2) * GameField.CellSize.y;

        // Проходим по всем частям фигуры
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // Вычисляем расстояние между позицией части фигуры и потолком
            float wallDistance = _targetShape.Parts[i].transform.position.y - topCellYPosition;

            // Округляем расстояние до ближайшего целого числа
            wallDistance = GetRoundedWallDistance(wallDistance);

            // Если расстояние не равно 0 и положительно
            if (wallDistance != 0 && wallDistance > 0)
            {
                // Возвращаем true, чтобы показать, что часть фигуры касается потолка
                return true;
            }
        }
        // Возвращаем false, когда ни одна часть фигуры не касается потолка
        return false;
    }

    // Список всех фигур на поле
    private List<Shape> _shapes = new List<Shape>();

    public void DestroyAllShapes()
    {
        // Перебираем все фигуры в списке и уничтожаем каждую фигуру
        foreach (Shape shape in _shapes)
        {
            Destroy(shape.gameObject);
        }

        // Очищаем список фигур
        _shapes.Clear();
    }
}
