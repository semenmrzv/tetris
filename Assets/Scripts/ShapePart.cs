using UnityEngine;

public class ShapePart : MonoBehaviour
{
    //  оординаты €чейки (X, Y)
    public Vector2Int CellId;

    // ”станавливаем позицию €чейки
    public void SetPosition(Vector2 position)
    {
        // ƒелаем позицию равной заданной
        transform.position = position;
    }

}
