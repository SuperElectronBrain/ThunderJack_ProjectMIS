using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class FadeIO : MonoBehaviour
{
    Image image;
    SpriteRenderer sr;
    Sequence sequence;
    private Sequence rewindSequence;

    [SerializeField]
    bool isImage;

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
    [SerializeField]
    bool isIncludeChild;

    [SerializeField] private bool isRewind;
    [SerializeField]
    bool isDestory;

    [SerializeField]
    List<Image> childImage = new();
    [SerializeField]
    List<TextMeshProUGUI> childText = new();

    [SerializeField] private List<SpriteRenderer> childSr = new();
    [SerializeField] private List<TextMeshPro> childTmp = new();

    [Header("Event")]
    public UnityEvent onFadeEvent;

    private void OnEnable()
    {
        if(isEnable)
            sequence.Restart();
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (isImage)
            Image();
        else
            Sprite();
    }

    void Image()
    {
        image = GetComponent<UnityEngine.UI.Image>();

        sequence = DOTween.Sequence().Pause().SetAutoKill(false);

        if (isFadeOut)
            sequence.Append(image.DOFade(1, duration));
        else
            sequence.Append(image.DOFade(0, duration));

        if (isRewind)
        {
            rewindSequence= DOTween.Sequence().Pause().SetAutoKill(false);
            
            if (!isFadeOut)
                rewindSequence.Append(image.DOFade(1, rewindDuration));
            else
                rewindSequence.Append(image.DOFade(0, rewindDuration));
            
            if (isIncludeChild)
            {
                rewindSequence.OnUpdate(() =>
                {
                    for (int i = 0; i < childText.Count; i++)
                    {
                        Color c = childText[i].color;
                        c.a = image.color.a;
                        childText[i].color = c;
                    }

                    for (int i = 0; i < childImage.Count; i++)
                    {
                        Color c = childImage[i].color;
                        c.a = image.color.a;
                        childImage[i].color = c;
                    }
                });
            }
        }
        else
        {
            sequence.AppendInterval(idleDuration);

            if (rewindDuration > 0)
            {
                if (!isFadeOut)
                    sequence.Append(image.DOFade(1, rewindDuration));
                else
                    sequence.Append(image.DOFade(0, rewindDuration));
            }
        }

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            childText.Add(text);
        }

        foreach (Image image in GetComponentsInChildren<Image>())
        {
            childImage.Add(image);
        }

        if (isIncludeChild)
        {
            sequence.OnUpdate(() =>
            {
                for (int i = 0; i < childText.Count; i++)
                {
                    Color c = childText[i].color;
                    c.a = image.color.a;
                    childText[i].color = c;
                }

                for (int i = 0; i < childImage.Count; i++)
                {
                    Color c = childImage[i].color;
                    c.a = image.color.a;
                    childImage[i].color = c;
                }
            });
        }

        sequence.OnComplete(() =>
        {
            onFadeEvent?.Invoke();
            if (isDestory)
                Destroy(gameObject);
        });
    }

    void Sprite()
    {
        foreach (TextMeshPro text in GetComponentsInChildren<TextMeshPro>())
        {
            childTmp.Add(text);
        }

        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            childSr.Add(sprite);
        }
        
        sr = GetComponent<SpriteRenderer>();

        sequence = DOTween.Sequence().Pause().SetAutoKill(false);

        if (isFadeOut)
            sequence.Append(sr.DOFade(1, duration));
        else
            sequence.Append(sr.DOFade(0, duration));
        
        if (isRewind)
        {
            rewindSequence= DOTween.Sequence().Pause().SetAutoKill(false);
            
            if (!isFadeOut)
                rewindSequence.Append(sr.DOFade(1, rewindDuration));
            else
                rewindSequence.Append(sr.DOFade(0, rewindDuration));
            
            if (isIncludeChild)
            {
                rewindSequence.OnUpdate(() =>
                {
                    for (int i = 0; i < childTmp.Count; i++)
                    {
                        Color c = childTmp[i].color;
                        c.a = sr.color.a;
                        childTmp[i].color = c;
                    }

                    for (int i = 0; i < childSr.Count; i++)
                    {
                        Color c = childSr[i].color;
                        c.a = sr.color.a;
                        childSr[i].color = c;
                    }
                });
            }
            
            rewindSequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            sequence.AppendInterval(idleDuration);

            if (rewindDuration > 0)
            {
                if (!isFadeOut)
                    sequence.Append(sr.DOFade(1, rewindDuration));
                else
                    sequence.Append(sr.DOFade(0, rewindDuration));
            }
        }

        if (isIncludeChild)
        {
            sequence.OnUpdate(() =>
            {
                for (int i = 0; i < childTmp.Count; i++)
                {
                    Color c = childTmp[i].color;
                    c.a = sr.color.a;
                    childTmp[i].color = c;
                }

                for (int i = 0; i < childSr.Count; i++)
                {
                    Color c = childSr[i].color;
                    c.a = sr.color.a;
                    childSr[i].color = c;
                }
            });
        }

        sequence.OnComplete(() =>
        {
            onFadeEvent?.Invoke();
            if (isDestory)
                Destroy(gameObject);
        });
    }

    public void StartFade()
    {
        if (isImage)
            Image();
        else
            Sprite();
    }

    public void Rewind()
    {
        rewindSequence.Restart();
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
