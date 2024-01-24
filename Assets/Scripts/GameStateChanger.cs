using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    // Скрипт игрового поля
    public GameField GameField;

    // Скрипт движения фигур
    public ShapeMover ShapeMover;
    // Скрипт появления фигур
    public ShapeSpawner ShapeSpawner;

    private void Start()
    {
        // Вызываем метод FirstStartGame();
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPositions();

        // НОВОЕ: Вызываем метод SpawnNextShape()
        SpawnNextShape();
    }
    public void SpawnNextShape()
    {
        // Создаём переменную nextShape, в которую записываем следующую фигуру, сгенерированную ShapeSpawner
        Shape nextShape = ShapeSpawner.SpawnNextShape();

        // Устанавливаем следующую фигуру в ShapeMover, который отвечает за перемещение фигур
        ShapeMover.SetTargetShape(nextShape);

        // Сдвигаем фигуру в заданную позицию на игровом поле
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 3));
    }
}
