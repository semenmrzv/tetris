using UnityEngine;

public class ShapePart : MonoBehaviour
{
    // ���������� ������ (X, Y)
    public Vector2Int CellId;

    // ������������� ������� ������
    public void SetPosition(Vector2 position)
    {
        // ������ ������� ������ ��������
        transform.position = position;
    }

}
