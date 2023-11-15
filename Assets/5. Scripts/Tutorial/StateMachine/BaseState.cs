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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�ձ� ������ ��� �������ϡ� �ٽ� ����غ��� �� �����Ұž�", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���� �⽾��ó �� �ӿ� �ִ� ���Կ� ������", true); }, 0);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(true, "û���ϱ�", true); }, 3);
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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpInteractionIcon(false, ""); }, 2);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�۾��밡 �� �۵��Ǵ��� �����غ��� �ϴµ��� �Ʊ� ������ �׷���, �������� ��Ḧ ���� �� �׽�Ʈ �غ��߰ڴ�.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 7);
				param.WaitFewSeconds(() => { PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable); }, 7);
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
			"���򿡰�\n" +
			"\n" +
			"��ĭ\n" +
			"\n" +
			"���� ������в��� �ڸ��� ��� �� ���Ը� ����� �ʴ� �� ���� ������ ���� ����ϴ�.\n" +
			"\n" +
			"Ȥ�� ������ �аԵǽŴٸ� ���� ã�ƿ��ּ���.\n" +
			"\n" +
			"��ſ��� �帱�� �ֽ��ϴ�. ���� ���� ���� ����Ͽ� ���������� ���Ͼ����� ������ �ðܵ׽��ϴ�.\n" +
			"\n" +
			"����\n" +
			"\n" +
			"���̾Ƹ�� x 1";
			Mailbox.main.AddQuest(advencedQuestData);
		}, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("������ �鸮�������� Ȥ�� �ڸ��� ����ִ� ���� ������ �ٳబ��������", true); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
		
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���⵵ ���� �������̾�. ���� ���� ������� ��� �Խ����� Ȯ���غ���?", true); }, 0);
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

//�Խ��ǰ� ��ȣ�ۿ��� �������� ��
public class TutorialCondition7 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = false; }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() => 
		{
			param.cinemachineVirtual.Priority = 1000;
			param.redin.gameObject.SetActive(true);
			param.dokan.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("��� ������̽���? �ȳ��ϼ���! �� ��ĭ�̶�� �ؿ�. ���ƿ��̱���!", true); }, 4);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 9);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("� Ȥ�� ������ �������ΰ���?", true); }, 9);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 12);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("��! �¾ƿ�. �� ������ �����̱���! ���ƿ��ż� �⻵��!", true); },12);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("Ȥ�� ���� ���� �빫�� �����̴��ǰ���?", true); }, 17);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 21);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("�� �����帮�ڸ� ���� �䵥�� Ȥ�� ������ ã�ư��� �������� ��ȭ�� ������ �� �������?", true); }, 21);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 26);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("�ơ� �˰ڽ��ϴ�. ��ħ ���� ���ڸ��� �պ����� �־��, �׷� �ֽŴٴ� ������ ��𿡡�", true); }, 26);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 31);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("����! �Ƹ� ���� ���ʿ� ���̴� ���Ͼ������� �ðܵ����� �� �̸��� �����Ͻø� �ðܵξ��� ������ �ǳ��ֽǰſ���!", true); }, 31);
		param.WaitFewSeconds(() => { param.dokan.PopUpSpeechBubble("", false); }, 36);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("�׷� ������ �ɰԿ�!", true); }, 36);
		param.WaitFewSeconds(() => { param.redin.PopUpSpeechBubble("", false); }, 39);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 39);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual.Priority = 0;
			param.dokan.gameObject.SetActive(false);
			param.redin.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
		}, 40);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 42);
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = true; }, 43);

	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}

//���ϰ� ��ȣ�ۿ��� �������� ��
public class TutorialCondition8 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = false; }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual.Priority = 1000;
			param.redin1.gameObject.SetActive(true);
			param.beil.gameObject.SetActive(true);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Communication);
		}, 1);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 3);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("����ʼ�!", true); }, 4);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 6);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�ȳ��ϼ���. ��ĭ�Բ��� �ñ�� ������ ã�����Դµ���.", true); }, 6);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 10);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�� ����̱�!", true); }, 10);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("������ �ñ���� ����� ������ ã�ƿ��� �ʾƼ� ��԰� �־��µ�", true); }, 12);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("���ϡ� �׷��� ������ �־��ҡ�", true); }, 17);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 20);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("��! �����ֳ�", true); }, 20);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 22);
		param.WaitFewSeconds(() => { Inventory.main.AddAItem(1, 1, 1); }, 22);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�����մϴ�.", true); }, 22);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�׳����� ���� ���������羾�� ��Ҵµ� Ȥ�� �����ΰ�?", true); }, 24);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 29);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("�� ���� �Ҿƹ����̽��ϴ�.", true); }, 29);
		param.WaitFewSeconds(() => { param.redin1.PopUpSpeechBubble("", false); }, 32);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�׷����� ���� ���̼�����", true); }, 32);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 35);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("��ħ �ߵƱ�! �ȱ׷��� ��簡 �� ���� �ʾƼ� ���ڿ��µ�!", true); }, 38);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 43);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("�ֱٿ� �մ��� ���� �����ʾ� ����� ä������ �ʱ⵵ �߰�,�� ���ø� Ư���� �������� 2�������� 10G�� �޵��� ����", true); }, 43);
		param.WaitFewSeconds(() => { param.beil.PopUpSpeechBubble("", false); }, 50);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); },50);
		param.WaitFewSeconds(() =>
		{
			param.cinemachineVirtual.Priority = 0;
			param.beil.gameObject.SetActive(false);
			param.redin1.gameObject.SetActive(false);
			PlayerCharacter.main.ChangeState(PlayerCharacterState.Moveable);
		}, 51);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 53);
		param.WaitFewSeconds(() => { GameManager.Instance.GameTime.enabled = true; }, 54);
		param.WaitFewSeconds(() => { TutorialManager.EventPublish(TutorialStates.N9); }, 54);
		
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

//���ϰ� ��ȣ�ۿ��� �������� ��
public class TutorialCondition11 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		PlayerCharacter.main.PopUpSpeechBubble("���� ��ű��� �����غ���?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 3);
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
		PlayerCharacter.main.PopUpSpeechBubble("���Է� ���ư��� �� ǻǻ���� �ִ� ���� �տ��� �⵵�� �ѹ� �غ���?", true);
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}
public class TutorialCondition13 : BaseState<TutorialManager>
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
public class TutorialCondition14 : BaseState<TutorialManager>
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