using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class FadeIO : MonoBehaviour
{
    UnityEngine.UI.Image image;
    Sequence sequence;

    [SerializeField]
    bool isFadeOut;    
    [SerializeField]
    float duration;

    [SerializeField]
    float idleDuration;

    [SerializeField]
    float rewindDuration;

    [Header("Option")]
    [SerializeField]
    bool isEnable;
    

    [Header("Event")]
    public UnityEvent onFadeEvent;

    private void OnEnable()
    {
        if(isEnable)
            sequence.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();

        sequence = DOTween.Sequence().Pause().SetAutoKill(false);

        if (isFadeOut)
            sequence.Append(image.DOFade(1, duration));
        else
            sequence.Append(image.DOFade(0, duration));

            sequence.AppendInterval(idleDuration);

        if(rewindDuration > 0)
        {
            if (!isFadeOut)
                sequence.Append(image.DOFade(1, rewindDuration));
            else
                sequence.Append(image.DOFade(0, rewindDuration));
        }


        sequence.OnComplete(() =>
        {
            onFadeEvent?.Invoke();
        });        
    }

    public void ChangeImage(Sprite newSprite)
    {
        image.sprite = newSprite;
    }

    public void PlayFadeIO()
    {
        sequence.Play();
    }   
}
