using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimationBehaviour : StateMachineBehaviour
{
	[SerializeField] private AnimationClip clip;

	//private Spine.Unity.SkeletonAnimation skeletonAnimation;
	//private Spine.AnimationState spineAnimationState;
	private Spine.TrackEntry trackEntry;

	void Awake()
	{
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if(clip != null)
        {
            trackEntry = animator.GetComponentInChildren<Spine.Unity.SkeletonAnimation>().state.SetAnimation(layerIndex, clip.name, stateInfo.loop);
			trackEntry.TimeScale = stateInfo.speed;
		}
	}
}
