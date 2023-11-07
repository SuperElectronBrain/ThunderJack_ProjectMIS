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
    [SerializeField]
    bool isDestory;

    [SerializeField]
    List<Image> childImage = new();
    [SerializeField]
    List<TextMeshProUGUI> childText = new();

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

        sequence.AppendInterval(idleDuration);

        if (rewindDuration > 0)
        {
            if (!isFadeOut)
                sequence.Append(image.DOFade(1, rewindDuration));
            else
                sequence.Append(image.DOFade(0, rewindDuration));
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
        sr = GetComponent<SpriteRenderer>();

        sequence = DOTween.Sequence().Pause().SetAutoKill(false);

        if (isFadeOut)
            sequence.Append(sr.DOFade(1, duration));
        else
            sequence.Append(sr.DOFade(0, duration));

        sequence.AppendInterval(idleDuration);

        if (rewindDuration > 0)
        {
            if (!isFadeOut)
                sequence.Append(sr.DOFade(1, rewindDuration));
            else
                sequence.Append(sr.DOFade(0, rewindDuration));
        }

/*        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            childText.Add(text);
        }*/

/*        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            childImage.Add(sprite);
        }*/

/*        if (isIncludeChild)
        {
            sequence.OnUpdate(() =>
            {
                for (int i = 0; i < childText.Count; i++)
                {
                    Color c = childText[i].color;
                    c.a = sr.color.a;
                    childText[i].color = c;
                }

                for (int i = 0; i < childImage.Count; i++)
                {
                    Color c = childImage[i].color;
                    c.a = sr.color.a;
                    childImage[i].color = c;
                }
            });
        }*/

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

    public void ChangeText()
    {

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
