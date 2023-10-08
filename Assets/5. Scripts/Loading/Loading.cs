using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine;

public class Loading : MonoBehaviour
{
    static string sceneName;

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
        SceneManager.LoadScene("LoadingScene");
    }

    void OpenShop()
    {

    }

    void CloseShop()
    {

    }
    
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        
        while(!op.isDone)
        {
            if(op.progress < 0.9f)
            {
                Debug.Log("로딩중... " + op.progress + "%");
            }
            else if(op.progress >= 0.9f)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    Debug.Log("로딩완료");
                    op.allowSceneActivation = true;
                    yield break;
                }
            }

            yield return null;
        }
    }
}