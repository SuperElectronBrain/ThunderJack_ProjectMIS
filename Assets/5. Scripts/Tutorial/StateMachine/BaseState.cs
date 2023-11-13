using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<T>
{
	public T entitiy;

	public BaseState<T> currState;

	[SerializeField] protected Dictionary<TutorialStates, BaseState<T>> States = new Dictionary<TutorialStates, BaseState<T>>();

	public void AddState(TutorialStates stateType, BaseState<T> newState)
	{
		States.Add(stateType, newState);
	}

	public void ChangeState(TutorialStates newState)
	{
		if (currState != null)
		{ currState.StateEnd(entitiy); }

		currState = States[newState];

		if (currState != null)
		{ currState.StateBegin(entitiy); }
	}
}

public abstract class BaseState<T>
{
	public abstract void StateBegin(T param);
	public abstract void StateUpdate(T param);
	public abstract void StateEnd(T param);
}

//게임 시작시
public class TutorialCondition0 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("먼길 오느라 잠깐 쉬었으니… 다시 출발해볼까 곧 도착할거야", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{
	}
	public override void StateEnd(TutorialManager param)
	{
	}
}

//다리 진입시
public class TutorialCondition1 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacterUIScript.main.PopupTitle(true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacterUIScript.main.PopupTitle(false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{
	}
	public override void StateEnd(TutorialManager param)
	{
	}
}

//다리 퇴장시
public class TutorialCondition2 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("왼쪽 기슭근처 숲 속에 있는 가게에 가보자", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//가게 입장시
public class TutorialCondition3 : BaseState<TutorialManager>
{
	private bool trigger0 = false;
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("먼지가 가득 쌓였네…", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, "청소하기", true); }, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{
		if(Input.GetKeyDown(KeyCode.E) == true)
		{
			if (trigger0 == false)
			{
				trigger0 = true;
				param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
				param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 2);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("작업대가 잘 작동되는지 시험해봐야 하는데… 아까 편지도 그렇고, 마을에서 재료를 구한 뒤 테스트 해봐야겠다.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 7);
			}
		}
	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//가게 퇴장시
public class TutorialCondition4 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => 
		{
			AdvencedQuestData advencedQuestData = new AdvencedQuestData();
			advencedQuestData.questID = 1;
			advencedQuestData.questScript =
			"레딘에게\n" +
			"\n" +
			"도칸\n" +
			"\n" +
			"이전 세공사분꼐서 자리를 비운 뒤 가게를 운영하지 않는 것 같아 편지를 통해 남깁니다.\n" +
			"\n" +
			"혹시 편지를 읽게되신다면 저를 찾아와주세요.\n" +
			"\n" +
			"당신에게 드릴게 있습니다. 제가 없을 때를 대비하여 보석상인인 베일씨에게 물건을 맡겨뒀습니다.\n" +
			"\n" +
			"보상\n" +
			"\n" +
			"다이아몬드 x 1";
			Mailbox.main.AddQuest(advencedQuestData);
		}, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("마을에 들리기전에… 혹시 자리가 비어있는 동안 누군가 다녀갔으려나…", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
		
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//우편함 상호작용 끝났을 때
public class TutorialCondition5 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("우편에 적힌대로 마을에 들러볼까?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//마을에 입장했을 때
public class TutorialCondition6 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("여기도 정말 오랜만이야. 요즘 마을 분위기는 어떤지 게시판을 확인해볼까?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);

		Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
		if(canvas != null)
		{
			GameObject noticeBoardUI = UniFunc.GetChildOfName(canvas.gameObject, "NoticeBoardUI");
			if(noticeBoardUI != null)
			{
				GameObjectEventComponent gameObjectEventComponent = noticeBoardUI.AddComponent<GameObjectEventComponent>();
				gameObjectEventComponent.m_OnEnable.AddListener(() => 
				{
					TutorialManager.EventPublish(TutorialStates.N7);
					gameObjectEventComponent.m_OnEnable.RemoveAllListeners();
					UnityEngine.Object.Destroy(gameObjectEventComponent);
				});
			}
		}
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//게시판과 상호작용을 종료했을 때
public class TutorialCondition7 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 2);
		//param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("여기도 정말 오랜만이야. 요즘 마을 분위기는 어떤지 게시판을 확인해볼까?", true); }, 0);
		//param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}
