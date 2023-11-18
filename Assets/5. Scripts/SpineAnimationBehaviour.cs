using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimationBehaviour : StateMachineBehaviour
{
	private AnimationClip clip;

	//private Spine.Unity.SkeletonAnimation skeletonAnimation;
	//private Spine.AnimationState spineAnimationState;
	private Spine.TrackEntry trackEntry;

	void Awake()
	{
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        base.OnStateEnter(animator, stateInfo, layerIndex);

		if (clip == null)
		{
			if (animator.GetCurrentAnimatorClipInfo(0) != null)
			{
				if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
				{
					clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
				}
			}
		}
		if (clip != null)
		{
			Spine.Unity.SkeletonAnimation skeletonAnimation = animator.GetComponent<Spine.Unity.SkeletonAnimation>();
			if(skeletonAnimation == null)
			{ skeletonAnimation = animator.GetComponentInChildren<Spine.Unity.SkeletonAnimation>(); }
			if (skeletonAnimation != null)
			{ trackEntry = skeletonAnimation.state.SetAnimation(layerIndex, clip.name, stateInfo.loop); }

			if(skeletonAnimation == null)
			{
				Spine.Unity.SkeletonGraphic skeletonGraphic = animator.GetComponent<Spine.Unity.SkeletonGraphic>();
				if (skeletonGraphic == null)
				{ skeletonGraphic = animator.GetComponentInChildren<Spine.Unity.SkeletonGraphic>(); }
				if (skeletonGraphic != null)
				{ trackEntry = skeletonGraphic.AnimationState.SetAnimation(layerIndex, clip.name, stateInfo.loop); }
			}
			
			if (trackEntry != null)
			{ trackEntry.TimeScale = stateInfo.speed; }
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);

		if (trackEntry != null)
		{
			trackEntry.TimeScale = stateInfo.speedMultiplier;
			//trackEntry.TrackTime = stateInfo.normalizedTime * trackEntry.TrackComplete;
		}
	}

	//public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	base.OnStateMove(animator, stateInfo, layerIndex);
	//
	//	if (trackEntry != null)
	//	{
	//		trackEntry.TimeScale = stateInfo.speed;
	//		//trackEntry.TrackTime = stateInfo.normalizedTime * trackEntry.TrackComplete;
	//	}
	//}
}
