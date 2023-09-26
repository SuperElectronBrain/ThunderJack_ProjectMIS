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
    Location curLocation;
    [SerializeField]
    string dialogueName;
    //CharacterDialogue characterDialogue;
    [SerializeField]
    protected CharacterData characterData;

    SkeletonAnimation skAni;
    [SerializeField]
    public Transform myTransform;

    public SkeletonAnimation SkAni { get { return skAni; } }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator= GetComponentInChildren<Animator>();
        skAni = GetComponentInChildren<SkeletonAnimation>();

        myTransform = transform.Find("Body");
        Debug.Log(gameObject.name + " start");
    }

    // Update is called once per frame
    void Update()
    {

    }

/*    public void PlayAnimation(AnimationType newAnimationType)
    {
        animator.Play(newAnimationType.ToString());
    }

    public void PlayAnimation(int newAnimationType)
    {
        animator.Play(((AnimationType)newAnimationType).ToString());
    }*/

    public void SetLocation(Location newLocation)
    {
        curLocation = newLocation;
    }

    public string GetDialogue()
    {
        return dialogueName;
    }

    public Location GetLocation()
    {
        return curLocation;
    }

    public void SetCharacterData(CharacterData cd)
    {
        characterData = cd;
    }

    public void InitCharacter(string characterInfo)
    {
        Debug.Log(gameObject.name + "Init");
        //myTransform.GetComponent<MeshRenderer>().material = AddressableManager.LoadObject<Material>(characterInfo + "_Material");
        skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>(characterInfo + "_Skeleton");
        //skAni.Initialize(true);
    }    
}
