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
    [SerializeField]
    public Transform myTransform;

    public SkeletonAnimation SkAni => skAni;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        myTransform = transform.Find("Body");
        animator= GetComponentInChildren<Animator>();
        skAni = myTransform.GetComponent<SkeletonAnimation>();
    }

    public void SetCharacterData(CharacterData cd)
    {
        characterData = cd;
    }

    public void InitCharacter(string characterInfo)
    {
        //myTransform.GetComponent<MeshRenderer>().material = AddressableManager.LoadObject<Material>(characterInfo + "_Material");
        skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>(characterInfo + "_Skeleton");
        //skAni.Initialize(true);
    }

    public void SkeletonInitialize()
    {
        skAni.Initialize(true);
    }
}