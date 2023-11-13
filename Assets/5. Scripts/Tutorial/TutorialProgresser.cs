using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialProgresser : MonoBehaviour
{
	[SerializeField]TutorialStates tutorialStates = TutorialStates.N0;
	public void ProgressTutorial()
	{
		TutorialManager.EventPublish(tutorialStates);
		Destroy(this);
	}

	//public void ProgressTutorial()
	//{
	//	CollisionComponent t_CollisionComponent = GetComponent<CollisionComponent>();
	//	if(t_CollisionComponent != null)
	//	{
	//		if(t_CollisionComponent.m_Colliders != null)
	//		{
	//			if(t_CollisionComponent.m_Colliders.Count > 0)
	//			{
	//				for(int i = 0; i < t_CollisionComponent.m_Colliders.Count; i = i + 1)
	//				{
	//					Collider t_Collider = t_CollisionComponent.m_Colliders[i];
	//					if (t_Collider != null)
	//					{
	//						TutorialComponent t_TutorialComponent = t_Collider.gameObject.GetComponent<TutorialComponent>();
	//						if (t_TutorialComponent != null)
	//						{
	//							if (t_TutorialComponent.GetCurrentStateType() == StateType.MovePointToPoint || t_TutorialComponent.GetCurrentStateType() == StateType.WaitingToEnterArea)
	//							{
	//								t_TutorialComponent.ProgressTutorial();
	//							}
	//							break;
	//						}
	//					}
	//				}
	//			}
	//		}
	//	}
	//}
}
