using Cinemachine;
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
		GameManager.Instance.GameTime.enabled = false;
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("먼지가 가득 쌓였네…", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, new Vector2(Screen.width * 0.6f, Screen.height * 0.4f), "청소하기", true); }, 3);
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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero, ""); }, 2);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("작업대가 잘 작동되는지 시험해봐야 하는데… 아까 편지도 그렇고, 마을에서 재료를 구한 뒤 테스트 해봐야겠다.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 7);
				param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); }, 7);
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
				gameObjectEventComponent.m_OnDisable.AddListener(() => 
				{
					TutorialManager.EventPublish(TutorialStates.N7);
					gameObjectEventComponent.m_OnDisable.RemoveAllListeners();
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
		param.WaitFewSeconds(() => 
		{
			param.cinemachineVirtual.Priority = 1000;
			param.redin.gameObject.SetActive(true);
			param.dokan.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
			PlayerCharacter.main.transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.transform.GetChild(2).gameObject.SetActive(false);
			GameObject.Find("도칸").transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero, "", true);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("어라 레딘님이시죠? 안녕하세요! 전 도칸이라고 해요. 돌아오셨군요!", true); }, 4);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 9);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("어… 혹시 편지를 남긴사람인가요?", true); }, 9);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("네! 맞아요. 제 편지를 읽으셨군요! 돌아오셔서 기뻐요!", true); },12);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("혹시 제게 무슨 용무라도 있으셨던건가요?", true); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 21);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("앗 말씀드리자면 조금 긴데… 혹시 다음에 찾아가서 차근차근 대화를 나눠볼 수 있을까요?", true); }, 21);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 26);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("아… 알겠습니다. 마침 저도 오자마자 손볼일이 있어서요, 그럼 주신다는 물건은 어디에…", true); }, 26);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 31);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("앗차! 아마 저기 왼쪽에 보이는 베일아저씨께 맡겨뒀으니 제 이름을 말씀하시면 맡겨두었던 물건을 건네주실거에요!", true); }, 31);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 36);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("그럼 다음에 뵐게요!", true); }, 36);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 39);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 39);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual.Priority = 0;
			param.dokan.gameObject.SetActive(false);
			param.redin.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
			PlayerCharacter.main.transform.GetChild(0).gameObject.SetActive(true);
			PlayerCharacter.main.transform.GetChild(2).gameObject.SetActive(true);
			GameObject.Find("도칸").transform.GetChild(0).gameObject.SetActive(true);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero);
		}, 40);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 42);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//베일과 대화를 시작했을 때
public class TutorialCondition8 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual1.Priority = 1000;
			param.redin1.gameObject.SetActive(true);
			param.beil.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
			PlayerCharacter.main.transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.transform.GetChild(2).gameObject.SetActive(false);
			GameObject.Find("베일").transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero, "", true);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("어서오십쇼!", true); }, 4);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 6);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("안녕하세요. 도칸님께서 맡기신 물건을 찾으러왔는데요.", true); }, 6);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 10);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("아 당신이군!", true); }, 10);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("물건을 맡기고는 몇달이 지나도 찾아오지 않아서 까먹고 있었는데", true); }, 12);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("하하… 그러한 사정이 있었죠…", true); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 20);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("자! 여기있네", true); }, 20);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 22);
		param.WaitFewSeconds(() => { Inventory.main.AddAItem(1, 1, 1); }, 22);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("감사합니다.", true); }, 22);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("그나저나 이전 보석세공사씨와 닮았는데 혹시 가족인가?", true); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 29);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("아 저희 할아버지셨습니다.", true); }, 29);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 32);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("그렇군… 좋은 분이셨지…", true); }, 32);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 35);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("마침 잘됐군! 안그래도 장사가 잘 되지 않아서 적자였는데!", true); }, 38);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 43);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("최근에 손님이 거의 오질않아 재고를 채워놓지 않기도 했고,내 오늘만 특별히 보석들을 2개까지만 10G에 받도록 하지", true); }, 43);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 50);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); },50);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual1.Priority = 0;
			param.beil.gameObject.SetActive(false);
			param.redin1.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
			PlayerCharacter.main.transform.GetChild(0).gameObject.SetActive(true);
			PlayerCharacter.main.transform.GetChild(2).gameObject.SetActive(true);
			GameObject.Find("베일").transform.GetChild(0).gameObject.SetActive(true);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero);
		}, 51);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 53);
		param.WaitFewSeconds(() => { TutorialManager.EventPublish(TutorialStates.N9); }, 54);
		
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//베일과 대화가 끝났을 때
public class TutorialCondition9 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.wall7.SetActive(true);
		param.wall8.SetActive(true);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//베일과 상호작용을 시작했을 때
