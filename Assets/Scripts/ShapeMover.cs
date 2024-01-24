using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            // Вычисляем новый номер ячейки для текущей части фигуры
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;

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

    // Скрипт игрового поля
    public GameField GameField;

    // Целевая фигура
    public Shape TargetShape;

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
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            // Вычисляем новый номер ячейки для текущей части фигуры
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;

            // Вычисляем новую позицию для текущей части фигуры
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // Обновляем значение ячейки для текущей части фигуры
            TargetShape.Parts[i].CellId = newPartCellId;

            // Устанавливаем новую позицию для текущей части фигуры
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // Вызываем метод горизонтального движения
        HorizontalMove();

        // Вызываем метод вертикального движения
        VerticalMove();
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
}
