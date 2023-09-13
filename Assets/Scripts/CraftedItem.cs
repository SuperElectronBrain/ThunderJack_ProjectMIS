using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : MonoBehaviour
{
	[HideInInspector] public bool m_IsMouseGrab = false;
	[HideInInspector] public bool m_IsMouseGrabable = false;
	private Vector3 m_OriginPosition;
	public AdvencedItem m_CompleteItem = new AdvencedItem();

	private AccessoryPlate m_AccessoryPlate;
	[SerializeField] private SpriteRenderer m_SpriteRenderer;

	// Start is called before the first frame update
	void Start()
    {
		m_OriginPosition = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonUp(0) == true)
		{
			if (m_AccessoryPlate != null)
			{
				if (m_AccessoryPlate.CraftItem(m_CompleteItem) == true) 
				{
					m_CompleteItem = new AdvencedItem();
					m_IsMouseGrabable = false;
					RefreshItemDisplay();
				}
			}

			transform.position = m_OriginPosition;
			m_IsMouseGrab = false;
		}
		if (m_IsMouseGrab == true)
		{
			Vector3 t_MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
			transform.position = new Vector3(t_MousePosition.x, t_MousePosition.y, Camera.main.orthographic ? transform.position.z : t_MousePosition.z);
		}
	}

	public void RefreshItemDisplay()
	{
		if (m_CompleteItem.IsAddable(new AdvencedItem()) == false)
		{
			if(m_CompleteItem.itemAmount > 0)
			{
				if (m_SpriteRenderer != null)
				{
					m_SpriteRenderer.gameObject.SetActive(true);
					m_SpriteRenderer.sprite = UniFunc.FindSprite(m_CompleteItem.itemCode + "");
				}
			}
		}
		else
		{
			m_SpriteRenderer.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		AccessoryPlate t_AccessoryPlate = other.GetComponent<AccessoryPlate>();
		if (t_AccessoryPlate != null)
		{
			m_AccessoryPlate = t_AccessoryPlate;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		AccessoryPlate t_AccessoryPlate = collision.GetComponent<AccessoryPlate>();
		if (t_AccessoryPlate != null)
		{
			m_AccessoryPlate = t_AccessoryPlate;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		AccessoryPlate t_AccessoryPlate = other.GetComponent<AccessoryPlate>();
		if (t_AccessoryPlate != null)
		{
			if (m_AccessoryPlate == t_AccessoryPlate)
			{
				m_AccessoryPlate = null;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		AccessoryPlate t_AccessoryPlate = collision.GetComponent<AccessoryPlate>();
		if (t_AccessoryPlate != null)
		{
			if (m_AccessoryPlate == t_AccessoryPlate)
			{
				m_AccessoryPlate = null;
			}
		}
	}
}
