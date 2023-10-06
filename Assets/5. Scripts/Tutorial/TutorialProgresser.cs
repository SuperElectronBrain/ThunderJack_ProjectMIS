using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialProgresser : MonoBehaviour
{
	public void ProgressTutorial()
	{
		CollisionComponent t_CollisionComponent = GetComponent<CollisionComponent>();
		if(t_CollisionComponent != null)
		{
			if(t_CollisionComponent.m_Colliders != null)
			{
				if(t_CollisionComponent.m_Colliders.Count > 0)
				{
					for(int i = 0; i < t_CollisionComponent.m_Colliders.Count; i = i + 1)
					{
						TutorialComponent t_TutorialComponent = t_CollisionComponent.m_Colliders[i].GetComponent<TutorialComponent>();
						if (t_TutorialComponent != null)
						{
							if(t_TutorialComponent.GetCurrentStateType() == StateType.MovePointToPoint)
							{
								t_TutorialComponent.ProgressTutorial();
							}
							break;
						}
					}
				}
			}
		}
	}
}
