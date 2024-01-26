using UnityEngine;

public class Shape : MonoBehaviour
{
    // ������ �������� ���� ShapePart � ��������� �������� 0
    public ShapePart[] Parts = new ShapePart[0];
    // ����������� ����� �������� ������
    public virtual void Rotate() { }

    // ���������� ��� ��������������� �������� ���� �����
    // ������� � ����� �� �������� � ����������
    public int ExtraSpawnYMove;

    public Vector2Int[] GetPartCellIds()
    {
        // ������ ����� ������ ���� Vector2Int � ��������, ������ ����� ������� ������ ������
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];

        // �������� �� ���� ������ ������
        for (int i = 0; i < Parts.Length; i++)
        {
            // ���������� � ������� startCellIds[i] �������� ������ ������ i-���� �������� ������� Parts
            startCellIds[i] = Parts[i].CellId;
        }

        // ���������� ������ startCellIds
        return startCellIds;
    }
}