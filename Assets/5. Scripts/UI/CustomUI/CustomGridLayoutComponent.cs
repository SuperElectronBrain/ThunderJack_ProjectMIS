using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomGridLayoutComponent : LayoutGroup, IPointerEnterHandler, IPointerExitHandler
{
	private enum StretchType { Vertical, Horizontal, NonStretch }
	[SerializeField] private StretchType m_StretchType = StretchType.Vertical;
	[SerializeField] private RectTransform m_BaseRect;

	private enum Constraint { Auto, Columns, Rows, Manual };
	[SerializeField] private Constraint constraint;
	[SerializeField] private int columns = 1;
	[SerializeField] private int rows = 1;
	[SerializeField] private Vector2 childSize = new Vector2(1.0f, 1.0f);

	[SerializeField] private Scrollbar m_Scrollbar;
	[SerializeField] private bool autoActivationScrollbar = false;
	[SerializeField] private bool fixedSizeScrollbarHandle = false;

	protected bool isMouseOver = false;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		RectTransform TargetRect = m_BaseRect != null ? m_BaseRect : rectTransform;
		columns = columns < 1 ? 1 : columns;
		rows = rows < 1 ? 1 : rows;

		if (constraint == Constraint.Auto)
		{
			float sqrt = Mathf.Sqrt(rectChildren.Count);
			columns = Mathf.CeilToInt(sqrt);
			rows = Mathf.CeilToInt(sqrt);
		}
		if (constraint == Constraint.Columns) { rows = Mathf.CeilToInt(rectChildren.Count / (float)columns); }
		if (constraint == Constraint.Rows) { columns = Mathf.CeilToInt(rectChildren.Count / (float)rows); }

		float child_xSize = childSize.x * (TargetRect.rect.width / columns);
		float child_ySize = childSize.y * (TargetRect.rect.height / rows);

		for (int i = 0; i < rectChildren.Count; i = i + 1)
		{
			int currentColumn = i % columns;
			int currentRow = i / columns;

			float child_xPos = ((1 - childSize.x) / 2) * (TargetRect.rect.width / columns);
			float child_yPos = ((1 - childSize.y) / 2) * (TargetRect.rect.height / rows);

			child_xPos = child_xPos + (child_xPos * 2 * currentColumn);
			child_yPos = child_yPos + (child_yPos * 2 * currentRow);

			if (m_BaseRect != null)
			{
				if (m_StretchType == StretchType.Horizontal)
				{
					child_xPos = child_xPos + (-(1 - childSize.x) * (TargetRect.rect.width / (columns * 2)));
				}
				else if (m_StretchType == StretchType.Vertical)
				{
					child_yPos = child_yPos + (-(1 - childSize.y) * (TargetRect.rect.height / (rows * 2)));
				}
			}

			float childAlignment_xPos = 0;
			float childAlignment_yPos = 0;

			if (childAlignment != TextAnchor.MiddleCenter)
			{
				if (childAlignment == TextAnchor.UpperLeft)
				{
					childAlignment_xPos = -child_xPos;
					childAlignment_yPos = -child_yPos;
				}
				else if (childAlignment == TextAnchor.UpperCenter)
				{ childAlignment_yPos = -child_yPos; }
				else if (childAlignment == TextAnchor.UpperRight)
				{
					childAlignment_xPos = -child_xPos + (rectTransform.rect.width - (child_xSize * columns));
					childAlignment_yPos = -child_yPos;
				}
				else if (childAlignment == TextAnchor.MiddleLeft)
				{ childAlignment_xPos = -child_xPos; }
				else if (childAlignment == TextAnchor.MiddleRight)
				{ childAlignment_xPos = -child_xPos + (rectTransform.rect.width - (child_xSize * columns)); }
				else if (childAlignment == TextAnchor.LowerLeft)
				{
					childAlignment_xPos = -child_xPos;
					childAlignment_yPos = -child_yPos + (rectTransform.rect.height - (child_ySize * rows));
				}
				else if (childAlignment == TextAnchor.LowerCenter)
				{ childAlignment_yPos = -child_yPos + (rectTransform.rect.height - (child_ySize * rows)); }
				else if (childAlignment == TextAnchor.LowerRight)
				{
					childAlignment_xPos = -child_xPos + (rectTransform.rect.width - (child_xSize * columns));
					childAlignment_yPos = -child_yPos + (rectTransform.rect.height - (child_ySize * rows));
				}
			}

			SetChildAlongAxis(rectChildren[i], 0, (currentColumn * child_xSize) + child_xPos + childAlignment_xPos, child_xSize);
			SetChildAlongAxis(rectChildren[i], 1, (currentRow * child_ySize) + child_yPos + childAlignment_yPos, child_ySize);
		}

		if (m_BaseRect != null)
		{
			float xSize = rectTransform.sizeDelta.x;
			float ySize = rectTransform.sizeDelta.y;

			if (rectChildren.Count > 0)
			{
				if (m_StretchType == StretchType.Horizontal)
				{
					xSize = (rectChildren[rectChildren.Count - 1].anchoredPosition.x - rectChildren[0].anchoredPosition.x) + child_xSize;
				}
				else if (m_StretchType == StretchType.Vertical)
				{
					ySize = (rectChildren[0].anchoredPosition.y - rectChildren[rectChildren.Count - 1].anchoredPosition.y) + child_ySize;
				}
			}
			rectTransform.sizeDelta = new Vector2(xSize, ySize);

			if (m_Scrollbar != null)
			{
				if (fixedSizeScrollbarHandle == false)
				{
					if (m_Scrollbar.direction == Scrollbar.Direction.TopToBottom || m_Scrollbar.direction == Scrollbar.Direction.BottomToTop)
					{
						m_Scrollbar.size = m_BaseRect.rect.height / (rectTransform.rect.height < m_BaseRect.rect.height ? m_BaseRect.rect.height : rectTransform.rect.height);
					}
					else if (m_Scrollbar.direction == Scrollbar.Direction.LeftToRight || m_Scrollbar.direction == Scrollbar.Direction.RightToLeft)
					{
						m_Scrollbar.size = m_BaseRect.rect.width / (rectTransform.rect.width < m_BaseRect.rect.width ? m_BaseRect.rect.width : rectTransform.rect.width);
					}
				}
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (m_BaseRect != null)
		{
			if (m_Scrollbar != null)
			{
				if (autoActivationScrollbar == true)
				{
					if (m_Scrollbar.direction == Scrollbar.Direction.TopToBottom || m_Scrollbar.direction == Scrollbar.Direction.BottomToTop)
					{
						m_Scrollbar.gameObject.SetActive(m_BaseRect.rect.height / rectTransform.rect.height < 1);
					}
					else if (m_Scrollbar.direction == Scrollbar.Direction.LeftToRight || m_Scrollbar.direction == Scrollbar.Direction.RightToLeft)
					{
						m_Scrollbar.gameObject.SetActive(m_BaseRect.rect.width / rectTransform.rect.width < 1);
					}
				}

				if (m_Scrollbar.direction == Scrollbar.Direction.TopToBottom)
				{
					if (rectTransform.rect.height > m_BaseRect.rect.height)
					{
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, m_Scrollbar.value * (rectTransform.rect.height - m_BaseRect.rect.height));
					}
				}
				else if (m_Scrollbar.direction == Scrollbar.Direction.LeftToRight)
				{
					if (rectTransform.rect.width > m_BaseRect.rect.width)
					{
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.y, m_Scrollbar.value * (rectTransform.rect.width - m_BaseRect.rect.width));
					}
				}

				if (isMouseOver == true)
				{
					m_Scrollbar.value = m_Scrollbar.value - Input.GetAxis("Mouse ScrollWheel");
					m_Scrollbar.value = m_Scrollbar.value < 0 ? 0 : (m_Scrollbar.value > 1 ? 1 : m_Scrollbar.value);
				}
			}
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		isMouseOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isMouseOver = false;
	}
}