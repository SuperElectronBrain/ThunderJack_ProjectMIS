using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    float gameSpeed;

    [SerializeField]
    float timer = 0;

    public UnityAction timeEvent;
    public UnityAction dayEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    public void Timer()
    {
        if (GameManager.Instance.isTimePasses)
        {
            timer += Time.deltaTime * gameSpeed;

            if (timer >= gameTime2RealTime * 60)
            {
                timer = 0;
                minute += 10;

                if (minute >= 60)
                {
                    minute = 0;
                    hour++;

                    if (hour >= 24)
                    {
                        hour = 0;
                        day++;

                        dayEvent?.Invoke();
                    }
                }

                timeEvent?.Invoke();
            }
        }
    }

    public string GetTime()
    {
        return hour + ":" + minute;
    }

    public int GetHour()
    {
        return hour;
    }

    public int GetMinute()
    {
        return minute;
    }

    public float GetGameSpeed()
    {
        return (gameTime2RealTime * 60) / gameSpeed;
    }

    public int GetTimeIdx(int h, int m)
    {
        return h * 6 + m / 10;
    }

    public int GetTimeIdx()
    {
        return hour * 6 + minute / 10; 
    }
}
