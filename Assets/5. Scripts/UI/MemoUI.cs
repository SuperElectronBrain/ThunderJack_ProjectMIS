using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoUI : UiComponent
{
    [SerializeField] private TextMeshProUGUI memoText;
    
    public void SetMemo(string memo)
    {
        memoText.text = memo;
    }
    
    public override void InactiveUI()
    {
        base.InactiveUI();
        
        if(sequence != null)
            sequence.PlayBackwards();
    }
}
