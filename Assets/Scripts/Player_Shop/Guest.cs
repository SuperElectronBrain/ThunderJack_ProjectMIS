using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Guest : MonoBehaviour
{
    SkeletonAnimation skAni;
    float answerWatingDuration;
    float craftWaitingDuration;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
    }

    public void WaitingForAnswer()
    {
        StartCoroutine(Waiting(answerWatingDuration));
    }

    public void WaitingForCraft()
    {
        StartCoroutine(Waiting(craftWaitingDuration));
    }

    IEnumerator Waiting(float watingDuration)
    {
        yield return new WaitForSeconds(watingDuration);
    }

    public void ExitShop()
    {

    }
}
