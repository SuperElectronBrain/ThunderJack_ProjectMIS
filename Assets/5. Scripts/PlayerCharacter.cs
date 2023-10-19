using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IGrabable
{
	public bool IsGrabable();
	public void SetGrabState(bool p_State);
	public void GrabMoving();
	//bool m_IsMouseGrabable;
	//bool m_IsMouseGrab;
}

public struct InteractableObject
{
	public IInteraction interaction;
	public GameObject interactionGO;

	public InteractableObject(IInteraction p_Interaction, GameObject p_GameObject) { interaction = p_Interaction; interactionGO = p_GameObject; }
}

public class PlayerCharacter : CharacterBase
{
	private float m_FadeInTimeBase = 0.0f;
	private float m_FadeInTime = 0.0f;
	private float m_FadeOutTimeBase = 0.0f;
	private float m_FadeOutTime = 0.0f;
	private float m_MonologueDisplayTime = 0.0f;
	private float m_GuideDisplayTime = 0.0f;
	[SerializeField] private float m_MoveAnimationSpeed = 1.0f;

	//private CameraController m_CameraCon;
	private CapsuleCollider m_Collider;
	public UnityEngine.UI.Image m_GrabItemSprite;
	[HideInInspector] public AdvencedItem m_GrabItemCode = new AdvencedItem();
	[HideInInspector] public AdvencedItem m_HoverItemCode = new AdvencedItem();
	public ItemInfoDisplay m_ItemInfoDisplay;
	//[HideInInspector] public GameObject m_HitObject;
	[SerializeField] private CollisionComponent m_CollisionComponent;
	[SerializeField] private GameObject m_PlayerCharacterUIPrefab;
	public PlayerCharacterUIScript m_PlayerCharacterUIScript;
	[SerializeField] private ParticleSystem m_FootStepEffectInside;
	[SerializeField] private ParticleSystem m_FootStepEffectOutdoor;
	[HideInInspector] public RecipeBook m_RecipeBook;
	[HideInInspector] public QuestComponet m_QuestComponet;
	[HideInInspector] public TutorialComponent m_TutorialComponent;
	[SerializeField] private Animator m_ShadowAnimator;
	//private IInteraction m_Interaction;
	private List<InteractableObject> InteractableObjects = new List<InteractableObject>();
	private GameObject m_GuideUI;
	private InteractionItem m_InteractionItem;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		m_UseScaleFlip = false;

		//m_CameraCon = Camera.main.gameObject.GetComponent<CameraController>();
		m_Collider = gameObject.GetComponent<CapsuleCollider>();
		if (m_CollisionComponent == null) { m_CollisionComponent = UniFunc.GetChildComponent<CollisionComponent>(transform); }
		if (m_CollisionComponent != null)
		{
			m_CollisionComponent.m_OnCollisionEnterUseParam.AddListener(OnInteractableObjectEnter);
			m_CollisionComponent.m_OnCollisionExitUseParam.AddListener(OnInteractableObjectExit);
		}
		if (m_RecipeBook == null) { m_RecipeBook = GetComponent<RecipeBook>(); }
		if (m_QuestComponet == null) { m_QuestComponet = GetComponent<QuestComponet>(); }
		if (m_TutorialComponent == null) { m_TutorialComponent = GetComponent<TutorialComponent>(); }

		if (m_FootStepEffectInside != null)
		{
			m_FootStepEffectInside.Stop();
		}
		if (m_FootStepEffectOutdoor != null)
		{
			m_FootStepEffectOutdoor.Stop();
		}

		if (m_InteractionItem == null)
		{
			m_InteractionItem = GetComponent<InteractionItem>();
		}

