using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

public interface IGrabable
{
	public bool IsGrabable();
	public void SetGrabState(bool p_State);
	public void GrabMoving();
	//bool m_IsMouseGrabable;
	//bool m_IsMouseGrab;
}

public class PlayerCharacter : CharacterBase
{
	//private CameraController m_CameraCon;
	private CapsuleCollider m_Collider;
	public UnityEngine.UI.Image m_GrabItemSprite;
	public AdvencedItem m_GrabItemCode = new AdvencedItem();

	//[HideInInspector] public GameObject m_HitObject;
	[SerializeField] private CollisionComponent m_CollisionComponent;
	[SerializeField] private GameObject m_PlayerCharacterUIPrefab;
	[SerializeField] private PlayerCharacterUIScript m_PlayerCharacterUIScript;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		m_UseScaleFlip = false;

		//m_CameraCon = Camera.main.gameObject.GetComponent<CameraController>();
		m_Collider = gameObject.GetComponent<CapsuleCollider>();
		if (m_CollisionComponent == null) { m_CollisionComponent = UniFunc.GetChildComponent<CollisionComponent>(transform); }

		FindPlayerCharacterUIScript();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		float DeltaTime = Time.deltaTime;

		if((m_GrabItemSprite != null ? m_GrabItemSprite.gameObject.activeSelf : false) == true)
		{
			m_GrabItemSprite.rectTransform.position = Input.mousePosition;
		}
	}

	//protected override void FixedUpdate()
	//{
	//	base.FixedUpdate();
	//	float DeltaTime = Time.fixedDeltaTime;
	//}

	protected override void KeyInput()
	{
		if (Camera.main.orthographic == false)
		{
			m_HorizontalMove = Input.GetAxis("Horizontal");
			m_VerticalMove = Input.GetAxis("Vertical");
		}
		if (Input.GetAxisRaw("Horizontal") == 0.0f) { m_HorizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { m_VerticalMove = 0.0f; }

		if (Input.GetKeyDown(KeyCode.Space) == true) { Jump(); }

		if (Input.GetKeyDown(KeyCode.E) == true) 
		{
			NPC t_NPC = GetInteractableCharacter(); 
			if (t_NPC != null)
			{
				t_NPC.StartConversation();

			}
		}
		if (Input.GetKeyDown(KeyCode.I) == true) 
		{ 
			if (m_Inventory.m_InventoryUIScript != null)
			{ 
				m_Inventory.m_InventoryUIScript.gameObject.SetActive(!m_Inventory.m_InventoryUIScript.gameObject.activeSelf); 
			}
		}

		if (Input.GetMouseButtonDown(0) == true)
		{
			DoRaycast(true);
		}
		if (Input.GetMouseButtonUp(0) == true)
		{
			DoRaycast(false);

			m_GrabItemCode = new AdvencedItem();
			if (m_GrabItemSprite != null) { m_GrabItemSprite.gameObject.SetActive(false); }
		}
	}

	protected override void Jump()
	{
		base.Jump();

		RaycastHit hit;
		Vector3 t_Point = m_Collider != null ? transform.up * ((m_Collider.height / 2) - m_Collider.radius) : transform.position;
		if (Physics.CapsuleCast(t_Point, -t_Point, m_Collider != null ? m_Collider.radius : transform.localScale.x / 2, transform.forward, out hit, Mathf.Infinity) == true)
		{
			if (hit.transform.gameObject != gameObject)
			{
				if(m_Rigidbody != null)
				{
					m_Rigidbody.AddForce(Vector3.up * jumpForce);
				}
			}
		}
	}

	//protected override void HorizontalMove(float DeltaTime)
	//{
	//	base.HorizontalMove(DeltaTime);
	//}

	//protected override void VerticalMove(float DeltaTime)
	//{
	//	base.VerticalMove(DeltaTime);
	//}

	public NPC GetInteractableCharacter()
	{
		NPC t_CharacterBase = null;
		if (m_CollisionComponent != null)
		{
			float t_DotProduct = -1.0f;
			for (int i = 0; i < m_CollisionComponent.m_Collisions.Count; i = i + 1)
			{
				if (m_CollisionComponent.m_Collisions[i] != null)
				{
					if (m_CollisionComponent.m_Collisions[i].gameObject != gameObject)
					{
						NPC t_CharacterBase1 = m_CollisionComponent.m_Collisions[i].gameObject.GetComponent<NPC>();
						if (t_CharacterBase1 != null)
						{
							float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (m_CollisionComponent.m_Collisions[i].transform.position - transform.position).normalized);
							if (t_DotProduct < t_DotProduct1)
							{
								t_DotProduct = t_DotProduct1;
								t_CharacterBase = t_CharacterBase1;
							}
						}
					}
				}
			}
			for (int i = 0; i < m_CollisionComponent.m_Colliders.Count; i = i + 1)
			{
				if (m_CollisionComponent.m_Colliders[i] != null)
				{
					if (m_CollisionComponent.m_Colliders[i].gameObject != gameObject)
					{
						NPC t_CharacterBase1 = m_CollisionComponent.m_Colliders[i].gameObject.GetComponent<NPC>();
						if (t_CharacterBase1 != null)
						{
							float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (m_CollisionComponent.m_Colliders[i].transform.position - transform.position).normalized);
							if (t_DotProduct < t_DotProduct1)
							{
								t_DotProduct = t_DotProduct1;
								t_CharacterBase = t_CharacterBase1;
							}
						}
					}
				}
			}
		}

		return t_CharacterBase;
	}

	public void FindPlayerCharacterUIScript()
	{
		if (m_PlayerCharacterUIScript == null) { m_PlayerCharacterUIScript = FindObjectOfType<PlayerCharacterUIScript>(); }
		if (m_PlayerCharacterUIScript == null)
		{
			Canvas canvas = FindObjectOfType<Canvas>();
			if (canvas != null)
			{
				if (m_PlayerCharacterUIPrefab != null)
				{
					m_PlayerCharacterUIScript = Instantiate(m_PlayerCharacterUIPrefab, canvas.transform).GetComponent<PlayerCharacterUIScript>();
				}
			}
		}

		if (m_PlayerCharacterUIScript != null)
		{
			if (m_GrabItemSprite == null)
			{
				m_GrabItemSprite = m_PlayerCharacterUIScript.m_MouseGrabIcon;
			}
			if(m_Inventory != null)
			{
				if (m_Inventory.m_InventoryUIScript == null)
				{
					m_Inventory.m_InventoryUIScript = m_PlayerCharacterUIScript.m_InventoryUIScript;
				}
				if (m_Inventory.m_MoneyText == null)
				{
					m_Inventory.m_MoneyText = m_PlayerCharacterUIScript.m_MoneyText;
				}
				if (m_Inventory.m_HonerText == null)
				{
					m_Inventory.m_HonerText = m_PlayerCharacterUIScript.m_HonerText;
				}
				m_Inventory.RefreshInventory();

				if (m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_Inventory == null)
				{
					m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_Inventory = m_Inventory;
					m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
				}
			}
		}
	}

	protected override void OnTriggerEnter(Collider collision)
	{
		m_CPAComponent = collision.gameObject.GetComponent<CameraPresetAreaComponent>();
		if (m_CPAComponent != null)
		{
			m_CPAComponent.m_PlayerCharacter = this;
		}
	}

	/// <summary>
	/// MouseButtonDown = true, MouseButtonUp = false
	/// </summary>
	/// <param name="p_MouseDown"> MouseButtonDown = true, MouseButtonUp = false </param>
	protected virtual void DoRaycast(bool p_MouseDown = true)
	{
		bool bMouseOnUI = UnityEngine.EventSystems.EventSystem.current != null ? UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() : false;
		if (p_MouseDown == true)
		{
			Vector3 t_MousePosition = Vector3.zero;
			if (Camera.main.orthographic == false)
			{
				t_MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
				bool result = Physics.Raycast(Camera.main.transform.position, t_MousePosition, out RaycastHit hit, Mathf.Infinity);
				// bMouseOnUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
				if (result == true)// && bMouseOnUI == false)
				{ OnClickHit(hit, bMouseOnUI); }
				else if (result == false)// || bMouseOnUI == true)
				{ OnClickMiss(bMouseOnUI); }
			}
			else if (Camera.main.orthographic == true)
			{
				t_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit2D = Physics2D.Raycast(t_MousePosition, new Vector3(t_MousePosition.x, t_MousePosition.y, t_MousePosition.z + 100));
				if (hit2D == true)// && bMouseOnUI == false)
				{ OnClickHit2D(hit2D, bMouseOnUI); }
				else if (hit2D == false)// || bMouseOnUI == true)
				{ OnClickMiss2D(bMouseOnUI); }
			}
		}
		else if(p_MouseDown == false)
		{
			Vector3 t_MousePosition = Vector3.zero;
			if (Camera.main.orthographic == false)
			{
				t_MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
				bool result = Physics.Raycast(Camera.main.transform.position, t_MousePosition, out RaycastHit hit, Mathf.Infinity);
				if (result == true)// && bMouseOnUI == false)
				{ OnReleaseHit(hit, bMouseOnUI); }
				else if (result == false)// || bMouseOnUI == true)
				{ OnReleaseMiss(bMouseOnUI); }
			}
			else if (Camera.main.orthographic == true)
			{
				t_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit2D = Physics2D.Raycast(t_MousePosition, new Vector3(t_MousePosition.x, t_MousePosition.y, t_MousePosition.z + 100));
				if (hit2D == true)// && bMouseOnUI == false)
				{ OnReleaseHit2D(hit2D, bMouseOnUI); }
				else if (hit2D == false)// || bMouseOnUI == true)
				{ OnReleaseMiss2D(bMouseOnUI); }
			}
		}
	}

	//3DHit

	protected virtual void OnClickHit(RaycastHit hit, bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			MillStoneHandle t_MillStoneHandle = hit.transform.GetComponent<MillStoneHandle>();
			if (t_MillStoneHandle != null)
			{
				MillStone t_MillStone = UniFunc.GetParentComponent<MillStone>(t_MillStoneHandle.gameObject);
				if (t_MillStone != null)
				{
					t_MillStone.bProgress = true;
				}
			}

			AccessoryPlate t_AccessoryPlate = hit.transform.GetComponent<AccessoryPlate>();
			if(t_AccessoryPlate != null)
			{
				if (t_AccessoryPlate.m_Input.IsAddable(new AdvencedItem()) == false)
				{
					if (t_AccessoryPlate.m_Input.itemAmount > 0)
					{
						if (m_GrabItemCode.IsAddable(new AdvencedItem()) == true)
						{
							m_Inventory.AddAItem(t_AccessoryPlate.m_Input.itemCode, t_AccessoryPlate.m_Input.itemProgress, t_AccessoryPlate.m_Input.itemAmount);
							m_GrabItemCode = t_AccessoryPlate.m_Input;
							if (m_GrabItemSprite != null)
							{
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode + "");
								m_GrabItemSprite.gameObject.SetActive(true);
							}
							t_AccessoryPlate.m_Input = new AdvencedItem();
							t_AccessoryPlate.RefreshPlate();
						}
					}
				}
			}

			IGrabable t_GrabableObject = hit.transform.GetComponent<IGrabable>();
			if (t_GrabableObject != null)
			{
				if (t_GrabableObject.IsGrabable() == true)
				{
					t_GrabableObject.SetGrabState(true);
				}
			}

			Debug.Log(hit.transform.gameObject);
			NPCShop t_NPCShop = hit.transform.GetComponent<NPCShop>();
			if (t_NPCShop != null)
			{
				if (m_PlayerCharacterUIScript != null)
				{
					if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
					{
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.gameObject.SetActive(true);
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_NPCInventory = t_NPCShop.m_Inventory;
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
					}
				}
			}
			else if (t_NPCShop == null)
			{
				if (m_PlayerCharacterUIScript != null)
				{
					if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
					{
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.gameObject.SetActive(false);
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_NPCInventory = null;
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
					}
				}
			}
		}
		else if (bMouseOnUI == true)
		{

		}
	}
	protected virtual void OnClickMiss(bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			if (m_PlayerCharacterUIScript != null)
			{
				if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
				{
					m_PlayerCharacterUIScript.m_NPCStoreUIScript.gameObject.SetActive(false);
					m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_NPCInventory = null;
					m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
				}
			}
		}
		else if (bMouseOnUI == true)
		{
		}
	}

	protected virtual void OnReleaseHit(RaycastHit hit, bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
			if (t_MillStone != null)
			{
				if (t_MillStone.SetItem(m_GrabItemCode) == true)
				{
					m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
				}

				/*
				//if (t_MillStone.M_Input == 0)
				//{
				//	AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
				//	if(t_AItem.IsAddable(new AdvencedItem()) == false)
				//	{
				//
				//		t_MillStone.M_Input = t_AItem.itemCode;
				//		t_MillStone.m_Progress = t_AItem.itemProgress;
				//	}
				//}
				*/
			}

			AccessoryPlate t_AccessoryPlate = hit.transform.GetComponent<AccessoryPlate>();
			if (t_AccessoryPlate != null)
			{
				if (t_AccessoryPlate.m_Input.IsAddable(new AdvencedItem()) == true)
				{
					AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					if (t_AItem.IsAddable(new AdvencedItem()) == false)
					{
						t_AccessoryPlate.m_Input = t_AItem;
						t_AccessoryPlate.RefreshPlate();
					}
				}
				else if (t_AccessoryPlate.m_Input.IsAddable(new AdvencedItem()) == false)
				{
					if (m_GrabItemCode.IsAddable(new AdvencedItem()) == false)
					{
						if (t_AccessoryPlate.CraftItem(m_GrabItemCode) == true)
						{
							m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
							t_AccessoryPlate.RefreshPlate();

							if (m_GrabItemSprite != null)
							{
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode + "");
								m_GrabItemSprite.gameObject.SetActive(true);
							}
						}
					}
				}
			}

			PlayerShop t_PlayerShop = hit.transform.GetComponent<PlayerShop>();
			if (t_PlayerShop != null)
			{
				if (t_PlayerShop.itemCode == 0)
				{
					AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					if (t_AItem.IsAddable(new AdvencedItem()) == false)
					{
						t_PlayerShop.itemCode = t_AItem.itemCode;
						t_PlayerShop.HandOverItem();
					}
				}
			}
		}
		else if (bMouseOnUI == true)
		{
		}
	}

	protected virtual void OnReleaseMiss(bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
		}
		else if (bMouseOnUI == true)
		{
		}
	}

	//2DHit

	protected virtual void OnClickHit2D(RaycastHit2D hit, bool bMouseOnUI)
	{
		//m_HitObject = hit.transform.gameObject;

		MillStoneHandle t_MillStoneHandle = hit.transform.GetComponent<MillStoneHandle>();
		if (t_MillStoneHandle != null)
		{
			MillStone t_MillStone = UniFunc.GetParentComponent<MillStone>(t_MillStoneHandle.gameObject);
			if (t_MillStone != null)
			{
				t_MillStone.bProgress = true;
			}
		}

		IGrabable t_GrabableObject = hit.transform.GetComponent<IGrabable>();
		if (t_GrabableObject != null)
		{
			if (t_GrabableObject.IsGrabable() == true)
			{
				t_GrabableObject.SetGrabState(true);
			}
		}
	}

	protected virtual void OnClickMiss2D(bool bMouseOnUI)
	{
		//m_HitObject = null;
	}

	protected virtual void OnReleaseHit2D(RaycastHit2D hit, bool bMouseOnUI)
	{
		MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
		if (t_MillStone != null)
		{
			if (t_MillStone.SetItem(m_GrabItemCode) == true)
			{
				m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
			}
		}

		AccessoryPlate t_AccessoryPlate = hit.transform.GetComponent<AccessoryPlate>();
		if (t_AccessoryPlate != null)
		{
			if (t_AccessoryPlate.m_Input.IsAddable(new AdvencedItem()) == true)
			{
				AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
				if (t_AItem.IsAddable(new AdvencedItem()) == false)
				{
					t_AccessoryPlate.m_Input = t_AItem;
					t_AccessoryPlate.RefreshPlate();
				}
			}
		}
	}

	protected virtual void OnReleaseMiss2D(bool bMouseOnUI)
	{
		//m_HitObject = null;
	}
}