public class TutorialCondition10 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.wall7.SetActive(false);
		param.wall8.SetActive(false);

		Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			GameObject NPCStoreUI = UniFunc.GetChildOfName(canvas.gameObject, "NPCStoreUI");
			if (NPCStoreUI != null)
			{
				GameObjectEventComponent gameObjectEventComponent = NPCStoreUI.AddComponent<GameObjectEventComponent>();
				gameObjectEventComponent.m_OnDisable.AddListener(() =>
				{
					TutorialManager.EventPublish(TutorialStates.N11);
					gameObjectEventComponent.m_OnDisable.RemoveAllListeners();
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

//베일과 상호작용을 종료했을 때
public class TutorialCondition11 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		PlayerCharacter.main.PopUpSpeechBubble("이제 장신구를 구매해볼까?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);

		param.WaitFewSeconds(() => {
			Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
			if (canvas != null)
			{
				GameObject NPCStoreUI = UniFunc.GetChildOfName(canvas.gameObject, "NPCStoreUI");
				if (NPCStoreUI != null)
				{
					GameObjectEventComponent gameObjectEventComponent = NPCStoreUI.AddComponent<GameObjectEventComponent>();
					gameObjectEventComponent.m_OnDisable.AddListener(() =>
					{
						TutorialManager.EventPublish(TutorialStates.N12);
						gameObjectEventComponent.m_OnDisable.RemoveAllListeners();
						UnityEngine.Object.Destroy(gameObjectEventComponent);
					});
				}
			}
		}, 2);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//가가와 상호작용을 종료했을 때
public class TutorialCondition12 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		PlayerCharacter.main.PopUpSpeechBubble("가게로 돌아가기 전 퓨퓨숲에 있는 동상 앞에서 기도를 한번 해볼까?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
		param.Trigger12.SetActive(true);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//마을을 빠져나갈 때
public class TutorialCondition13 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.wall9.SetActive(true);
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = false; }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual2.Priority = 1000;
			param.cador.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 2);
		param.WaitFewSeconds(() => { param.cador.PopUpSpeechBubble("찾았다…!", true); }, 4);
		param.WaitFewSeconds(() => { param.cador.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 7);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual2.Priority = 0;
			param.cador.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
		}, 8);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 9);
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = true; }, 8);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("퓨퓨숲은 오른쪽 폭포 강 기슭에 있었을거야.", true); }, 8);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 11);
		param.WaitFewSeconds(() => { TutorialManager.EventPublish(TutorialStates.N14); }, 11);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//퓨퓨숲 동상 앞에 갔을때
public class TutorialCondition14 : BaseState<TutorialManager>
{
	private bool trigger0 = false;
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, new Vector2(Screen.width * 0.6f, Screen.height * 0.4f), "기도하기", true); }, 0);
	}
	public override void StateUpdate(TutorialManager param)
	{
		if (Input.GetKeyDown(KeyCode.E) == true)
		{
			if (trigger0 == false)
			{
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero); }, 0);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("기도를 했더니 무언가 보인다. 고급 장신구에 맞게 제작하면 될 것 같은 체인이 떨어져있다.", true); }, 0);
				param.WaitFewSeconds(() => { Inventory.main.AddAItem(1, 1, 1); }, 0);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("이제 가게로 돌아가보자", true); }, 5);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 8);
				param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); }, 8);
				trigger0 = true;
			}
		}
	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//가게에 들어갔을 때
