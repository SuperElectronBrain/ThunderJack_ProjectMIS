using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

public enum PlayerCharacterState
{ Moveable, Communication, Fishing, }

public enum PlayerCharacterAnimation
{ FishingStart, FishingIdle, FishingHit, FishingBigHit, FishingPull, FishingSuccess, FishingFail, FishingEnd, Panic, }

public class PlayerCharacter : CharacterBase
{
	private PlayerCharacterState currentState = PlayerCharacterState.Moveable;

	private float m_FadeInTimeBase = 0.0f;
	private float m_FadeInTime = 0.0f;
	private float m_FadeOutTimeBase = 0.0f;
	private float m_FadeOutTime = 0.0f;
	private float m_MonologueDisplayTime = 0.0f;
	private float m_GuideDisplayTime = 0.0f;
	[SerializeField] private float m_MoveAnimationSpeed = 1.0f;
	private bool isInteractionIconUsed = false;

	//Component
	private CapsuleCollider m_Collider;
	[SerializeField] private CollisionComponent m_CollisionComponent;
	[HideInInspector] public RecipeBook m_RecipeBook;
	[HideInInspector] public QuestComponet m_QuestComponet;
	[HideInInspector] public TutorialComponent m_TutorialComponent;
	[HideInInspector] public IllustratedGuideComponent m_IllustratedGuideComponent;

	//UI
	[SerializeField] private GameObject m_PlayerCharacterUIPrefab;
	public PlayerCharacterUIScript m_PlayerCharacterUIScript;
	public UnityEngine.UI.Image m_GrabItemSprite;
	[HideInInspector] public AdvencedItem m_GrabItemCode = new AdvencedItem();
	public ItemInfoDisplay m_ItemInfoDisplay;
	[HideInInspector] public AdvencedItem m_HoverItemCode = new AdvencedItem();
	[SerializeField] private GameObject m_SpeechBubble;
	[SerializeField] private TextMeshPro m_SpeechBubbleText;
	[SerializeField] private UnityEngine.UI.Text m_SpeechBubbleTextLegacy;

	//VFX
	[SerializeField] private ParticleSystem m_FootStepEffectInside;
	[SerializeField] private ParticleSystem m_FootStepEffectOutdoor;

	//Animation
	[SerializeField] private Animator m_ShadowAnimator;

	//SFX
	[SerializeField] private AudioSource m_FootStepSound;

	//Interaction Target(NPC or Etc...)
	private List<InteractableObject> InteractableObjects = new List<InteractableObject>();
	private Transform m_InteractionTarget;

	//Interaction Item
	private InteractionItem m_InteractionItem;

	private GameObject m_GuideUI;
	public static PlayerCharacter main
	{ 
		get { return FindObjectOfType<PlayerCharacter>(); }
		set {; }
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		m_UseScaleFlip = false;

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
		if (m_IllustratedGuideComponent == null)
		{
			m_IllustratedGuideComponent = GetComponent<IllustratedGuideComponent>();
			if (m_IllustratedGuideComponent == null) { m_IllustratedGuideComponent = gameObject.AddComponent<IllustratedGuideComponent>(); } 
		}

		if (m_FootStepEffectInside != null) { m_FootStepEffectInside.Stop(); }
		if (m_FootStepEffectOutdoor != null) { m_FootStepEffectOutdoor.Stop(); }

		if (m_InteractionItem == null) { m_InteractionItem = GetComponent<InteractionItem>(); }

		EventManager.Subscribe(EventType.WorkTime, ForceStartBusiness);
		EventManager.Subscribe(EventType.StartInteraction, CommunicationStart);
		EventManager.Subscribe(EventType.EndIteraction, CommunicationEnd);
		EventManager.Subscribe(DialogEventType.ShopGemOpen, OpenBeilShop);
		EventManager.Subscribe(DialogEventType.ShopJewelryOpen, OpenGagaShop);
		FindPlayerCharacterUIScript();
	} 

