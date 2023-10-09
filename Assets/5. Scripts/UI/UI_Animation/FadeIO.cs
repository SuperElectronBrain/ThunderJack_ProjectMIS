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
    [SerializeField]
    bool isIncludeChild;

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

        foreach(TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            childText.Add(text);
        }

        foreach(Image image in GetComponentsInChildren<Image>())
        {
            childImage.Add(image);
        }

        if(isIncludeChild)
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
        });        
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
