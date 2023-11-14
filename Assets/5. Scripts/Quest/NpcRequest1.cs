using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRequest_1 : NpcRequest
{
    private RequestUI requestUI;
    protected override void InitNpcRequest()
    {
        EventManager.Subscribe(EventType.EndConversation, UpdateRequestProgress);
    }

    protected override void ShowNpcRequest()
    {
        var charDB = GameManager.Instance.CharacterDB;
        var uiTransform = GameManager.Instance.NpcRequestManager.requestUiParent;
        var npcCount = charDB.GetCharacterCount();
        var requestObj = AddressableManager.LoadObject<GameObject>("RequestPrefab");

        requestUI = Instantiate(requestObj, uiTransform).GetComponent<RequestUI>();

        requestUI.SetRequestTitle(npcRequestData.requestName, npcRequestData.requestDescription);
        
        for (int i = 1; i <= npcCount; i++)
        {
            requestUI.AddRequestItem(charDB.GetCharacterName(i));
        }
    }

    void UpdateRequestProgress()
    {
        var charDB = GameManager.Instance.CharacterDB;
        var npcCount = charDB.GetCharacterCount();
        var progress = 0;
        
        for (int i = 1; i <= npcCount; i++)
        {
            if (charDB.GetNPC(i).isAcquaintance)
            {
                requestUI.CompleteRequestItem(i - 1);
                progress++;
            }
        }

        if (progress == npcCount)
        {
            requestUI.CompleteRequest();
        }
    }
}