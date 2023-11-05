using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InitScene : MonoBehaviour
{
    [SerializeField]
    SceneType sceneType;
    public UnityEvent onInitEventToOutSide;
    public UnityEvent onInitEventToInSide;
    public UnityEvent onInitEventToBusiness;

    private void Start()
    {
        GameManager.Instance.InitScene(sceneType);
        switch (GameManager.Instance.prevScene)
        {
            case SceneType.OutSide:
                onInitEventToOutSide?.Invoke();
                break;
            case SceneType.InSide:
                onInitEventToInSide?.Invoke();
                break;
            case SceneType.Bussiness:
                onInitEventToBusiness?.Invoke();
                break;
        }
    }
}