		EventManager.Subscribe(EventType.EndIteraction, CommunicationEnd);
		EventManager.Subscribe(EventType.EndIteraction, CloseNPCShop);
		EventManager.Subscribe(DialogEventType.ShopGemOpen, OpenBeilShop);
		EventManager.Subscribe(DialogEventType.ShopJewelryOpen, OpenGagaShop);
		FindPlayerCharacterUIScript();
	} 

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		float DeltaTime = Time.deltaTime;

		if ((m_GrabItemSprite != null ? m_GrabItemSprite.gameObject.activeSelf : false) == true)
		{
			m_GrabItemSprite.rectTransform.position = Input.mousePosition;
		}
		if (m_ItemInfoDisplay != null)
		{
			if ((m_ItemInfoDisplay.m_ItemInfoDisplayGO != null ? m_ItemInfoDisplay.m_ItemInfoDisplayGO.activeSelf : false) == true)
			{
				if(m_ItemInfoDisplay.m_ItemInfoDisplayRect != null)
				{
					m_ItemInfoDisplay.m_ItemInfoDisplayRect.position = Input.mousePosition;
				}
			}

			if(m_ItemInfoDisplay.m_ItemInfoDisplayGO != null)
			{
				if(m_PlayerCharacterUIScript != null)
				{
					if (m_PlayerCharacterUIScript.m_InventoryUIScript != null)
					{
						if(m_PlayerCharacterUIScript.m_InventoryUIScript.gameObject.activeSelf == false)
						{
							m_ItemInfoDisplay.m_ItemInfoDisplayGO.SetActive(false);
						}
					}
				}
			}
		}
		if(m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_InteractionIcon != null)
			{
				InteractableObject t_InteractableObject = GetInteractableObject();
				if(t_InteractableObject.interaction != null)
				{
					Vector3 t_Vector = Camera.main.WorldToScreenPoint(t_InteractableObject.interactionGO.transform.position);
					t_Vector.x = t_Vector.x + 0;
					t_Vector.y = t_Vector.y + 75;
					m_PlayerCharacterUIScript.m_InteractionIcon.rectTransform.position = t_Vector;
				}
			}
		}


		if (m_MonologueDisplayTime > 0.0f)
		{
			m_MonologueDisplayTime = m_MonologueDisplayTime - DeltaTime;
			if(m_MonologueDisplayTime < 0.0f)
			{
				PopUpMonologue("", 0.0f);

				if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.PopUpMonologue) { m_TutorialComponent.ProgressTutorial(); } }
				m_MonologueDisplayTime = 0.0f;
			}
		}
		if (m_GuideDisplayTime > 0.0f)
		{
			m_GuideDisplayTime = m_GuideDisplayTime - DeltaTime;
			if (m_GuideDisplayTime < 0.0f)
			{
				PopUpGuide("", 0.0f);

				if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.PopUpGuide) { m_TutorialComponent.ProgressTutorial(); } }
				m_GuideDisplayTime = 0.0f;
			}
		}
		if (m_FadeInTime > 0.0f)
		{
			m_FadeInTime = m_FadeInTime - DeltaTime;

			if (m_PlayerCharacterUIScript != null)
			{
				if (m_PlayerCharacterUIScript.m_FadeUI != null)
				{
					Color t_Color = m_PlayerCharacterUIScript.m_FadeUI.color;
					t_Color.a = m_FadeInTime / m_FadeInTimeBase;
					m_PlayerCharacterUIScript.m_FadeUI.color = t_Color;
				}
			}

			if (m_FadeInTime < 0.0f)
			{
				if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.FadeIn) { m_TutorialComponent.ProgressTutorial(); } }
				m_FadeInTime = 0.0f;
				m_FadeInTimeBase = 0.0f;
			}
		}
		if (m_FadeOutTime > 0.0f)
		{
			m_FadeOutTime = m_FadeOutTime - DeltaTime;

			if (m_PlayerCharacterUIScript != null)
			{
				if (m_PlayerCharacterUIScript.m_FadeUI != null)
				{
					Color t_Color = m_PlayerCharacterUIScript.m_FadeUI.color;
					t_Color.a = 1 - (m_FadeOutTime / m_FadeOutTimeBase);
					m_PlayerCharacterUIScript.m_FadeUI.color = t_Color;
				}
			}

			if (m_FadeOutTime < 0.0f)
			{
				if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.FadeOut) { m_TutorialComponent.ProgressTutorial(); } }
				m_FadeOutTime = 0.0f;
				m_FadeOutTimeBase = 0.0f;
			}
		}

		if(m_FootStepEffectInside != null || m_FootStepEffectOutdoor != null)
		{
			if (m_FootStepEffectInside != null)
			{
				if (m_HorizontalMove != 0)
				{
					if (m_FootStepEffectInside.isPlaying != true)
					{
						if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Build_Inside")
						{
							m_FootStepEffectInside.Play();
						}
					}
				}
				else if (m_HorizontalMove == 0)
				{
					m_FootStepEffectInside.Stop();
				}
			}
			if (m_FootStepEffectOutdoor != null)
			{
				if (m_HorizontalMove != 0)
				{
					if (m_FootStepEffectOutdoor.isPlaying != true)
					{
						if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Build_Inside")
						{
							m_FootStepEffectOutdoor.Play();
						}
					}
				}
				else if (m_HorizontalMove == 0)
				{
					m_FootStepEffectOutdoor.Stop();
				}
			}

			Vector3 t_ModelingScale = (m_FootStepEffectInside != null ? m_FootStepEffectInside : m_FootStepEffectOutdoor).transform.parent.localScale;
			if (m_HorizontalMove > 0) { t_ModelingScale.x = -1.0f; }
			else if (m_HorizontalMove < 0) { t_ModelingScale.x = 1.0f; }
			(m_FootStepEffectInside != null ? m_FootStepEffectInside : m_FootStepEffectOutdoor).transform.parent.localScale = t_ModelingScale;
		}

		if(m_Animator != null)
		{
			m_Animator.SetFloat("MoveSpeed", (m_Velocity / m_Speed) * m_MoveAnimationSpeed);
		}
		if (m_ShadowAnimator != null)
		{
			m_ShadowAnimator.SetFloat("MoveSpeed", (m_Velocity / m_Speed) * m_MoveAnimationSpeed);
			m_ShadowAnimator.SetFloat("HorizontalSpeed", m_UseScaleFlip ? (m_HorizontalMove < 0 ? -m_HorizontalMove : m_HorizontalMove) : m_HorizontalMove);
			m_ShadowAnimator.SetFloat("VerticalSpeed", m_VerticalMove);
		}

		/*
		if(m_Interaction != null)
		{
			if (m_Interaction.IsUsed == true)
			{
				bMovable = false;
			}
			else if (m_Interaction.IsUsed == false)
			{
				bMovable = true;
				m_Interaction = null;
			}
		}
		*/
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		float DeltaTime = Time.fixedDeltaTime;
		SetMoveDirection(Camera.main.transform.right, new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized);
	}

	protected override void KeyInput()
	{
		if (Camera.main.orthographic == false)
		{
			m_HorizontalMove = Input.GetAxis("Horizontal");
			m_VerticalMove = Input.GetAxis("Vertical");
		}
		if (Input.GetAxisRaw("Horizontal") == 0.0f) { m_HorizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { m_VerticalMove = 0.0f; }

		if(bMovable == true)
		{
			if (Input.GetKeyDown(KeyCode.Space) == true) { Jump(); }
			if (Input.GetKeyDown(KeyCode.E) == true) 
			{
				IInteraction T_Interaction = GetInteractableObject().interaction;
				if (T_Interaction != null)
				{
					T_Interaction.Interaction(gameObject);
					CommunicationStart();
				}
				PopUpInteractionIcon(false);
			}
			if (Input.GetKeyDown(KeyCode.Q) == true)
			{ 
				if (m_Inventory.m_InventoryUIScript != null)
				{ 
					m_Inventory.m_InventoryUIScript.gameObject.SetActive(!m_Inventory.m_InventoryUIScript.gameObject.activeSelf);
					m_Inventory.RefreshInventory();
				}
			}
			if (Input.GetKeyDown(KeyCode.F1) == true)
			{
				GameObject t_GameObject = UniFunc.GetChildOfName(GameObject.Find("GuideUIs"), "ManualUI");
				if(t_GameObject != null)
				{
					t_GameObject.SetActive(!t_GameObject.activeSelf);
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
	}

	protected override void Jump()
	{
		base.Jump();

		//RaycastHit hit;
		//Vector3 t_Point = m_Collider != null ? transform.up * ((m_Collider.height / 2) - m_Collider.radius) : transform.position;
		//if (Physics.CapsuleCast(t_Point, -t_Point, m_Collider != null ? m_Collider.radius : transform.localScale.x / 2, transform.forward, out hit, Mathf.Infinity) == true)
		//{
		//	if (hit.transform.gameObject != gameObject)
		//	{
		//		if(m_Rigidbody != null)
		//		{
		//			m_Rigidbody.AddForce(Vector3.up * jumpForce);
		//		}
		//	}
		//}
	}

	//protected override void HorizontalMove(float DeltaTime)
	//{
	//	base.HorizontalMove(DeltaTime);
	//}

	//protected override void VerticalMove(float DeltaTime)
	//{
	//	base.VerticalMove(DeltaTime);
	//}

	public void TakeComponents(PlayerCharacter p_PlayerCharacter)
	{
		if (p_PlayerCharacter != null)
		{
			if (m_Inventory == null)
			{
				m_Inventory = gameObject.GetComponent<Inventory>();
				if(m_Inventory == null) { m_Inventory = gameObject.AddComponent<Inventory>(); }
			}
			if (m_Inventory != null)
			{
				//if (m_Inventory.GetAItems().Count <= 0) { m_Inventory.TakeInventoryItems(p_PlayerCharacter.m_Inventory); }
				m_Inventory.TakeInventoryItems(p_PlayerCharacter.m_Inventory);
			}

			if(m_RecipeBook == null)
			{
				m_RecipeBook = gameObject.GetComponent<RecipeBook>();
				if (m_RecipeBook == null) { m_RecipeBook = gameObject.AddComponent<RecipeBook>(); }
			}
			if (m_RecipeBook != null)
			{
				//if (m_RecipeBook.GetItemRecipes().Count <= 0) { m_RecipeBook.TakeItemRecipes(p_PlayerCharacter.m_RecipeBook); }
				m_RecipeBook.TakeItemRecipes(p_PlayerCharacter.m_RecipeBook);
			}

			if(m_QuestComponet == null)
			{
				m_QuestComponet = gameObject.GetComponent<QuestComponet>();
				if (m_QuestComponet == null) { m_QuestComponet = gameObject.AddComponent<QuestComponet>(); }
			}
			if (m_QuestComponet != null)
			{
				//if (m_QuestComponet.GetQuests().Count <= 0) { m_QuestComponet.TakeQuests(p_PlayerCharacter.m_QuestComponet); }
				m_QuestComponet.TakeQuests(p_PlayerCharacter.m_QuestComponet);
			}

			if (m_TutorialComponent == null)
			{
				m_TutorialComponent = gameObject.GetComponent<TutorialComponent>();
				if (m_TutorialComponent == null) { m_TutorialComponent = gameObject.AddComponent<TutorialComponent>(); }
			}
			if (m_TutorialComponent != null)
			{
				//if (m_TutorialComponent.GetStates().Count <= 0) { m_TutorialComponent.TakeStates(p_PlayerCharacter.m_TutorialComponent); }
				m_TutorialComponent.TakeStates(p_PlayerCharacter.m_TutorialComponent);
			}
		}
	}

	public void OpenNPCShop()
	{
		InteractableObject t_InteractableObject = GetInteractableObject();
		if (t_InteractableObject.interactionGO != null)
		{
			NPCShop t_NPCShop = t_InteractableObject.interactionGO.GetComponent<NPCShop>();
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
				CloseNPCShop();
			}
		}
	}

	public void OpenBeilShop()
	{
		if (m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
			{
				m_PlayerCharacterUIScript.m_NPCStoreUIScript.SetShopType(true);
			}
		}
		OpenNPCShop();
	}

	public void OpenGagaShop()
	{
		if (m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
			{
				m_PlayerCharacterUIScript.m_NPCStoreUIScript.SetShopType(false);
			}
		}
		OpenNPCShop();
	}

	public void CloseNPCShop()
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

	public InteractionItem GetInteractionItem()
	{
		return m_InteractionItem;
	}

	public InteractableObject GetInteractableObject()
	{
		InteractableObject t_Interaction = new InteractableObject(null, null);
		if (InteractableObjects != null)
		{
			if (InteractableObjects.Count > 0)
			{
				float t_DotProduct = -1.0f;
				for (int i = 0; i < InteractableObjects.Count; i = i + 1)
				{
					if(InteractableObjects[i].interactionGO != null)
					{
						float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (InteractableObjects[i].interactionGO.transform.position - transform.position).normalized);
						if (t_DotProduct < t_DotProduct1)
						{
							t_DotProduct = t_DotProduct1;
							t_Interaction = new InteractableObject(InteractableObjects[i].interaction, InteractableObjects[i].interactionGO);
						}
					}
				}
			}
		}

		/*
		if (m_CollisionComponent != null)
		{
			float t_DotProduct = -1.0f;
			for (int i = 0; i < m_CollisionComponent.m_Collisions.Count; i = i + 1)
			{
				if (m_CollisionComponent.m_Collisions[i] != null)
				{
					if (m_CollisionComponent.m_Collisions[i].gameObject != gameObject)
					{
						IInteraction t_Interaction1 = m_CollisionComponent.m_Collisions[i].gameObject.GetComponent<IInteraction>();
						if (t_Interaction1 != null)
						{
							float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (m_CollisionComponent.m_Collisions[i].transform.position - transform.position).normalized);
							if (t_DotProduct < t_DotProduct1)
							{
								t_DotProduct = t_DotProduct1;
								t_Interaction = t_Interaction1;
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
						IInteraction t_Interaction1 = m_CollisionComponent.m_Colliders[i].gameObject.GetComponent<IInteraction>();
						if (t_Interaction1 != null)
						{
							float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (m_CollisionComponent.m_Colliders[i].transform.position - transform.position).normalized);
							if (t_DotProduct < t_DotProduct1)
							{
								t_DotProduct = t_DotProduct1;
								t_Interaction = t_Interaction1;
							}
						}
					}
				}
			}
		}
		*/
		return t_Interaction;
	}

	private void OnInteractableObjectEnter(Collider other)
	{
		if(other.gameObject != gameObject)
		{
			IInteraction t_Interaction = other.gameObject.GetComponent<IInteraction>();
			if (t_Interaction != null)
			{
				if (InteractableObjects == null) { InteractableObjects = new List<InteractableObject>(); }
				if(t_Interaction != InteractableObjects.Find((InteractableObject x) => { return x.interaction == t_Interaction; }).interaction)
				{
					InteractableObjects.Add(new InteractableObject(t_Interaction, other.gameObject));
				}
			}
		}
		
		PopUpInteractionIcon(InteractableObjects.Count > 0);
	}

	private void OnInteractableObjectExit(Collider other)
	{
		if (other.gameObject != gameObject)
		{
			IInteraction t_Interaction = other.gameObject.GetComponent<IInteraction>();
			if (t_Interaction != null)
			{
				InteractableObjects.Remove(new InteractableObject(t_Interaction, other.gameObject));
				InteractableObjects.TrimExcess();
			}
		}

		PopUpInteractionIcon(InteractableObjects.Count > 0);
	}

	public override void CommunicationEnd()
	{
		base.CommunicationEnd();
		PopUpInteractionIcon(false);
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
			m_PlayerCharacterUIScript.ReFindUI();
			if (m_GrabItemSprite == null)
			{
				m_GrabItemSprite = m_PlayerCharacterUIScript.m_MouseGrabIcon;
			}
			if (m_ItemInfoDisplay == null || m_ItemInfoDisplay != null)
			{
				m_ItemInfoDisplay = m_PlayerCharacterUIScript.m_ItemInfoDisplay;
			}
			if (m_Inventory != null)
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

				if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
				{
					if (m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_Inventory == null)
					{
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_Inventory = m_Inventory;
						m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
					}
				}
			}
			if (m_RecipeBook != null)
			{
				if (m_RecipeBook.m_RecipeBookUIScript == null)
				{
					if(m_PlayerCharacterUIScript.m_RecipeBookUIScript != null)
					{
						m_RecipeBook.m_RecipeBookUIScript = m_PlayerCharacterUIScript.m_RecipeBookUIScript;
						m_RecipeBook.m_RecipeBookUIScript.m_RecipeBook = m_RecipeBook;
						if(m_Inventory != null) { m_RecipeBook.m_RecipeBookUIScript.m_Inventory = m_Inventory; }
					}
				}
			}
			if (m_QuestComponet != null)
			{
				if (m_QuestComponet.m_QuestListUIScript == null)
				{
					if(m_PlayerCharacterUIScript.m_QuestListUIScript != null)
					{
						m_QuestComponet.m_QuestListUIScript = m_PlayerCharacterUIScript.m_QuestListUIScript;
						if(m_QuestComponet.m_QuestListUIScript.m_QuestComponet == null)
						{
							m_QuestComponet.m_QuestListUIScript.m_QuestComponet = m_QuestComponet;
						}
						m_QuestComponet.m_QuestListUIScript.RefreshUI();
					}
				}
				if (m_QuestComponet.m_MailBoxUIScript == null)
				{
					if (m_PlayerCharacterUIScript.m_MailBoxUIScript != null)
					{
						m_QuestComponet.m_MailBoxUIScript = m_PlayerCharacterUIScript.m_MailBoxUIScript;
					}
				}
			}
		}
	}

	public void PopUpInteractionIcon(bool param)
	{
		if (m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_InteractionIcon != null)
			{
				if(m_PlayerCharacterUIScript.m_InteractionIcon.gameObject.activeSelf != param)
				{
					m_PlayerCharacterUIScript.m_InteractionIcon.gameObject.SetActive(param);
				}
			}
		}
	}

	public void PopUpMonologue(string p_Script, float p_Time)
	{
		if(m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_MonologueUI != null)
			{
				if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO != null)
				{
					if (p_Time > 0.0f)
					{
						if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.activeSelf == false)
						{
							m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.SetActive(true);
						}
						if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText != null)
						{
							m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText.text = p_Script;
						}
					}
					else if (p_Time <= 0.0f)
					{
						if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.activeSelf == true)
						{
							m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.SetActive(false);
						}

						if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText != null)
						{
							m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText.text = "";
						}
					}
				}
			}
		}

		m_MonologueDisplayTime = p_Time;
	}

	public void PopUpGuide(string p_GuideUIName, float p_Time)
	{
		if (m_GuideUI != null) { m_GuideUI.SetActive(false); }
		m_GuideUI = UniFunc.GetChildOfName(GameObject.Find("GuideUIs"), p_GuideUIName);
		if(m_GuideUI != null)
		{
			if(p_Time > 0.0f)
			{
				if (m_GuideUI.activeSelf == false) { m_GuideUI.SetActive(true); }
			}
			else if(p_Time <= 0.0f)
			{
				if (m_GuideUI.activeSelf == true) { m_GuideUI.SetActive(false); }
			}
		}

		m_GuideDisplayTime = p_Time;
	}

	public void FadeIn(float p_Time) { m_FadeInTimeBase = p_Time; m_FadeInTime = p_Time; }
	public void FadeOut(float p_Time) { m_FadeOutTimeBase = p_Time; m_FadeOutTime = p_Time; }

	public void SetPlayerGrabItem(AdvencedItem p_AItem)
	{
		m_GrabItemCode = p_AItem;
		if(m_InteractionItem == null)
		{
			if (m_GrabItemSprite != null)
			{
				m_GrabItemSprite.sprite = m_InteractionItem == null ? UniFunc.FindSprite(m_GrabItemCode.itemCode) : null;
				m_GrabItemSprite.gameObject.SetActive(m_InteractionItem == null ? (m_GrabItemCode.itemCode > 0 ? true : false) : false);
			}
		}
		else if(m_InteractionItem != null)
		{
			m_InteractionItem.ItemInteraction(m_GrabItemCode.itemCode);
		}
	}

	//protected override void OnTriggerEnter(Collider collision)
	//{
	//	m_CPAComponent = collision.gameObject.GetComponent<CameraPresetAreaComponent>();
	//	if (m_CPAComponent != null)
	//	{
	//		m_CPAComponent.m_PlayerCharacter = this;
	//	}
	//}

	/// <summary>
	/// MouseButtonDown = true, MouseButtonUp = false
	/// </summary>
	/// <param name="p_MouseDown"> MouseButtonDown = true, MouseButtonUp = false </param>
	protected virtual void DoRaycast(bool p_MouseDown = true)
	{
		bool bMouseOnUI = UnityEngine.EventSystems.EventSystem.current != null ? UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() : false;
		Vector3 t_MousePosition = Vector3.zero;
		t_MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		bool result = Physics.Raycast(Camera.main.transform.position, t_MousePosition, out RaycastHit hit, Mathf.Infinity);
		if (result == true)
		{
			if (p_MouseDown == true)
			{ OnClickHit(hit, bMouseOnUI); }
			else if (p_MouseDown == false)
			{ OnReleaseHit(hit, bMouseOnUI); }
		}
		else if (result == false)
		{
			if (p_MouseDown == true)
			{ OnClickMiss(bMouseOnUI); }
			else if (p_MouseDown == false)
			{ OnReleaseMiss(bMouseOnUI); }
		}
		t_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit2D = Physics2D.Raycast(t_MousePosition, new Vector3(t_MousePosition.x, t_MousePosition.y, t_MousePosition.z + 100));
		if (hit2D == true)
		{
			if(p_MouseDown == true)
			{ OnClickHit2D(hit2D, bMouseOnUI); }
			else if (p_MouseDown == false)
			{ OnReleaseHit2D(hit2D, bMouseOnUI); }
		}
		else if (hit2D == false)
		{
			if (p_MouseDown == true)
			{ OnClickMiss2D(bMouseOnUI); }
			else if (p_MouseDown == false)
			{ OnReleaseMiss2D(bMouseOnUI); }
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

			PressHandle t_PressHandle = hit.transform.GetComponent<PressHandle>();
			if (t_PressHandle != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressHandle.gameObject);
				if (t_Press != null)
				{
					t_Press.bProgress = true;
				}
			}

			/*
			AccessoryPlate t_AccessoryPlate = hit.transform.GetComponent<AccessoryPlate>();
			if (t_AccessoryPlate != null)
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
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
								m_GrabItemSprite.gameObject.SetActive(true);
							}
							t_AccessoryPlate.m_Input = new AdvencedItem();
							t_AccessoryPlate.RefreshPlate();
						}
					}
				}
			}
			*/

			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput != null)
					{
						if (t_Press.m_AccessoryInput.itemAmount > 0)
						{
							if (m_GrabItemCode == null)
							{
								//m_Inventory.AddAItem(t_Press.m_AccessoryInput);
								SetPlayerGrabItem(t_Press.m_AccessoryInput);
								t_Press.m_AccessoryInput = null;
								t_Press.RefreshPlate();
							}
						}
					}
				}
			}

			/*
			PressOutput t_PressOutput = hit.transform.GetComponent<PressOutput>();
			if (t_PressOutput != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressOutput.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_CraftedItem != null)
					{
						if (t_Press.m_CraftedItem.itemAmount > 0)
						{
							if (m_GrabItemCode == null)
							{
								m_Inventory.AddAItem(t_Press.m_CraftedItem);
								m_GrabItemCode = t_Press.m_CraftedItem;
								if (m_GrabItemSprite != null)
								{
									m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
									m_GrabItemSprite.gameObject.SetActive(true);
								}
								t_Press.m_CraftedItem = null;
								t_Press.RefreshOutput();
							}
						}
					}
				}
			}
			*/

			IGrabable t_GrabableObject = hit.transform.GetComponent<IGrabable>();
			if (t_GrabableObject != null)
			{
				if (t_GrabableObject.IsGrabable() == true)
				{
					t_GrabableObject.SetGrabState(true);
				}
			}

			/*
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
			*/
		}
		else if (bMouseOnUI == true)
		{

		}
	}

	protected virtual void OnClickMiss(bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			//if (m_PlayerCharacterUIScript != null)
			//{
			//	if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
			//	{
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.gameObject.SetActive(false);
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_NPCInventory = null;
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
			//	}
			//}
		}
		else if (bMouseOnUI == true)
		{
		}
	}

	protected virtual void OnReleaseHit(RaycastHit hit, bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			/*
			MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
			if (t_MillStone != null)
			{
				if (t_MillStone.SetItem(m_GrabItemCode) == true)
				{
					m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
				}

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
			}
			*/

			/*
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
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
								m_GrabItemSprite.gameObject.SetActive(true);
							}
						}
					}
				}
			}
			*/

			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput == null)
					{
						//AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						//if (t_AItem != null)
						if (m_GrabItemCode != null)
						{
							//t_Press.m_AccessoryInput = t_AItem;
							t_Press.m_AccessoryInput = m_GrabItemCode;
							t_Press.RefreshPlate();
						}
					}
				}
			}

			/*
			*/
			PlayerShop t_PlayerShop = hit.transform.GetComponent<PlayerShop>();
			if (t_PlayerShop != null)
			{
				if (t_PlayerShop.itemCode == 0)
				{
					if (m_GrabItemCode.itemCode >= 28 && m_GrabItemCode.itemCode <= 57)
					{
						//AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						//if (m_Inventory.FindAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount) == true)
						//{
						//	t_PlayerShop.itemCode = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount).itemCode;
						//	t_PlayerShop.HandOverItem();
						//}
						t_PlayerShop.itemCode = m_GrabItemCode.itemCode;
						t_PlayerShop.HandOverItem();
					}
				}
			}

			TrashCan t_TrashCan = hit.transform.GetComponent<TrashCan>();
			if (t_TrashCan != null) 
			{
				if (m_GrabItemCode != null)
				{
					if (m_GrabItemCode.itemCode == 21)
					{
						//m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					}
				}
			}
		}
		else if (bMouseOnUI == true)
		{
			Canvas t_Canvas = FindObjectOfType<Canvas>();
			if(t_Canvas != null)
			{
				GraphicRaycaster t_GraphicRaycaster = t_Canvas.GetComponent<GraphicRaycaster>();
				if(t_GraphicRaycaster != null)
				{
					PointerEventData t_PointerEventData = new PointerEventData(null);
					t_PointerEventData.position = Input.mousePosition;
					List<RaycastResult> results = new List<RaycastResult>();
					t_GraphicRaycaster.Raycast(t_PointerEventData, results);

					for(int i = 0; i < results.Count; i = i + 1)
					{
						InventoryUIScript t_InventoryUIScript = UniFunc.GetParentComponent<InventoryUIScript>(results[i].gameObject);
						if (t_InventoryUIScript != null)
						{
							m_Inventory.AddAItem(m_GrabItemCode);

							List<AAA> t_AAA = FindObjectsOfType<AAA>().ToList();
							for(int j = 0; j < results.Count; j = j + 1)
							{
								if (t_AAA[i].m_ItemCode == m_GrabItemCode.itemCode)
								{
									Destroy(t_AAA[i].gameObject);
									break;
								}
							}
							break;
						}
					}
				}
			}
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

			PressHandle t_PressHandle = hit.transform.GetComponent<PressHandle>();
			if (t_PressHandle != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressHandle.gameObject);
				if (t_Press != null)
				{
					t_Press.bProgress = true;
				}
			}

			/*
			AccessoryPlate t_AccessoryPlate = hit.transform.GetComponent<AccessoryPlate>();
			if (t_AccessoryPlate != null)
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
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
								m_GrabItemSprite.gameObject.SetActive(true);
							}
							t_AccessoryPlate.m_Input = new AdvencedItem();
							t_AccessoryPlate.RefreshPlate();
						}
					}
				}
			}
			*/

			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput != null)
					{
						if (t_Press.m_AccessoryInput.itemAmount > 0)
						{
							if (m_GrabItemCode == null)
							{
								//m_Inventory.AddAItem(t_Press.m_AccessoryInput);
								SetPlayerGrabItem(t_Press.m_AccessoryInput);
								t_Press.m_AccessoryInput = null;
								t_Press.RefreshPlate();
							}
						}
					}
				}
			}

			/*
			PressOutput t_PressOutput = hit.transform.GetComponent<PressOutput>();
			if (t_PressOutput != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressOutput.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_CraftedItem != null)
					{
						if (t_Press.m_CraftedItem.itemAmount > 0)
						{
							if (m_GrabItemCode == null)
							{
								m_Inventory.AddAItem(t_Press.m_CraftedItem);
								m_GrabItemCode = t_Press.m_CraftedItem;
								if (m_GrabItemSprite != null)
								{
									m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
									m_GrabItemSprite.gameObject.SetActive(true);
								}
								t_Press.m_CraftedItem = null;
								t_Press.RefreshOutput();
							}
						}
					}
				}
			}
			*/

			IGrabable t_GrabableObject = hit.transform.GetComponent<IGrabable>();
			if (t_GrabableObject != null)
			{
				if (t_GrabableObject.IsGrabable() == true)
				{
					t_GrabableObject.SetGrabState(true);
				}
			}

			/*
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
			*/
		}
		else if (bMouseOnUI == true)
		{

		}
	}

	protected virtual void OnClickMiss2D(bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			//if (m_PlayerCharacterUIScript != null)
			//{
			//	if (m_PlayerCharacterUIScript.m_NPCStoreUIScript != null)
			//	{
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.gameObject.SetActive(false);
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.m_NPCInventory = null;
			//		m_PlayerCharacterUIScript.m_NPCStoreUIScript.RefreshUI();
			//	}
			//}
		}
		else if (bMouseOnUI == true)
		{
		}
	}

	protected virtual void OnReleaseHit2D(RaycastHit2D hit, bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			/*
			MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
			if (t_MillStone != null)
			{
				if (t_MillStone.SetItem(m_GrabItemCode) == true)
				{
					m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
				}

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
			}
			*/

			/*
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
								m_GrabItemSprite.sprite = UniFunc.FindSprite(m_GrabItemCode.itemCode);
								m_GrabItemSprite.gameObject.SetActive(true);
							}
						}
					}
				}
			}
			*/

			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput == null)
					{
						//AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						//if (t_AItem != null)
						if (m_GrabItemCode != null)
						{
							//t_Press.m_AccessoryInput = t_AItem;
							t_Press.m_AccessoryInput = m_GrabItemCode;
							t_Press.RefreshPlate();
						}
					}
				}
			}

			/*
			*/
			PlayerShop t_PlayerShop = hit.transform.GetComponent<PlayerShop>();
			if (t_PlayerShop != null)
			{
				if (t_PlayerShop.itemCode == 0)
				{
					if (m_GrabItemCode.itemCode >= 28 && m_GrabItemCode.itemCode <= 57)
					{
						//AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						//if (m_Inventory.FindAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount) == true)
						//{
						//	t_PlayerShop.itemCode = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount).itemCode;
						//	t_PlayerShop.HandOverItem();
						//}
						t_PlayerShop.itemCode = m_GrabItemCode.itemCode;
						t_PlayerShop.HandOverItem();
					}
				}
			}

			TrashCan t_TrashCan = hit.transform.GetComponent<TrashCan>();
			if (t_TrashCan != null)
			{
				if (m_GrabItemCode != null)
				{
					if (m_GrabItemCode.itemCode == 21)
					{
						//m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					}
				}
			}
		}
		else if (bMouseOnUI == true)
		{
			Canvas t_Canvas = FindObjectOfType<Canvas>();
			if (t_Canvas != null)
			{
				GraphicRaycaster t_GraphicRaycaster = t_Canvas.GetComponent<GraphicRaycaster>();
				if (t_GraphicRaycaster != null)
				{
					PointerEventData t_PointerEventData = new PointerEventData(null);
					t_PointerEventData.position = Input.mousePosition;
					List<RaycastResult> results = new List<RaycastResult>();
					t_GraphicRaycaster.Raycast(t_PointerEventData, results);

					for (int i = 0; i < results.Count; i = i + 1)
					{
						InventoryUIScript t_InventoryUIScript = UniFunc.GetParentComponent<InventoryUIScript>(results[i].gameObject);
						if (t_InventoryUIScript != null)
						{
							m_Inventory.AddAItem(m_GrabItemCode);

							List<AAA> t_AAA = FindObjectsOfType<AAA>().ToList();
							for (int j = 0; j < results.Count; j = j + 1)
							{
								if (t_AAA[i].m_ItemCode == m_GrabItemCode.itemCode)
								{
									Destroy(t_AAA[i].gameObject);
									break;
								}
							}
							break;
						}
					}
				}
			}
		}
	}

	protected virtual void OnReleaseMiss2D(bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
		}
		else if (bMouseOnUI == true)
		{
		}
	}
}
