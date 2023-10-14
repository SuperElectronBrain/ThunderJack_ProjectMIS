using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.InitScene(SceneType.OutSide);        
    }
}
