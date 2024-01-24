using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            // ��������� ����� ����� ������ ��� ������� ����� ������
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;

            // ���� ����� ������� ������� �� ������� �������� ����
            if (newPartCellId.x < 0 || newPartCellId.y < 0
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                // ���������� false
                return false;
            }
        }
        // ����� ���������� true
        return true;
    }

    // ������ �������� ����
    public GameField GameField;

    // ������� ������
    public Shape TargetShape;

    // �������� �������� ����
    public float MoveDownDelay = 0.8f;

    // ������ �������� ����
    private float _moveDownTimer = 0;

    public void MoveShape(Vector2Int deltaMove)
    {
        // ���� ����������� �� deltaMove ����������
        if (!CheckMovePossible(deltaMove))
        {
            // ������� �� ������
            return;
        }
        // �������� �� ���� ������ ������
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            // ��������� ����� ����� ������ ��� ������� ����� ������
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;

            // ��������� ����� ������� ��� ������� ����� ������
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);

            // ��������� �������� ������ ��� ������� ����� ������
            TargetShape.Parts[i].CellId = newPartCellId;

            // ������������� ����� ������� ��� ������� ����� ������
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // �������� ����� ��������������� ��������
        HorizontalMove();

        // �������� ����� ������������� ��������
        VerticalMove();
    }

    private void HorizontalMove()
    {
        // ���� ���� ������ ������� ����� ��� A
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // ���������� ������ �����
            MoveShape(Vector2Int.left);
        }
        // �����, ���� ���� ������ ������� ������ ��� D
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // ���������� ������ ������
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        // ����������� ������ �� �������� ���������� �������
        _moveDownTimer += Time.deltaTime;

        // ���� ������ ���������� ������� ��� ���� ������ ������� ���� ��� S
        if (_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // �������� ������
            _moveDownTimer = 0;

            // ���������� ������ ����
            MoveShape(Vector2Int.down);
        }
    }
}
