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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("먼길 오느라 잠깐 쉬었으니… 다시 출발해볼까?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("왼쪽 길로 가다 보면 마을에 곧 도착할거야", true); }, 4);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { UniFunc.GetChildOfName(PlayerCharacterUIScript.main.gameObject, "OptionPanel").SetActive(true); }, 7);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("왼쪽 기슭근처 숲 속에 있는 집에 가보자", true); }, 0);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 2);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, new Vector2(Screen.width * 0.55f, Screen.height * 0.5f), "청소하기", true); }, 2);
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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("장사를 시작하기 전에, 마을에서 재료를 구하자.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 6);
				param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); }, 3);
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
			"To. 레딘\n" +
			"\n" +
			"전에 계시던 세공사분꼐서 자리를 비운 뒤 가게를 운영하지 않는 것 같아 편지를 남깁니다.\n" +
			"당신에게 드릴게 있습니다. 혹시 편지를 읽게되신다면 저를 찾아와주세요.\n" +
			"\n" +
			"From. 도칸\n" +
			"\n" +
			"보상 : 액세서리 제작용 재료";
			Mailbox.main.AddQuest(advencedQuestData);
		}, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("마을에 들리기전에 우편함을 확인해보자. 혹시 누군가 다녀갔으려나…", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("편지에 적힌대로 마을에 들러볼까?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("여기도 정말 오랜만이야. 요즘 마을 분위기는 어떤지 게시판을 확인해보자.", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);

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
		//GameManager.Instance.GameTime.TimeStop(true);
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
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("어… 혹시 편지를 남긴사람인가요?", true); }, 7);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 9);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("맞아요. 제 편지를 읽으셨군요! 돌아오셔서 기뻐요!", true); },9);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("혹시 제게 볼 일이 있으셨나요?", true); }, 12);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 14);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("말하자면 조금 긴데… 다음에 찾아가서 차근차근 대화를 할 수 있을까요?", true); }, 14);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("알겠습니다. 마침 저도 오자마자 볼 일이 생겨서요. 그럼 주신다는 물건은 어디에 있나요?", true); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 20);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("앗차! 저기 왼쪽에 있는 곰 베일 아저씨께 맡겨뒀으니 제 이름을 말씀하시면 물건을 건네주실거에요!", true); }, 20);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("그럼 다음에 다시 뵈요!", true); }, 24);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 26);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 26);
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
			PlayerCharacter.main.PopUpSpeechBubble("", false);

			GameObject.Find("베일").transform.position = new Vector3(197.863678f, 15.0428715f, 264.626343f);
			GameObject.Find("베일").GetComponent<NPC>().destinationPos = new Vector3(197.863678f, 15.0428715f, 264.626343f);
			GameObject.Find("베일").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
			GameObject.Find("가가").transform.position = new Vector3(198.192886f, 15.0428715f, 248.688812f);
			GameObject.Find("가가").GetComponent<NPC>().destinationPos = new Vector3(198.192886f, 15.0428715f, 248.688812f);
			GameObject.Find("가가").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
		}, 27);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 29);
		//param.WaitFewSeconds(() => { GameManager.Instance.GameTime.TimeStop(false); }, 43);
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
		//param.WaitFewSeconds(() => { GameManager.Instance.GameTime.TimeStop(true); }, 0);
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
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 5);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("안녕하세요. 도칸님이 맡기신 물건을 찾으러왔습니다.", true); }, 5);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("아 당신이군!", true); }, 7);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("물건을 받고 몇달이 지나도 찾아오지 않아서 잊고 있었는데", true); }, 8);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 10);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("하하… 사정이 있었어요…", true); }, 10);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("자! 여기있네", true); }, 12);
		param.WaitFewSeconds(() => { Inventory.main.PopUpInventory(true); }, 12);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 13);
		param.WaitFewSeconds(() => 
		{
			if(Inventory.main != null)
			{
				Inventory inventory = Inventory.main;
				inventory.AddAItem(1, 1, 10);
				inventory.AddAItem(2, 1, 10);
				inventory.AddAItem(3, 1, 10);
				inventory.AddAItem(4, 1, 10);
				inventory.AddAItem(5, 1, 10);
				inventory.AddAItem(55, 1, 10);
				inventory.AddAItem(56, 1, 10);
				inventory.AddAItem(57, 1, 10);
				inventory.AddAItem(58, 1, 10);
				inventory.AddAItem(59, 1, 10);
				inventory.AddAItem(60, 1, 10);
			}
		}, 13);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("감사합니다.", true); }, 13);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 14);
		param.WaitFewSeconds(() => { Inventory.main.PopUpInventory(false); }, 14);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("그나저나 전에 계시던 보석세공사씨와 닮았는데, 혹시 가족인가?", true); }, 14);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("아 저희 할아버지셨습니다.", true); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 19);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("그렇군… 좋은 분이셨지…", true); }, 19);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 21);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("참, 마침 장사가 잘 되지 않아서 적자였는데", true); }, 22);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("내 오늘만 특별히 재료들을 싸게 팔아주지!", true); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 26);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 26);
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
			PlayerCharacter.main.PopUpSpeechBubble("", false);
		}, 27);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 29);
		param.WaitFewSeconds(() => { TutorialManager.EventPublish(TutorialStates.N9); }, 30);
		//param.WaitFewSeconds(() => { GameManager.Instance.GameTime.TimeStop(false); }, 54);

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
		GameObject.Find("베일").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
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
		PlayerCharacter.main.PopUpSpeechBubble("이제 가젤 상인에게 장신구를 구매해볼까?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		GameObject.Find("가가").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);

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
		param.wall7.SetActive(false);
		param.wall8.SetActive(false);
		PlayerCharacter.main.PopUpSpeechBubble("집으로 돌아가기 전에, 퓨퓨 동상 앞에서 기도를 한번 해보자.", true);
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

//퓨퓨숲 동상 앞에 갔을때
public class TutorialCondition13 : BaseState<TutorialManager>
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
				param.WaitFewSeconds(() => { param.coinThrowingEffect.Play(); }, 0);
				param.WaitFewSeconds(() => { param.coinDonationEffect.Play(); }, 2);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("기도를 했더니 무언가 보인다. 고급 장신구에 맞게 제작하면 될 것 같은 체인이 떨어져있다.", true); }, 3);
				param.WaitFewSeconds(() => { Inventory.main.AddAItem(1, 1, 1); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("이제 가게로 돌아가보자", true); }, 8);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 11);
				param.WaitFewSeconds(() => 
				{
					GameObject.Find("베일").GetComponent<NPC>().ChangeState(NPCBehaviour.Idle);
					GameObject.Find("가가").GetComponent<NPC>().ChangeState(NPCBehaviour.Idle);
					PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); 
				}, 11);
				//GameManager.Instance.GameTime.TimeStop(false);
				trigger0 = true;
			}
		}
	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//마을을 빠져나갈 때
