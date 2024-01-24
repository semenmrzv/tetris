using UnityEngine;

public class GameFieldCell : MonoBehaviour
{
    // ������� ������ �� ������� ����
    private Vector2 _position;

    // ���� ������� ������
    private bool _isEmpty;

    // ������ ������ ������ GameFieldCell
    public GameFieldCell(Vector2 position)
    {
        // ����������� ������� ���������� ��������
        _position = position;

        // ������ ������ ���������� ������
        _isEmpty = true;
    }
    // �������� ������� ������
    public Vector2 GetPosition()
    {
        // ���������� �������� _position
        return _position;
    }
    // ������ ������ ������ ��� �����������
    public void SetIsEmpty(bool value)
    {
        // ������������� ���� ������� ��� �������������
        _isEmpty = value;
    }
    // �����, ����� �� ������
    public bool GetIsEmpty()
    {
        // ���������� �������� _isEmpty
        return _isEmpty;
    }
}