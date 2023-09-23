using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Sequence : MonoBehaviour
{
    public List<Sequence> sequences = new List<Sequence>();

    Sequence finalSequence;
    void Start()
    {
        finalSequence = DOTween.Sequence().SetAutoKill(false).Pause()
        .Append(transform.DOMoveX(0, 3))
        .Append(transform.GetComponent<Renderer>().material.DOColor(Color.blue, 3))
        .Append(transform.DOShakeRotation(1));
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            finalSequence.Restart();
        }
    }
}

[System.Serializable]
public class MoveSequence
{
    public Vector3 targetPos;
    public float duration;

    public Sequence Move(Sequence sequence ,Transform transform)
    {
        return sequence.Append(transform.DOMove(targetPos, duration));
    }
}
