using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Conversation()
    {
        //GameManager.Instance.StartConversation(dialogueName, this);
    }

    public void PlayAnimation(AnimationType newAnimationType)
    {
        animator.Play(newAnimationType.ToString());
    }

    public void PlayAnimation(int newAnimationType)
    {
        //animator.Play(((AnimationType)newAnimationType).ToString());
    }

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
}