	// Update is called once per frame
	protected override void Update()
	{
		float DeltaTime = Time.deltaTime;
		KeyInput();

		if (isWall == true)
		{
			m_HorizontalMove = 0;
			m_VerticalMove = 0;
		}

		GrabItemDisplayProcessor();
		ItemInfoDisplayProcessor();
		PlayerCharacterUIProcessor();
		
		MonologueDisplayProcessor(DeltaTime);
		GuideDisplayProcessor(DeltaTime);
		FadeProcessor(DeltaTime);

		FootStepEffectProcessor();
		FootStepSoundProcessor();
		AnimationProcessor();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		float DeltaTime = Time.fixedDeltaTime;
		GroundCheck();
		ForwardCheck();

		Vector3 t_HorizontalMoveDirection = Vector3.ProjectOnPlane(Camera.main.transform.right, m_GroundNormalVector);
		Vector3 t_VerticalMoveDirection = Vector3.ProjectOnPlane(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized, m_GroundNormalVector);
		SetMoveDirection(t_HorizontalMoveDirection, t_VerticalMoveDirection);
	}
	protected override void KeyInput()
	{
		if(currentState == PlayerCharacterState.Moveable)
		{
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "loading")
			{
				m_HorizontalMove = Input.GetAxis("Horizontal");
				m_VerticalMove = Input.GetAxis("Vertical");
				if (Input.GetAxisRaw("Horizontal") == 0.0f) { m_HorizontalMove = 0.0f; }
				if (Input.GetAxisRaw("Vertical") == 0.0f) { m_VerticalMove = 0.0f; }

				m_CharacterInputVector = ((m_HorizontalMoveDirection * m_HorizontalMove) + (m_VerticalMoveDirection * m_VerticalMove)).normalized;

				if (Input.GetKeyDown(KeyCode.Space) == true) { Jump(); }
				if (Input.GetKeyDown(KeyCode.E) == true)
				{
					InteractableObject t_InteractionObj = GetInteractableObject();
					if (t_InteractionObj.interaction != null)
					{
						bool binteractable = true;
						if (TutorialManager.Instance != null)
						{
							if (TutorialManager.Instance.currentState == TutorialStates.N2)
							{
								if (t_InteractionObj.interactionGO.name != "Portal (2)")
								{
									PopUpSpeechBubble("짐부터 푼 뒤에 우편함을 열어보자.", true);
									TutorialManager.Instance.WaitFewSeconds(() => { PopUpSpeechBubble("", false); }, 2);
									binteractable = false;
								}
							}
							else if (TutorialManager.Instance.currentState == TutorialStates.N6)
							{
								if (t_InteractionObj.interactionGO.name != "NoticeBoard")
								{
									PopUpSpeechBubble("우선 게시판부터 확인을 해보자.", true);
									TutorialManager.Instance.WaitFewSeconds(() => { PopUpSpeechBubble("", false); }, 2);
									binteractable = false;
								}
							}
							else if (TutorialManager.Instance.currentState == TutorialStates.N7)
							{
								if (t_InteractionObj.interactionGO.name == "베일")
								{
									TutorialManager.EventPublish(TutorialStates.N8);
									binteractable = false;
								}
							}
							else if(TutorialManager.Instance.currentState == TutorialStates.N9)
							{
								if (t_InteractionObj.interactionGO.name != "베일")
								{
									PopUpSpeechBubble("이러고 있을 시간이 없어. 내일 장사를 하기 위해서는 상인을 찾아가야 해.", true);
									TutorialManager.Instance.WaitFewSeconds(() => { PopUpSpeechBubble("", false); }, 3);
									binteractable = false;
								}
								else if (t_InteractionObj.interactionGO.name == "베일")
								{
									TutorialManager.EventPublish(TutorialStates.N10);
								}
							}
						}

						if (binteractable == true)
						{
							t_InteractionObj.interaction.Interaction(gameObject);
							m_InteractionTarget = t_InteractionObj.interactionGO.transform;
							Vector3 t_Vector = GetInteractionDirection();
						}
					}
					else if(t_InteractionObj.interaction == null)
					{
						PopUpInteractionIcon(false, Vector2.zero);
					}
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
					if (t_GameObject != null)
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
	}

	public void ChangeState(PlayerCharacterState newState)
	{
		currentState = newState;

		if (currentState == PlayerCharacterState.Communication)
		{
			m_HorizontalMove = 0.0f;
			m_VerticalMove = 0.0f;
			m_Rigidbody.velocity = Vector3.zero;
		}
		if (currentState == PlayerCharacterState.Fishing)
		{
			ChangeAnimationState(PlayerCharacterAnimation.FishingStart);
		}
	}

	public void ChangeAnimationState(PlayerCharacterAnimation newState)
	{
		if (m_Animator != null)
		{
			if (newState == PlayerCharacterAnimation.FishingStart)
			{
				m_Animator.SetTrigger("FishingStart"); 
			}
			else if (newState == PlayerCharacterAnimation.FishingIdle)
			{
				m_Animator.SetTrigger("FishingIdle");
			}
			else if (newState == PlayerCharacterAnimation.FishingHit)
			{
				m_Animator.SetTrigger("FishingHit");
			}
			else if (newState == PlayerCharacterAnimation.FishingBigHit)
			{
				m_Animator.SetTrigger("FishingHit");
			}
			else if (newState == PlayerCharacterAnimation.FishingPull)
			{
				m_Animator.SetTrigger("FishingPull");
			}
			else if (newState == PlayerCharacterAnimation.FishingSuccess)
			{
				m_Animator.SetTrigger("FishingSuccess"); 
			}
			else if (newState == PlayerCharacterAnimation.FishingFail)
			{
				m_Animator.SetTrigger("FishingFail");
			}
			else if (newState == PlayerCharacterAnimation.FishingEnd)
			{
			}
		}
	}

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
				m_Inventory.CleanInventory();
				m_Inventory.TakeData(p_PlayerCharacter.m_Inventory);
			}

			if(m_RecipeBook == null)
			{
				m_RecipeBook = gameObject.GetComponent<RecipeBook>();
				if (m_RecipeBook == null) { m_RecipeBook = gameObject.AddComponent<RecipeBook>(); }
			}
			if (m_RecipeBook != null)
			{
				m_RecipeBook.TakeItemRecipes(p_PlayerCharacter.m_RecipeBook);
			}

			if(m_IllustratedGuideComponent == null)
			{
				m_IllustratedGuideComponent = gameObject.GetComponent<IllustratedGuideComponent>();
				if(m_IllustratedGuideComponent == null) { m_IllustratedGuideComponent = gameObject.AddComponent<IllustratedGuideComponent>(); }
			}
			if(m_IllustratedGuideComponent != null)
			{
				m_IllustratedGuideComponent.TakeData(p_PlayerCharacter.m_IllustratedGuideComponent);
			}

			if(m_QuestComponet == null)
			{
				m_QuestComponet = gameObject.GetComponent<QuestComponet>();
				if (m_QuestComponet == null) { m_QuestComponet = gameObject.AddComponent<QuestComponet>(); }
			}
			if (m_QuestComponet != null)
			{
				m_QuestComponet.TakeData(p_PlayerCharacter.m_QuestComponet);
			}

			if (m_TutorialComponent == null)
			{
				m_TutorialComponent = gameObject.GetComponent<TutorialComponent>();
				if (m_TutorialComponent == null) { m_TutorialComponent = gameObject.AddComponent<TutorialComponent>(); }
			}
			if (m_TutorialComponent != null)
			{
				m_TutorialComponent.TakeStates(p_PlayerCharacter.m_TutorialComponent);
			}
		}
	}

	public void ForceStartBusiness()
	{
		ChangeAnimationState(PlayerCharacterAnimation.Panic);

		GameObject go = new GameObject();
		go.name = "Portal";
		Portal portal = go.AddComponent<Portal>();
		portal.destinationSceneName = "Build_Inside";
		portal.LoadingScene();
	}

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
			BasicItemData basicItemData = UniFunc.FindItemData(m_GrabItemCode.itemCode);
			if (basicItemData != null)
			{
				if (basicItemData.itemType != ItemType.Jewelry)
				{
					m_InteractionItem.ItemInteraction(m_GrabItemCode.itemCode);
				}
			}
		}
	}

	private void AnimationProcessor()
	{
		if (m_Animator != null)
		{
			m_Animator.SetFloat("MoveSpeed", (m_Velocity / m_Speed) * m_MoveAnimationSpeed);
			m_Animator.SetFloat("HorizontalSpeed", m_UseScaleFlip ? (m_HorizontalMove < 0 ? -m_HorizontalMove : m_HorizontalMove) : m_HorizontalMove);
			m_Animator.SetFloat("VerticalSpeed", m_VerticalMove);
		}
		if (m_ShadowAnimator != null)
		{
			m_ShadowAnimator.SetFloat("MoveSpeed", (m_Velocity / m_Speed) * m_MoveAnimationSpeed);
			m_ShadowAnimator.SetFloat("HorizontalSpeed", m_UseScaleFlip ? (m_HorizontalMove < 0 ? -m_HorizontalMove : m_HorizontalMove) : m_HorizontalMove);
			m_ShadowAnimator.SetFloat("VerticalSpeed", m_VerticalMove);
		}
	}
	private void FootStepEffectProcessor()
	{
		if (m_FootStepEffectInside != null || m_FootStepEffectOutdoor != null)
		{
			if (m_CollisionCount > 0)
			{
				if (m_HorizontalMove != 0)
				{
					if (m_FootStepEffectInside != null)
					{
						if (m_FootStepEffectInside.isPlaying != true)
						{
							m_FootStepEffectInside.Play();
						}
					}
				}
				else if (m_HorizontalMove == 0)
				{
					if (m_FootStepEffectInside != null) { m_FootStepEffectInside.Stop(); }
					if (m_FootStepEffectOutdoor != null) { m_FootStepEffectOutdoor.Stop(); }
				}
			}

			Vector3 t_ModelingScale = (m_FootStepEffectInside != null ? m_FootStepEffectInside : m_FootStepEffectOutdoor).transform.parent.localScale;
			if (m_HorizontalMove > 0) { t_ModelingScale.x = -1.0f; }
			else if (m_HorizontalMove < 0) { t_ModelingScale.x = 1.0f; }
			(m_FootStepEffectInside != null ? m_FootStepEffectInside : m_FootStepEffectOutdoor).transform.parent.localScale = t_ModelingScale;
		}
	}
	private void FootStepSoundProcessor()
	{
		if (m_Velocity > 0.1f)
		{
			if (isGround == true)
			{
				if (m_FootStepSound != null)
				{
					if (m_FootStepSound.isPlaying == false)
					{
						m_FootStepSound.Play();
					}
				}
			}
			else if (isGround == false)
			{
				if (m_FootStepSound != null)
				{
					if (m_FootStepSound.isPlaying == true)
					{
						m_FootStepSound.Stop();
					}
				}
			}
		}
		else if(m_Velocity < 0.1f)
		{
			if (m_FootStepSound != null)
			{
				if (m_FootStepSound.isPlaying == true)
				{
					m_FootStepSound.Stop();
				}
			}
		}
	}

	#region Interaction
	public Vector3 GetInteractionDirection()
	{
		Vector3 direction = Vector3.zero;
		if(m_InteractionTarget != null)
		{
			direction = Camera.main.transform.InverseTransformDirection(m_InteractionTarget.position - transform.position);
			direction.y = 0;
		}
		return direction; 
	}

	//가장 가깝고 카메라가 바라보는 각도와 가장 많이 일지하는 위치에 있는 상호작용 대상의 참조를 찾는 함수
	public InteractableObject GetInteractableObject()
	{
		InteractableObject t_Interaction = new InteractableObject(null, null);
		if (InteractableObjects != null)
		{
			//상호작용 대상이 1개 이상 존재한다면
			if (InteractableObjects.Count > 0)
			{
				float t_DotProduct = -1.0f;
				for (int i = 0; i < InteractableObjects.Count; i = i + 1)
				{
					//상호작용 대상이 게임오브젝트로써 존재한다면
					if(InteractableObjects[i].interactionGO != null)
					{
						//상호작용 대상이 비활성화된 상태가 아니라면
						if (InteractableObjects[i].interactionGO.activeSelf == true)
						{
							//상호작용 대상의 콜라이더가 켜져있는 상태라면
							if (InteractableObjects[i].interactionGO.GetComponent<Collider>().enabled == true)
							{
								//현재 카메라가 바라보는 벡터와 플레이어를 중심으로 계산한 상호작용 대상까지의 벡터로 내적을 실행
								float t_DotProduct1 = Vector3.Dot(Camera.main.transform.forward, (InteractableObjects[i].interactionGO.transform.position - transform.position).normalized);
								//이전에 계산된 내적 값보다 지금 계산된 내적 값이 더 크다면
								if (t_DotProduct < t_DotProduct1)
								{
									//내적 값을 갱신하고 상호작용 대상을 재지정함
									t_DotProduct = t_DotProduct1;
									t_Interaction = new InteractableObject(InteractableObjects[i].interaction, InteractableObjects[i].interactionGO);
								}
							}
						}
					}
				}
			}
		}
		return t_Interaction;
	}
	private void OnInteractableObjectEnter(Collider other)
	{
		if(other.gameObject != gameObject)
		{
			IInteraction t_Interaction = other.gameObject.GetComponent<IInteraction>();
			if (t_Interaction != null)
			{
				if(other.gameObject.GetComponent<INpcOnly>() == null)
				{
					if (InteractableObjects == null) { InteractableObjects = new List<InteractableObject>(); }
					if (t_Interaction != InteractableObjects.Find((InteractableObject x) => { return x.interaction == t_Interaction; }).interaction)
					{
						InteractableObjects.Add(new InteractableObject(t_Interaction, other.gameObject));
					}

				}
			}
		}
		
		PopUpInteractionIcon(InteractableObjects.Count > 0, Vector2.zero);
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

		PopUpInteractionIcon(InteractableObjects.Count > 0, Vector2.zero);
	}
	public virtual void CommunicationStart()
	{
		ChangeState(PlayerCharacterState.Communication);
		PopUpInteractionIcon(false, Vector2.zero);
	}
	public virtual void CommunicationEnd()
	{
		ChangeState(PlayerCharacterState.Moveable);
		m_InteractionTarget = null;
		//CloseNPCShop();
		PopUpInteractionIcon(true, Vector2.zero);
	}
	#endregion

	#region NPCShop
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
	#endregion

	#region PlayerUI
	public void FindPlayerCharacterUIScript()
	{
		if (m_PlayerCharacterUIScript == null) { m_PlayerCharacterUIScript = FindObjectOfType<PlayerCharacterUIScript>(); }
		if (m_PlayerCharacterUIScript == null)
		{
			if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "loading")
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
					m_Inventory.m_MoneyText = m_PlayerCharacterUIScript.m_CurrencyUIScript.m_MoneyText;
				}
				if (m_Inventory.m_HonerText == null)
				{
					m_Inventory.m_HonerText = m_PlayerCharacterUIScript.m_CurrencyUIScript.m_HonerText;
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
			if(m_IllustratedGuideComponent != null)
			{
				if (m_IllustratedGuideComponent.m_IllustratedGuideUIScript == null)
				{
					if(m_PlayerCharacterUIScript.m_IllustratedGuideUIScript != null)
					{
						m_IllustratedGuideComponent.m_IllustratedGuideUIScript = m_PlayerCharacterUIScript.m_IllustratedGuideUIScript;
						m_IllustratedGuideComponent.m_IllustratedGuideUIScript.m_IllustratedGuideComponent = m_IllustratedGuideComponent;
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
						m_QuestComponet.m_MailBoxUIScript.m_QuestComponet = m_QuestComponet;
					}
				}
			}
		}
	}
	public void PlayerCharacterUIProcessor() 
	{
		if(isInteractionIconUsed == false)
		{
			if (m_PlayerCharacterUIScript != null)
			{
				if (m_PlayerCharacterUIScript.m_InteractionIcon != null)
				{
					if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect != null)
					{
						InteractableObject t_InteractableObject = GetInteractableObject();
						if (t_InteractableObject.interaction != null)
						{
							Vector3 t_Vector = Camera.main.WorldToScreenPoint(t_InteractableObject.interactionGO.transform.position);
							t_Vector.x = t_Vector.x + 0;
							t_Vector.y = t_Vector.y + 75;
							m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect.position = t_Vector;

							if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText != null)
							{
								m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "상호작용";

								NPC t_NPC = t_InteractableObject.interaction as NPC;
								NoticeBoard t_NoticeBoard = t_InteractableObject.interaction as NoticeBoard;
								if (t_NPC != null || t_NoticeBoard != null)
								{
									if (t_NPC != null) { m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "대화하기"; }
									else if (t_NoticeBoard != null) { m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "확인하기"; }
									if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage != null)
									{
										if (m_PlayerCharacterUIScript.m_InteractionIcon.m_Talk != null)
										{
											m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage.sprite = m_PlayerCharacterUIScript.m_InteractionIcon.m_Talk;
										}
									}
								}
								Flower t_Flower = t_InteractableObject.interaction as Flower;
								FlowerPot t_FlowerPot = t_InteractableObject.interaction as FlowerPot;
								if (t_Flower != null || t_FlowerPot != null)
								{
									if (t_Flower != null) { m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "수확하기"; }
									else if (t_FlowerPot != null) { m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "밭 가꾸기"; }
									if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage != null)
									{
										if (m_PlayerCharacterUIScript.m_InteractionIcon.m_Plants != null)
										{
											m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage.sprite = m_PlayerCharacterUIScript.m_InteractionIcon.m_Plants;
										}
									}
								}
								Door t_Door = t_InteractableObject.interaction as Door;
								InteractablePortal t_Portal = t_InteractableObject.interaction as InteractablePortal;
								if (t_Door != null || t_Portal != null)
								{
									m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "이동하기";
									if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage != null)
									{
										if (m_PlayerCharacterUIScript.m_InteractionIcon.m_Open != null)
										{
											m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage.sprite = m_PlayerCharacterUIScript.m_InteractionIcon.m_Open;
										}
									}
								}
								Mailbox t_Mailbox = t_InteractableObject.interaction as Mailbox;
								if (t_Mailbox != null)
								{
									m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "확인하기";
									if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage != null)
									{
										if (m_PlayerCharacterUIScript.m_InteractionIcon.m_Mail != null)
										{
											m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage.sprite = m_PlayerCharacterUIScript.m_InteractionIcon.m_Mail;
										}
									}
								}
								FishingPointComponent t_Fish = t_InteractableObject.interaction as FishingPointComponent;
								if (t_Fish != null)
								{
									m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = "낚시하기";
									if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage != null)
									{
										if (m_PlayerCharacterUIScript.m_InteractionIcon.m_Fish != null)
										{
											m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionImage.sprite = m_PlayerCharacterUIScript.m_InteractionIcon.m_Fish;
										}
									}
								}
							}
						}
						else if (t_InteractableObject.interaction == null)
						{
							m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect.gameObject.SetActive(false);
						}
					}
				}
			}
		}
	}
	#endregion

	#region UIControll
	public void PopUpInteractionIcon(bool param, Vector2 pVector, string pString = "상호작용", bool pCall = false)
	{
		isInteractionIconUsed = pCall;
		if (m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_InteractionIcon != null)
			{
				m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect.position = pVector;
				if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect.gameObject.activeSelf != param)
				{
					m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionIconRect.gameObject.SetActive(param);

					if (m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText != null)
					{
						m_PlayerCharacterUIScript.m_InteractionIcon.m_InteractionText.text = pString;
					}
				}
			}
		}
	}
	public void PopUpSpeechBubble(string p_Script, bool bParam)
	{
		if(m_SpeechBubble != null)
		{
			if (m_SpeechBubble.activeSelf != bParam) { m_SpeechBubble.SetActive(bParam); }
			if (m_SpeechBubbleText != null)
			{
				if(m_SpeechBubbleTextLegacy != null)
				{
					m_SpeechBubbleTextLegacy.text = p_Script;

					m_SpeechBubbleTextLegacy.DOKill();
					m_SpeechBubbleTextLegacy.text = null;
					m_SpeechBubbleTextLegacy.DOText(p_Script, 1f).OnUpdate(() =>
					{
						m_SpeechBubbleText.text = m_SpeechBubbleTextLegacy.text;
					});
				}
			}
		}
	}
	public void PopUpMonologue(string p_Script, bool bParam)
	{
		if (m_PlayerCharacterUIScript != null)
		{
			if (m_PlayerCharacterUIScript.m_MonologueUI != null)
			{
				if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO != null)
				{
					if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.activeSelf != bParam)
					{
						m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueGO.SetActive(bParam);
					}
					if (m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText != null)
					{
						m_PlayerCharacterUIScript.m_MonologueUI.m_MonologueText.text = p_Script;
					}
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
	#endregion
	
	#region  UIProcessor
	private void GrabItemDisplayProcessor()
	{
		if ((m_GrabItemSprite != null ? m_GrabItemSprite.gameObject.activeSelf : false) == true)
		{
			m_GrabItemSprite.rectTransform.position = Input.mousePosition;
		}
	}
	private void ItemInfoDisplayProcessor()
	{
		if (m_ItemInfoDisplay != null)
		{
			if (m_ItemInfoDisplay.m_ItemInfoDisplayGO != null)
			{
				if (m_ItemInfoDisplay.m_ItemInfoDisplayGO.activeSelf == true)
				{
					if (m_ItemInfoDisplay.m_ItemInfoDisplayRect != null)
					{
						m_ItemInfoDisplay.m_ItemInfoDisplayRect.position = Input.mousePosition;
					}
				}

				if (m_PlayerCharacterUIScript != null)
				{
					if (m_PlayerCharacterUIScript.m_InventoryUIScript != null)
					{
						if (m_PlayerCharacterUIScript.m_InventoryUIScript.gameObject.activeSelf == false)
						{
							m_ItemInfoDisplay.m_ItemInfoDisplayGO.SetActive(false);
						}
					}
				}

				if (Input.GetMouseButton(0) == true)
				{
					m_ItemInfoDisplay.m_ItemInfoDisplayGO.SetActive(false);
				}
			}
		}
	}
	private void MonologueDisplayProcessor(float DeltaTime)
	{
		if (m_MonologueDisplayTime > 0.0f)
		{
			m_MonologueDisplayTime = m_MonologueDisplayTime - DeltaTime;
			if (m_MonologueDisplayTime < 0.0f)
			{
				PopUpMonologue("", 0.0f);

				if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.PopUpMonologue) { m_TutorialComponent.ProgressTutorial(); } }
				m_MonologueDisplayTime = 0.0f;
			}
		}
	}
	private void GuideDisplayProcessor(float DeltaTime)
	{
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
	}
	private void FadeProcessor(float DeltaTime)
	{
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
				//if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.FadeIn) { m_TutorialComponent.ProgressTutorial(); } }
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
				//if (m_TutorialComponent != null) { if (m_TutorialComponent.GetCurrentStateType() == StateType.FadeOut) { m_TutorialComponent.ProgressTutorial(); } }
				m_FadeOutTime = 0.0f;
				m_FadeOutTimeBase = 0.0f;
			}
		}
	}
	#endregion

	#region RayCast
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

			IGrabable t_GrabableObject = hit.transform.GetComponent<IGrabable>();
			if (t_GrabableObject != null)
			{
				if (t_GrabableObject.IsGrabable() == true)
				{
					t_GrabableObject.SetGrabState(true);
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
		}
		else if (bMouseOnUI == true)
		{
		}
	}
	protected virtual void OnReleaseHit(RaycastHit hit, bool bMouseOnUI)
	{
		if (bMouseOnUI == false)
		{
			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput == null)
					{
						AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						if (t_AItem != null)
						{
							t_Press.m_AccessoryInput = t_AItem;
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
						if (m_Inventory.FindAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount) == true)
						{
							t_PlayerShop.itemCode = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount).itemCode;
							t_PlayerShop.HandOverItem();
						}
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
						m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					}
				}
			}
		}
		//마우스가 UI 위에 존재한다면
		else if (bMouseOnUI == true)
		{
			BasicItemData basicItemData = UniFunc.FindItemData(m_GrabItemCode.itemCode);
			if (basicItemData != null)
			{
				if (basicItemData.itemType != ItemType.Jewelry)
				{
					Canvas t_Canvas = FindObjectOfType<Canvas>();
					//현재 씬에 캔버스가 존재한다면
					if (t_Canvas != null)
					{
						GraphicRaycaster t_GraphicRaycaster = t_Canvas.GetComponent<GraphicRaycaster>();
						//캔버스에 GraphicRaycaster가 달려있다면
						if (t_GraphicRaycaster != null)
						{
							PointerEventData t_PointerEventData = new PointerEventData(null);
							t_PointerEventData.position = Input.mousePosition;
							List<RaycastResult> results = new List<RaycastResult>();
							t_GraphicRaycaster.Raycast(t_PointerEventData, results);

							//GraphicRaycaster를 이용해 마우스가 가리키는 방향으로 레이를 발사하여, 레이에 걸린 UI들이 존재한다면
							for (int i = 0; i < results.Count; i = i + 1)
							{
								InventoryUIScript t_InventoryUIScript = UniFunc.GetParentComponent<InventoryUIScript>(results[i].gameObject);
								//레이캐스트에 걸린 UI들중 인벤토리가 있다면
								if (t_InventoryUIScript != null)
								{
									//현재 씬의 플레이어 캐릭터가 InteractionItem 컴포넌트를 가지고 있다면
									if (m_InteractionItem != null)
									{
										List<AAA> t_AAA = FindObjectsOfType<AAA>().ToList();
										if (t_AAA != null)
										{
											//현재 씬에 물리 아이템이 하나라도 존재한다면
											for (int j = 0; j < t_AAA.Count; j = j + 1)
											{
												//현재 씬에 존재하는 물리 아이템중, 현재 캐릭터가 마우스로 드래그하여 들고 있는 아이템과 아이템 코드가 일치하는 것이 있다면
												if (t_AAA[j].m_ItemCode == m_GrabItemCode.itemCode)
												{
													//해당 물리 아이템을 삭제함
													Destroy(t_AAA[j].gameObject);
													t_AAA.TrimExcess();
													break;
												}
											}
										}
									}

									//인벤토리에 현재 캐릭터가 드래그하여 들고있는 아이템을 다시 넣어줌(현재 캐릭터가 들고있는 아이템을 Null로 만드는 것은 다음 프레임의 Update에서 실행됨)
									m_Inventory.AddAItem(m_GrabItemCode);
									break;
								}
							}
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

			//BBB t_BBB = hit.transform.GetComponent<BBB>();
			//if(t_BBB != null)
			//{
			//	AAA t_AAA = UniFunc.GetParentComponent<AAA>(t_BBB.transform);
			//	if(t_AAA != null)
			//	{
			//		SetPlayerGrabItem(new AdvencedItem(t_AAA.m_ItemCode, 1, 1));
			//	}
			//}

			//AAA t_AAA = hit.transform.GetComponent<AAA>();
			//if (t_AAA != null)
			//{
			//	SetPlayerGrabItem(new AdvencedItem(t_AAA.m_ItemCode, 1, 1));
			//}
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
			PressAccessoryPlate t_PressAccessoryPlate = hit.transform.GetComponent<PressAccessoryPlate>();
			if (t_PressAccessoryPlate != null)
			{
				Press t_Press = UniFunc.GetParentComponent<Press>(t_PressAccessoryPlate.gameObject);
				if (t_Press != null)
				{
					if (t_Press.m_AccessoryInput == null)
					{
						AdvencedItem t_AItem = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
						if (t_AItem != null)
						{
							t_Press.m_AccessoryInput = t_AItem;
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
						if (m_Inventory.FindAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount) == true)
						{
							t_PlayerShop.itemCode = m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount).itemCode;
							t_PlayerShop.HandOverItem();
						}
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
						m_Inventory.PopAItem(m_GrabItemCode.itemCode, m_GrabItemCode.itemProgress, m_GrabItemCode.itemAmount);
					}
				}
			}
		}
		else if (bMouseOnUI == true)
		{
			BasicItemData basicItemData = UniFunc.FindItemData(m_GrabItemCode.itemCode);
			if (basicItemData != null)
			{
				if (basicItemData.itemType != ItemType.Jewelry)
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
#endregion

	#region Collision

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
	}

	protected override void OnCollisionExit(Collision collision)
	{
		base.OnCollisionExit(collision);
	}
	#endregion
}
