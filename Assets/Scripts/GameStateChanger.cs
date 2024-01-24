using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    // ������ �������� ����
    public GameField GameField;

    // ������ �������� �����
    public ShapeMover ShapeMover;

    private void Start()
    {
        // �������� ����� FirstStartGame();
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        // ��������� ������ �������� ����
        GameField.FillCellsPositions();

        // ��������� �������, ���� ����� ��������� ������
        // ��� ��������� �������, ����� ������������� ����������� ������ ������
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 2));
    }
}
