using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Sequence : MonoBehaviour
{
    public List<Sequence> sequences = new List<Sequence>();

    Sequence finalSequence;
    Sequence sf;

    public void OnEnable()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    public void OnDisable()
    {
        
    }

    void Start()
    {
        DOTween.Init();
        transform.localScale = Vector3.one * 0.1f;

        gameObject.SetActive(false);
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        transform.localScale = Vector3.one * 0.2f;

        seq.Append(transform.DOScale(1.1f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
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
