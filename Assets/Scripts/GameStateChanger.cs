using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    // Скрипт игрового поля
    public GameField GameField;

    // Скрипт движения фигур
    public ShapeMover ShapeMover;

    private void Start()
    {
        // Вызываем метод FirstStartGame();
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        // Заполняем ячейки игрового поля
        GameField.FillCellsPositions();

        // Вычисляем позицию, куда будет двигаться фигура
        // Это временное решение, чтобы потестировать перемещение первой фигуры
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 2));
    }
}
