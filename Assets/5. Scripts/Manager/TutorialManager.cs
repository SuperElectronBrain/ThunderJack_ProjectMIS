using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TutorialEventType { None,  }
public enum TutorialStates { None, N0, N1, N2, N3, N4, N5, N6, N7, N8, N9, N10, N11, N12, N13, N14, N15, N16, N17, N18, N19, EndOfTutorial }

[Serializable]
public class TutorialEvent
{
	public TutorialEventType key;
	public UnityEngine.Events.UnityEvent value;
}

public class TutorialManager : MonoBehaviour
{
	//Instance
	private static TutorialManager instance = null;
	public static TutorialManager Instance
	{ 
		get 
		{ 
			if(instance == null)
			{ instance = FindObjectOfType<TutorialManager>(); }
			if (instance != null)
			{
				DontDestroyOnLoad(instance.gameObject);
				TutorialManager[] TutorialManagers = FindObjectsOfType<TutorialManager>();
				for (int i = 0; i < TutorialManagers.Length; i = i + 1)
				{
					if(TutorialManagers[i] != instance)
					{ Destroy(TutorialManagers[i]); }
				}
			}
			return instance;
		} 
	}

	//Event
	private List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();
	public List<TutorialEvent> TutorialEvents
	{ 
		get 
		{
			if(tutorialEvents == null)
			{
				tutorialEvents = new List<TutorialEvent>();
			}
			return tutorialEvents; 
		}
	}

	//FSM
	public TutorialStates currentState = TutorialStates.None;
	private FiniteStateMachine<TutorialManager> finiteStateMachine = new FiniteStateMachine<TutorialManager>();

	[Header("VirtualCamera")]
	public CinemachineVirtualCamera cinemachineVirtual;
	public CinemachineVirtualCamera cinemachineVirtual1;
	public CinemachineVirtualCamera cinemachineVirtual2;

	[Header("DummyNPC")]
	public TutorialNPC redin;
	public TutorialNPC redin1;
	public TutorialNPC dokan;
	public TutorialNPC beil;
	public TutorialNPC cador;

	[Header("Trigger")]
	public GameObject Trigger12;

	[Header("Wall")]
	public GameObject wall7;
	public GameObject wall8;
	public GameObject wall9;

	[Header("Effect")]
	public ParticleSystem coinThrowingEffect;
	public ParticleSystem coinDonationEffect;

	public static bool EventSubscribe(TutorialEventType pEventType, UnityEngine.Events.UnityAction pAction)
    {
		bool bSuccessed = false;
		if (Instance != null)
		{
			TutorialEvent tutorialEvent = Instance.TutorialEvents.Find((TutorialEvent e) =>
			{
				if (e.key == pEventType)
				{ e.value.AddListener(pAction); }
				return e.key == pEventType;
			});
			if (tutorialEvent == null)
			{
				tutorialEvent = new TutorialEvent();
				tutorialEvent.key = pEventType;
				tutorialEvent.value = new UnityEngine.Events.UnityEvent();
				tutorialEvent.value.AddListener(pAction);
				Instance.TutorialEvents.Add(tutorialEvent);
			}
			bSuccessed = true;
		}
		return bSuccessed;
	}

	public static bool EventUnsubscribe(TutorialEventType pEventType, UnityEngine.Events.UnityAction pAction)
    {
		bool bSuccessed = false;
		if (Instance != null)
		{
			Instance.TutorialEvents.Find((TutorialEvent e) =>
			{
				if (e.key == pEventType)
				{ 
					e.value.RemoveListener(pAction);
					bSuccessed = true;
				}
				return e.key == pEventType;
			});
		}
		return bSuccessed;
	}

	public static bool EventPublish(TutorialEventType param)
    {
		bool bSuccessed = false;
		if (Instance != null)
		{
			Instance.TutorialEvents.Find((TutorialEvent e) =>
			{
				if (e.key == param)
				{
					e.value.Invoke();
					bSuccessed = true;
				}
				return e.key == param;
			});
		}
		return bSuccessed;
	}

	public static void EventPublish(TutorialStates param)
	{
		if (Instance.currentState != TutorialStates.EndOfTutorial)
		{
			Instance.currentState = param;
			Instance.finiteStateMachine.ChangeState(param);
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<TutorialManager>();
			if (instance != null)
			{
				DontDestroyOnLoad(instance.gameObject);
				TutorialManager[] TutorialManagers = FindObjectsOfType<TutorialManager>();
				for (int i = 0; i < TutorialManagers.Length; i = i + 1)
				{
					if (TutorialManagers[i] != instance)
					{ Destroy(TutorialManagers[i]); }
				}
			}
		}
		if (instance != null)
		{
			if (instance != this)
			{
				Destroy(gameObject);
			}
		}
	}

