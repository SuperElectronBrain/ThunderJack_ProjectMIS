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
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("�۾��밡 �� �۵��Ǵ��� �����غ��� �ϴµ��� �Ʊ� ������ �׷���, �������� ��Ḧ ���� �� �׽�Ʈ �غ��߰ڴ�.", true); }, 3);
				param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 7);
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
		param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���� ������� ������ �鷯����?", true); }, 0);
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

//�Խ��ǰ� ��ȣ�ۿ��� �������� ��
public class TutorialCondition7 : BaseState<TutorialManager>
{
	public override void StateBegin(TutorialManager param)
	{
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeOut(1); }, 0);
		param.WaitFewSeconds(() => { PlayerCharacter.main.FadeIn(1); }, 2);
		//param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("���⵵ ���� �������̾�. ���� ���� ������� ��� �Խ����� Ȯ���غ���?", true); }, 0);
		//param.WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, 5);
	}
	public override void StateUpdate(TutorialManager param)
	{

	}
	public override void StateEnd(TutorialManager param)
	{

	}
}
