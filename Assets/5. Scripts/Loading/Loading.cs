using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Spine.Unity;

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

        foreach(Image image in loadingBar.GetComponentsInChildren<Image>())
        {
            image.sprite = openRhombus;
        }
    }

    void CloseShop()
    {
        closeImage.SetActive(true);

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

        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                skGraphic.AnimationState.Complete += (Spine.TrackEntry te) =>
                {
                    op.allowSceneActivation = true;
                };                
            }            
            yield return null;
        }
    }
}