public class TutorialCondition14 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.wall9.SetActive(true);
		//param.WaitFewSeconds(() => { GameManager.Instance.GameTime.TimeStop(true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual2.Priority = 1000;
			param.cador.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 2);
		param.WaitFewSeconds(() => { param.cador.PopUpSpeechBubble("찾았다…!", true); }, 4);
		param.WaitFewSeconds(() => { param.cador.PopUpSpeechBubble("", false); }, 6);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 6);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual2.Priority = 0;
			param.cador.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
		}, 7);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 8);
		//param.WaitFewSeconds(() => { GameManager.Instance.GameTime.TimeStop(false); }, 8);
	}
	public override void StateUpdate(TutorialManager param)
	{
		
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
		GameManager.Instance.GameTime.TimeStop(true);
		param.WaitFewSeconds(() =>
		{ 
			PlayerCharacterUIScript.main.PopupTutorialImage(0, true);
			PlayerCharacterUIScript.main.PopupTutorialText(0, true);
			GuideState = 1;
		}, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{
		if (Input.GetMouseButtonDown(0) == true || Input.anyKeyDown)
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

				UniFunc.GetChildOfName(PlayerCharacterUIScript.main.gameObject, "InventoryUI").SetActive(true);
				GameManager.Instance.GameTime.TimeStop(false);
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