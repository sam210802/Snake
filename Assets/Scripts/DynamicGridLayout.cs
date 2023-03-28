using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : LayoutGroup
{
    [SerializeField]
    private int columns;
    [SerializeField]
    private int rows;

    Vector2 cellSize;
    
    [SerializeField]
    Vector2 spacing;

    // default method called
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float cellWidth = 0;
        float cellHeight = 0;

        foreach (RectTransform child in transform) {
            cellWidth = Mathf.Max(cellWidth, child.sizeDelta.x);
            cellHeight = Mathf.Max(cellHeight, child.sizeDelta.y);
        }

        rectTransform.sizeDelta = new Vector2((cellWidth + spacing.x * 2) * columns, (cellHeight + spacing.y * 2) * rows);

        float sqrRt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrRt);
        columns = Mathf.CeilToInt(sqrRt);

        // float parentWidth = rectTransform.rect.width;
        // float parentHeight = rectTransform.rect.height;

        // float cellWidth = (parentWidth / (float) columns) - ((spacing.x / (float) columns) * (columns - 1));
        // float cellHeight = (parentHeight / (float) rows) - ((spacing.y / (float) rows) * (rows - 1));

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++) {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount);
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount);

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {
        
    }
}
