using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    // ������ �������� ����
    public GameField GameField;

    // ������ �������� �����
    public ShapeMover ShapeMover;
    // ������ ��������� �����
    public ShapeSpawner ShapeSpawner;

    private void Start()
    {
        // �������� ����� FirstStartGame();
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPositions();

        // �����: �������� ����� SpawnNextShape()
        SpawnNextShape();
    }
    public void SpawnNextShape()
    {
        // ������ ���������� nextShape, � ������� ���������� ��������� ������, ��������������� ShapeSpawner
        Shape nextShape = ShapeSpawner.SpawnNextShape();

        // ������������� ��������� ������ � ShapeMover, ������� �������� �� ����������� �����
        ShapeMover.SetTargetShape(nextShape);

        // �������� ������ � �������� ������� �� ������� ����
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 3));
    }
}
