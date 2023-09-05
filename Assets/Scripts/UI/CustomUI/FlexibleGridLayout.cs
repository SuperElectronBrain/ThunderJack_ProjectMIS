using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
	private enum FitType { Autofit, FitToColumns, FitToRows, Manual };

	[SerializeField] private FitType fitType;
	[SerializeField] private bool fitX;
	[SerializeField] private bool fitY;
	[SerializeField] private int columns;
	[SerializeField] private int rows;
	[SerializeField] private Vector2 cellSize;
	[SerializeField] private Vector2 ratio = new Vector2(1.0f, 1.0f);

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		columns = columns < 1 ? 1 : columns;
		rows = rows < 1 ? 1 : rows;
		ratio.x = ratio.x < 0 ? 0 : ratio.x;
		ratio.y = ratio.y < 0 ? 0 : ratio.y;

		if (fitType == FitType.Autofit)
		{
			float sqrRt = Mathf.Sqrt(transform.childCount);
			columns = Mathf.CeilToInt(sqrRt);
			rows = Mathf.CeilToInt(sqrRt);
		}
		if (fitType == FitType.FitToColumns)
		{
			rows = Mathf.CeilToInt(transform.childCount / (float)columns);
		}
		if(fitType == FitType.FitToRows)
		{
			columns = Mathf.CeilToInt(transform.childCount / (float)rows);
		}

		cellSize.x = rectTransform.rect.width / columns;
		cellSize.y = rectTransform.rect.height / rows;
		
		float xPos = cellSize.x;
		float yPos = cellSize.y;

		if (fitX == true) { cellSize.y = cellSize.x < cellSize.y ? cellSize.x : cellSize.y; }
		if (fitY == true) { cellSize.x = cellSize.x < cellSize.y ? cellSize.x : cellSize.y; }

		float xSize = cellSize.x * ratio.x;
		float ySize = cellSize.y * ratio.y;

		for (int i = 0; i < rectChildren.Count; i = i + 1)
		{
			if (childAlignment == TextAnchor.UpperLeft || childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.LowerLeft)
			{
				SetChildAlongAxis(rectChildren[i], 0, ((i % columns) * cellSize.x) + ((cellSize.x * (1 - ratio.x)) / 2), xSize);
			}
			else if(childAlignment == TextAnchor.UpperCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.LowerCenter)
			{
				SetChildAlongAxis(rectChildren[i], 0, ((i % columns) * xPos) + ((xPos - cellSize.x) / 2) + ((cellSize.x * (1 - ratio.x)) / 2), xSize);
			}
			else if (childAlignment == TextAnchor.UpperRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.LowerRight)
			{
				SetChildAlongAxis(rectChildren[i], 0, ((i % columns) * cellSize.x) + (rectTransform.rect.width - (cellSize.x * columns)) + ((cellSize.x * (1 - ratio.x)) / 2), xSize);
			}

			if(childAlignment == TextAnchor.UpperLeft || childAlignment == TextAnchor.UpperCenter || childAlignment == TextAnchor.UpperRight)
			{
				SetChildAlongAxis(rectChildren[i], 1, ((i / columns) * cellSize.y) + ((cellSize.y * (1 - ratio.y)) / 2), ySize);
			}
			else if (childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.MiddleRight)
			{
				SetChildAlongAxis(rectChildren[i], 1, ((i / columns) * yPos) + ((yPos - cellSize.y) / 2) + ((cellSize.y * (1 - ratio.y)) / 2), ySize);
			}
			else if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.LowerRight)
			{
				SetChildAlongAxis(rectChildren[i], 1, ((i / columns) * cellSize.y) + (rectTransform.rect.height - (cellSize.y * rows)) + ((cellSize.y * (1 - ratio.y)) / 2), ySize);
			}
		}

		cellSize.x = xSize / rectTransform.rect.width;
		cellSize.y = ySize / rectTransform.rect.height;
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
