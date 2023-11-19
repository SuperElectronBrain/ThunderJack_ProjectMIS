using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum AnimationType
{
    Default, Smile, Angry, Sad, Last
}

/*[System.Serializable]
public class CharacterDialogue
{
    public TextAsset characterDialogue;
}*/

public class Character : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    string dialogueName;
    //CharacterDialogue characterDialogue;
    [SerializeField]
    protected CharacterData characterData;

    SkeletonAnimation skAni;
    private SkeletonAnimation shadowSkAni;
    [SerializeField]
    public Transform myTransform;

    public SkeletonAnimation SkAni => skAni;
    public SkeletonAnimation ShadowSkAni => shadowSkAni;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        myTransform = transform.Find("Body");
        animator= GetComponentInChildren<Animator>();
        skAni = myTransform.GetComponent<SkeletonAnimation>();
        shadowSkAni = transform.Find("Shadow").GetComponentInChildren<SkeletonAnimation>();
    }

    public void SetCharacterData(CharacterData cd)
    {
        characterData = cd;
    }

    public void InitCharacter(string characterInfo)
    {
        //myTransform.GetComponent<MeshRenderer>().material = AddressableManager.LoadObject<Material>(characterInfo + "_Material");
        skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>(characterInfo + "_Skeleton");
        shadowSkAni.skeletonDataAsset = skAni.skeletonDataAsset;
        //skAni.Initialize(true);
    }

    public void SkeletonInitialize()
    {
        skAni.Initialize(true);
        shadowSkAni.Initialize(true);
    }
}