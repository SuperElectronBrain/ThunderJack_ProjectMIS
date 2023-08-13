using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : Singleton<GameTime>
{
    [SerializeField]
    int day = 1;
    [SerializeField]
    int hour;
    [SerializeField]
    int minute;

    [SerializeField]
    int gameTime2RealTime;

    [SerializeField]
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isTimePasses)
        {
            timer += Time.deltaTime;

            if(timer >= gameTime2RealTime * 60)
            {
                timer = 0;
                minute += 10;

                if(minute >= 60)
                {
                    minute = 0;
                    hour++;

                    if(hour >= 24)
                    {
                        hour = 0;
                        day++;
                    }
                }
            }
        }
    }
}
