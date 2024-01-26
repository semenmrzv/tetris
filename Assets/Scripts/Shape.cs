using UnityEngine;

public class Shape : MonoBehaviour
{
    // Массив объектов типа ShapePart с начальным размером 0
    public ShapePart[] Parts = new ShapePart[0];
    // Виртуальный метод вращения фигуры
    public virtual void Rotate() { }

    // Переменная для дополнительного смещения двух фигур
    // Объявим её позже на префабах в инспекторе
    public int ExtraSpawnYMove;

    public Vector2Int[] GetPartCellIds()
    {
        // Создаём новый массив типа Vector2Int с размером, равным длине массива частей фигуры
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];

        // Проходим по всем частям фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // Записываем в элемент startCellIds[i] значение номера ячейки i-того элемента массива Parts
            startCellIds[i] = Parts[i].CellId;
        }

        // Возвращаем массив startCellIds
        return startCellIds;
    }
}