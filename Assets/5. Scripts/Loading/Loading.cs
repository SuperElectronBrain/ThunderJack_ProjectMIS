using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Spine.Unity;
using DG.Tweening;

public class Loading : MonoBehaviour
{
    static string sceneName;
    [SerializeField]
    GameObject openImage;
    [SerializeField]
    GameObject closeImage;
    [SerializeField]
    Sprite openRhombus;
    [SerializeField]
    Sprite closeRhombus;
    [SerializeField]
    SkeletonGraphic skGraphic;
    [SerializeField]
    GameObject loadingBar;
    [SerializeField]
    Image fadeOutImage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());

        if (GameManager.Instance.isWork)
            OpenShop();
        else
            CloseShop();        
    }

    public static void LoadScene(string nextSceneName)
    {
        sceneName = nextSceneName;
        SceneManager.LoadScene("loading");
    }

    void OpenShop()
    {
        openImage.SetActive(true);
        skGraphic.startingAnimation = "Open";

        foreach(Image image in loadingBar.GetComponentsInChildren<Image>())
        {
            image.sprite = openRhombus;
        }
    }

    void CloseShop()
    {
        closeImage.SetActive(true);
        skGraphic.startingAnimation = "Close";

        foreach (Image image in loadingBar.GetComponentsInChildren<Image>())
        {
            image.sprite = closeRhombus;
        }
    }
    
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        bool isDone = false;
        bool canBreak = false;

        skGraphic.AnimationState.Complete += (Spine.TrackEntry te) =>
        {
            isDone = true;
        };

        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                if (isDone)
                {
                    fadeOutImage.gameObject.SetActive(true);
/*                    Sequence foSequence = DOTween.Sequence().SetAutoKill(false).Pause()
                        .Append(fadeOutImage.DOFade(1f, 1f))
                        .OnComplete(() =>
                        {
                            fadeOutImage.color = new Color(0, 0, 0, 255f);
                            canBreak = true;
                        })
                        .Play();*/
                    //if (canBreak)
                        break;
                }                
            }            
            yield return null;
        }
        //fadeOutImage.color = new Color(0, 0, 0, 255f);
        yield return new WaitForSeconds(1f);
        op.allowSceneActivation = true;
    }
}