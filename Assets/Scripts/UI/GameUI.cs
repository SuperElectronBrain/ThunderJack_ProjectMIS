using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    Text watch;

    // Start is called before the first frame update
    void Start()
    {
        GameTime.Instance.timeEvent += TimeChange;
    }

    void TimeChange()
    {
        watch.text = GameTime.Instance.GetTime();
    }
}