public class TutorialCondition15 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("판매대 앞에서 장사를 시작해보자.", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//장사를 시작했을 때
public class TutorialCondition16 : BaseState<TutorialManager>
{
	int GuideState = 0;
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() =>
		{ 
			PlayerCharacterUIScript.main.PopupTutorialImage(0, true);
			PlayerCharacterUIScript.main.PopupTutorialText(0, true);
			GuideState = 1;
		}, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{
		if (Input.GetMouseButtonDown(0) == true)
		{
			if(GuideState == 1)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(0, false);
				PlayerCharacterUIScript.main.PopupTutorialText(0, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(1, true);
				PlayerCharacterUIScript.main.PopupTutorialText(1, true);
				GuideState = 2;
			}
			else if (GuideState == 2)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(1, false);
				PlayerCharacterUIScript.main.PopupTutorialText(1, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(2, true);
				PlayerCharacterUIScript.main.PopupTutorialText(2, true);
				GuideState = 3;
			}
			else if (GuideState == 3)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(2, false);
				PlayerCharacterUIScript.main.PopupTutorialText(2, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(3, true);
				PlayerCharacterUIScript.main.PopupTutorialText(3, true);
				GuideState = 4;
			}
			else if (GuideState == 4)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(3, false);
				PlayerCharacterUIScript.main.PopupTutorialText(3, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(4, true);
				PlayerCharacterUIScript.main.PopupTutorialText(4, true);
				GuideState = 5;
			}
			else if (GuideState == 5)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(4, false);
				PlayerCharacterUIScript.main.PopupTutorialText(4, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(5, true);
				PlayerCharacterUIScript.main.PopupTutorialText(5, true);
				GuideState = 6;
			}
			else if (GuideState == 6)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(5, false);
				PlayerCharacterUIScript.main.PopupTutorialText(5, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(6, true);
				PlayerCharacterUIScript.main.PopupTutorialText(6, true);
				GuideState = 7;
			}
			else if (GuideState == 7)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(6, false);
				PlayerCharacterUIScript.main.PopupTutorialText(6, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(7, true);
				PlayerCharacterUIScript.main.PopupTutorialText(7, true);
				GuideState = 8;
			}
			else if (GuideState == 8)
			{
				PlayerCharacterUIScript.main.PopupTutorialText(7, false);

				PlayerCharacterUIScript.main.PopupTutorialText(8, true);
				GuideState = 9;
			}
			else if (GuideState == 9)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(7, false);
				PlayerCharacterUIScript.main.PopupTutorialText(8, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(8, true);
				PlayerCharacterUIScript.main.PopupTutorialText(9, true);
				GuideState = 10;
			}
			else if (GuideState == 10)
			{
				PlayerCharacterUIScript.main.PopupTutorialText(9, false);

				PlayerCharacterUIScript.main.PopupTutorialText(10, true);
				GuideState = 11;
			}
			else if (GuideState == 11)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(8, false);
				PlayerCharacterUIScript.main.PopupTutorialText(10, false);

				PlayerCharacterUIScript.main.PopupTutorialImage(9, true);
				PlayerCharacterUIScript.main.PopupTutorialText(11, true);
				GuideState = 12;
			}
			else if (GuideState == 12)
			{
				PlayerCharacterUIScript.main.PopupTutorialImage(9, false);
				PlayerCharacterUIScript.main.PopupTutorialText(11, false);
				GuideState = 13;

				GameManager.Instance.GameTime.enabled = true;
				TutorialManager.EventPublish(TutorialStates.N17);
			}
		}
	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//제작 가이드의 출력이 완료되었을 때
public class TutorialCondition17 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

public class TutorialCondition18 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{

	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}
public class TutorialCondition19 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{

	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

public class EndOfTutorial : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		GameManager.Instance.GameTime.enabled = true;
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}