	private void Start()
	{
		finiteStateMachine.entitiy = this;
		finiteStateMachine.AddState(TutorialStates.N0, new TutorialCondition0());
		finiteStateMachine.AddState(TutorialStates.N1, new TutorialCondition1());
		finiteStateMachine.AddState(TutorialStates.N2, new TutorialCondition2());
		finiteStateMachine.AddState(TutorialStates.N3, new TutorialCondition3());
		finiteStateMachine.AddState(TutorialStates.N4, new TutorialCondition4());
		finiteStateMachine.AddState(TutorialStates.N5, new TutorialCondition5());
		finiteStateMachine.AddState(TutorialStates.N6, new TutorialCondition6());
		finiteStateMachine.AddState(TutorialStates.N7, new TutorialCondition7());
		finiteStateMachine.AddState(TutorialStates.N8, new TutorialCondition8());
		finiteStateMachine.AddState(TutorialStates.N9, new TutorialCondition9());
		finiteStateMachine.AddState(TutorialStates.N10, new TutorialCondition10());
		finiteStateMachine.AddState(TutorialStates.N11, new TutorialCondition11());
		finiteStateMachine.AddState(TutorialStates.N12, new TutorialCondition12());
		finiteStateMachine.AddState(TutorialStates.N13, new TutorialCondition13());
		finiteStateMachine.AddState(TutorialStates.N14, new TutorialCondition14());
		finiteStateMachine.AddState(TutorialStates.N15, new TutorialCondition15());
		finiteStateMachine.AddState(TutorialStates.N16, new TutorialCondition16());
		finiteStateMachine.AddState(TutorialStates.N17, new TutorialCondition17());
		finiteStateMachine.AddState(TutorialStates.N18, new TutorialCondition18());
		finiteStateMachine.AddState(TutorialStates.N19, new TutorialCondition19());
		finiteStateMachine.AddState(TutorialStates.EndOfTutorial, new EndOfTutorial());

		GameObject titleScreen = GameObject.Find("TitleScreenBackGround");
		if(titleScreen != null)
		{
			GameObject startButton = UniFunc.GetChildOfName(titleScreen, "StartButton");
			if(startButton != null)
			{
				UnityEngine.UI.Button button = startButton.GetComponent<UnityEngine.UI.Button>();
				if(button != null)
				{
					button.onClick.AddListener(StartTutorial);
				}
			}

			GameObject loadButton = UniFunc.GetChildOfName(titleScreen, "LoadButton");
			if (loadButton != null)
			{
				UnityEngine.UI.Button button = loadButton.GetComponent<UnityEngine.UI.Button>();
				if (button != null)
				{
					button.onClick.AddListener(SkipTutorial);
				}
			}
		}
	}

	private void Update()
	{
		if(finiteStateMachine.currState != null)
		{ finiteStateMachine.currState.StateUpdate(this); }
	}

	public void WaitFewSeconds(UnityEngine.Events.UnityAction pAction, float time)
	{
		StartCoroutine(Coroutine(time, pAction));
	}

	private IEnumerator Coroutine(float time, UnityEngine.Events.UnityAction pAction)
	{
		yield return new WaitForSeconds(time);
		pAction.Invoke();
	}

	private void StartTutorial()
	{
		finiteStateMachine.ChangeState(TutorialStates.N0);

		GameObject titleScreen = GameObject.Find("TitleScreenBackGround");
		if (titleScreen != null)
		{
			GameObject startButton = UniFunc.GetChildOfName(titleScreen, "StartButton");
			if (startButton != null)
			{
				UnityEngine.UI.Button button = startButton.GetComponent<UnityEngine.UI.Button>();
				if (button != null)
				{
					button.onClick.RemoveListener(StartTutorial);
				}
			}
		}
	}

	private void SkipTutorial()
	{
		finiteStateMachine.ChangeState(TutorialStates.EndOfTutorial);

		GameObject titleScreen = GameObject.Find("TitleScreenBackGround");
		if (titleScreen != null)
		{
			GameObject loadButton = UniFunc.GetChildOfName(titleScreen, "LoadButton");
			if (loadButton != null)
			{
				UnityEngine.UI.Button button = loadButton.GetComponent<UnityEngine.UI.Button>();
				if (button != null)
				{
					button.onClick.RemoveListener(SkipTutorial);
				}
			}
		}
	}
}
