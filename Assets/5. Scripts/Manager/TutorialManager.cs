using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TutorialEventType { None,  }
public enum TutorialStates { None, N0, N1, N2, N3, N4, N5, N6, N7 }

[Serializable]
public class TutorialEvent
{
	public TutorialEventType key;
	public UnityEngine.Events.UnityEvent value;
}

public class TutorialManager : MonoBehaviour
{
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

	List<int> TutorialTask = new List<int>();

	private FiniteStateMachine<TutorialManager> finiteStateMachine = new FiniteStateMachine<TutorialManager>();

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
		Instance.finiteStateMachine.ChangeState(param);
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

		finiteStateMachine.ChangeState(TutorialStates.N0);
	}

	private void Update()
	{
		finiteStateMachine.currState.StateUpdate(this);
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
}
