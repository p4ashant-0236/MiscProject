using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridResizer : MonoBehaviour
{
    // Default grid size (4x4)
    private int rows = 4;
    private int columns = 4;

    // Spacing between grid items (x and y)
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);

    // Padding around the grid (left/right and top/bottom)
    [SerializeField] private Vector2 padding = new Vector2(20f, 20f);

    // References to GridLayoutGroup and RectTransform
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform rectTransform;

    // Constants for adjusting padding and spacing
    private const float PADDING_X_MULTIPLIER = 2f;
    private const float PADDING_Y_MULTIPLIER = 2f;

    void Start()
    {
        UpdateBoard();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateBoard();
    }

    // Initializes the grid with specific row and column counts
    internal void Initialize(int row, int column)
    {
        gridLayout.spacing = spacing;
        gridLayout.constraintCount = Mathf.Min(row, column);
        rows = row;
        columns = column;
    }

    // Updates the grid's cell size based on the available space
    internal void UpdateBoard()
    {
        // Calculate total available width and height, factoring in padding and spacing
        float totalWidth = rectTransform.rect.width - padding.x * PADDING_X_MULTIPLIER - (spacing.x * (columns - 1));
        float totalHeight = rectTransform.rect.height - padding.y * PADDING_Y_MULTIPLIER - (spacing.y * (rows - 1));

        // Calculate individual cell size
        float cellWidth = totalWidth / columns;
        float cellHeight = totalHeight / rows;

        // Use the smaller dimension to ensure cells are square
        float cellSize = Mathf.Floor(Mathf.Min(cellWidth, cellHeight));

        // Set the calculated cell size for the grid layout
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
