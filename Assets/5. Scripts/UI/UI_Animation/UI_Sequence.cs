using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public enum TweenMode
{
    DoMove,
    DoRotate,
    DoScale,
    DoInterval,
    DoInActive
}

[Serializable]
public class TweenSequenceElement
{
    public TweenMode Mode;
    public Transform Target;
    public Ease Ease = Ease.Linear;
    public Vector3 TweenTarget;
    public float Duration = 1;
}

[Serializable]
public class TweenSequence
{
    public List<TweenSequenceElement> Elements;
}

public class UI_Sequence : MonoBehaviour
{
    public bool PlayOnAwake;
    public bool IgnoreTimeScale;
    public bool IsLoop;
    public bool isEneble;
    public bool isDisable;
    public bool isRewind;

    public float SequenceLength { get; private set; }

    [field: SerializeField] public List<TweenSequence> Sequences { get; private set; } = new();

    private Sequence _sequence;

    [Header("Event")]
    public UnityEvent onComplateEvents;

    private void Awake()
    {
        if (PlayOnAwake)
        {
            ReStart();
            if(isRewind)
            {
                _sequence.OnComplete(() =>
                {
                    PlayBackwards();
                });
            }
        }
    }

    public void OnEnable()
    {
        if(isEneble)
        {
            ReStart();
            if (isRewind)
            {
                _sequence.OnComplete(() =>
                {
                    PlayBackwards();
                });
            }
        }
    }

    private void OnDisable()
    {
        if(isDisable)
        {
            ReStart();
        }
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }

    public void ReStart()
    {
        if (_sequence == null)
        {
            BakeSeqence();
        }

        _sequence.Restart();
        _sequence.OnComplete(() =>
        {
            onComplateEvents?.Invoke();
        });
    }

    public void PlayBackwards()
    {
        if (_sequence == null)
        {
            BakeSeqence();
            _sequence.Rewind();
        }

        _sequence.PlayBackwards();
        _sequence.OnRewind(() =>
        {
            gameObject.SetActive(false);
            Debug.Log("Rewind");
        });
    }

    public void BakeSeqence()
    {
        _sequence = DOTween.Sequence().SetUpdate(IgnoreTimeScale).SetRecyclable(true).SetAutoKill(false).Pause();
        SequenceLength = 0;

        if (IsLoop)
        {
            _sequence.SetLoops(-1);
        }

        foreach (var sequence in Sequences)
        {
            var seq = DOTween.Sequence();
            float duration = 0;

            foreach (var elements in sequence.Elements)
            {
                switch (elements.Mode)
                {
                    case TweenMode.DoMove:
                        if (elements.Target is RectTransform rectTransform)
                        {
                            seq.Join(rectTransform.DOAnchorPos(elements.TweenTarget, elements.Duration)
                                .SetEase(elements.Ease));
                        }
                        else
                        {
                            seq.Join(elements.Target.DOLocalMove(elements.TweenTarget, elements.Duration)
                                .SetEase(elements.Ease));
                        }
                        break;

                    case TweenMode.DoRotate:
                        seq.Join(elements.Target.DOLocalRotate(elements.TweenTarget, elements.Duration)
                            .SetEase(elements.Ease));
                        break;

                    case TweenMode.DoScale:
                        seq.Join(elements.Target.DOScale(elements.TweenTarget, elements.Duration)
                            .SetEase(elements.Ease));
                        break;
                    case TweenMode.DoInterval:
                        seq.AppendInterval(elements.Duration);
                        break;
                    case TweenMode.DoInActive:
                        seq.OnComplete(() =>
                        {
                            elements.Target.gameObject.SetActive(false);
                        });
                        break;
                }

                if (elements.Duration > duration)
                {
                    duration = elements.Duration;
                }
            }

            _sequence.Append(seq);
            SequenceLength += duration;
        }
    }
}
