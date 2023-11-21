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

//���� ���۽�
public class TutorialCondition0 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�ձ� ������ ��� �������ϡ� �ٽ� ����غ���?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���� ��� ���� ���� ������ �� �����Ұž�", true); }, 4);
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

//�ٸ� ���Խ�
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

//�ٸ� �����
public class TutorialCondition2 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���� �⽾��ó �� �ӿ� �ִ� ���� ������", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//���� �����
public class TutorialCondition3 : BaseState<TutorialManager>
{
	private bool trigger0 = false;
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("������ ���� �׿��ס�", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 2);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, new Vector2(Screen.width * 0.55f, Screen.height * 0.5f), "û���ϱ�", true); }, 2);
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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("��縦 �����ϱ� ����, �������� ��Ḧ ������.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 6);
				param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); }, 3);
			}
		}
	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//���� �����
public class TutorialCondition4 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => 
		{
			AdvencedQuestData advencedQuestData = new AdvencedQuestData();
			advencedQuestData.questID = 1;
			advencedQuestData.questScript =
			"To. ����\n" +
			"\n" +
			"���� ��ô� ������в��� �ڸ��� ��� �� ���Ը� ����� �ʴ� �� ���� ������ ����ϴ�.\n" +
			"��ſ��� �帱�� �ֽ��ϴ�. Ȥ�� ������ �аԵǽŴٸ� ���� ã�ƿ��ּ���.\n" +
			"\n" +
			"From. ��ĭ\n" +
			"\n" +
			"���� : �׼����� ���ۿ� ���";
			Mailbox.main.AddQuest(advencedQuestData);
		}, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("������ �鸮������ �������� Ȯ���غ���. Ȥ�� ������ �ٳబ��������", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//������ ��ȣ�ۿ� ������ ��
public class TutorialCondition5 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("������ ������� ������ �鷯����?", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//������ �������� ��
public class TutorialCondition6 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���⵵ ���� �������̾�. ���� ���� ������� ��� �Խ����� Ȯ���غ���.", true); }, 0);
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

//�Խ��ǰ� ��ȣ�ۿ��� �������� ��
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
			GameObject.Find("��ĭ").transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero, "", true);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("��� ������̽���? �ȳ��ϼ���! �� ��ĭ�̶�� �ؿ�. ���ƿ��̱���!", true); }, 4);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("� Ȥ�� ������ �������ΰ���?", true); }, 7);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 9);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("�¾ƿ�. �� ������ �����̱���! ���ƿ��ż� �⻵��!", true); },9);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("Ȥ�� ���� �� ���� �����̳���?", true); }, 12);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 14);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("�����ڸ� ���� �䵥�� ������ ã�ư��� �������� ��ȭ�� �� �� �������?", true); }, 14);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("�˰ڽ��ϴ�. ��ħ ���� ���ڸ��� �� ���� ���ܼ���. �׷� �ֽŴٴ� ������ ��� �ֳ���?", true); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 20);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("����! ���� ���ʿ� �ִ� �� ���� �������� �ðܵ����� �� �̸��� �����Ͻø� ������ �ǳ��ֽǰſ���!", true); }, 20);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("�׷� ������ �ٽ� �ƿ�!", true); }, 24);
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
			GameObject.Find("��ĭ").transform.GetChild(0).gameObject.SetActive(true);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero);
			PlayerCharacter.main.PopUpSpeechBubble("", false);

			GameObject.Find("����").transform.position = new Vector3(197.863678f, 15.0428715f, 264.626343f);
			GameObject.Find("����").GetComponent<NPC>().destinationPos = new Vector3(197.863678f, 15.0428715f, 264.626343f);
			GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
			GameObject.Find("����").transform.position = new Vector3(198.192886f, 15.0428715f, 248.688812f);
			GameObject.Find("����").GetComponent<NPC>().destinationPos = new Vector3(198.192886f, 15.0428715f, 248.688812f);
			GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
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

//���ϰ� ��ȭ�� �������� ��
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
			GameObject.Find("����").transform.GetChild(0).gameObject.SetActive(false);
			PlayerCharacter.main.PopUpInteractionIcon(false, Vector2.zero, "", true);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("����ʼ�!", true); }, 4);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 5);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�ȳ��ϼ���. ��ĭ���� �ñ�� ������ ã�����Խ��ϴ�.", true); }, 5);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 7);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�� ����̱�!", true); }, 7);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("������ �ް� ����� ������ ã�ƿ��� �ʾƼ� �ذ� �־��µ�", true); }, 8);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 10);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("���ϡ� ������ �־���䡦", true); }, 10);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("��! �����ֳ�", true); }, 12);
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
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�����մϴ�.", true); }, 13);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 14);
		param.WaitFewSeconds(() => { Inventory.main.PopUpInventory(false); }, 14);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�׳����� ���� ��ô� ���������羾�� ��Ҵµ�, Ȥ�� �����ΰ�?", true); }, 14);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�� ���� �Ҿƹ����̽��ϴ�.", true); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 19);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�׷����� ���� ���̼�����", true); }, 19);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 21);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("��, ��ħ ��簡 �� ���� �ʾƼ� ���ڿ��µ�", true); }, 22);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�� ���ø� Ư���� ������ �ΰ� �Ⱦ�����!", true); }, 24);
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
			GameObject.Find("����").transform.GetChild(0).gameObject.SetActive(true);
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

//���ϰ� ��ȭ�� ������ ��
public class TutorialCondition9 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);
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

//���ϰ� ��ȣ�ۿ��� �������� ��
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

//���ϰ� ��ȣ�ۿ��� �������� ��
public class TutorialCondition11 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		PlayerCharacter.main.PopUpSpeechBubble("���� ���� ���ο��� ��ű��� �����غ���?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 4);
		GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Business);

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

//������ ��ȣ�ۿ��� �������� ��
public class TutorialCondition12 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.wall7.SetActive(false);
		param.wall8.SetActive(false);
		PlayerCharacter.main.PopUpSpeechBubble("������ ���ư��� ����, ǻǻ ���� �տ��� �⵵�� �ѹ� �غ���.", true);
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

//ǻǻ�� ���� �տ� ������
public class TutorialCondition13 : BaseState<TutorialManager>
{
	private bool trigger0 = false;
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, new Vector2(Screen.width * 0.6f, Screen.height * 0.4f), "�⵵�ϱ�", true); }, 0);
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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�⵵�� �ߴ��� ���� ���δ�. ��� ��ű��� �°� �����ϸ� �� �� ���� ü���� �������ִ�.", true); }, 3);
				param.WaitFewSeconds(() => { Inventory.main.AddAItem(1, 1, 1); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���� ���Է� ���ư�����", true); }, 8);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 11);
				param.WaitFewSeconds(() => 
				{
					GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Idle);
					GameObject.Find("����").GetComponent<NPC>().ChangeState(NPCBehaviour.Idle);
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

//������ �������� ��
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
		param.WaitFewSeconds(() => { param.cador.PopUpSpeechBubble("ã�Ҵ١�!", true); }, 4);
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

//���Կ� ���� ��
public class TutorialCondition15 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�ǸŴ� �տ��� ��縦 �����غ���.", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//��縦 �������� ��
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

//���� ���̵��� ����� �Ϸ�Ǿ��� ��
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