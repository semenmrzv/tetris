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

        // НОВОЕ: Вызываем метод начала игры
        StartGame();
    }

    public void SpawnNextShape()
    {
        // Создаём переменную nextShape, в которую записываем следующую фигуру, сгенерированную ShapeSpawner
        Shape nextShape = ShapeSpawner.SpawnNextShape();

        // Устанавливаем следующую фигуру в ShapeMover, который отвечает за перемещение фигур
        ShapeMover.SetTargetShape(nextShape);

        // Сдвигаем фигуру в заданную позицию на игровом поле
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - GameField.InvisibleYFieldSize + nextShape.ExtraSpawnYMove));
    }
    // Экран игры 
    public GameObject GameScreen;

    // Экран конца игры
    public GameObject GameEndScreen;

    public SpriteMask mask;

    private void StartGame()
    {
        // Показываем новую фигуру
        SpawnNextShape();

        // Устанавливаем экран игры
        SwitchScreens(true);

        // Включаем движение фигур
        ShapeMover.SetActive(true);

        mask.enabled = true;
    }

    public void EndGame()
    {
        // Устанавливаем экран конца игры
        SwitchScreens(false);

        // Отключаем движение фигур
        ShapeMover.SetActive(false);

        mask.enabled = false;
    }

    private void SwitchScreens(bool isGame)
    {
        // Активируем экран игры
        GameScreen.SetActive(isGame);

        // Скрываем экран завершения игры
        GameEndScreen.SetActive(!isGame);
    }

    public void RestartGame()
    {
        // Удаляем все фигуры в игре
        ShapeMover.DestroyAllShapes();

        // Начинаем новую игру
        StartGame();
    }
}

