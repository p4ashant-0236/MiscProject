using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridResizer : MonoBehaviour
{
    private int rows = 4;
    private int columns = 4;
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);
    [SerializeField] private Vector2 padding = new Vector2(20f, 20f); // Left/right and top/bottom padding

    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform rectTransform;

    void Start()
    {
        UpdateBoard();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateBoard();
    }

    internal void Initialize(int row, int column)
    {
        gridLayout.spacing = spacing;
        gridLayout.constraintCount = Mathf.Min(row, column);
        rows = row;
        columns = column;
    }

    internal void UpdateBoard()
    {
        float totalWidth = rectTransform.rect.width - padding.x * 2 - (spacing.x * (columns - 1));
        float totalHeight = rectTransform.rect.height - padding.y * 2 - (spacing.y * (rows - 1));

        float cellWidth = totalWidth / columns;
        float cellHeight = totalHeight / rows;

        float cellSize = Mathf.Floor(Mathf.Min(cellWidth, cellHeight));

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
