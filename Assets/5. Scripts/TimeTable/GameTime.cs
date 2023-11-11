using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTime : MonoBehaviour
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

    [SerializeField]
    bool isTimeStop;

    [SerializeField]
    int closeTime;

    public bool IsNextDay;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Subscribe(EventType.Work, StartWork);
        EventManager.Publish(EventType.Minute);
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    public void Timer()
    {
        if (isTimeStop)
            return;

        timer += Time.deltaTime * gameSpeed;

        if (timer >= gameTime2RealTime * 60)
        {
            timer = 0;
            minute += 10;

            if (minute >= 60)
            {
                minute = 0;
                hour++;

                if (hour == 9)
                {
                    EventManager.Publish(EventType.WorkTime);
                    isTimeStop = true;
                    timer = 0;
                    return;
                }
                else if (hour >= 24)
                {
                    hour = 0;
                    day++;
                }

                if (closeTime == hour)
                    EventManager.Publish(EventType.CloseShop);

                EventManager.Publish(EventType.hour);
            }

            EventManager.Publish(EventType.Minute);
        }
    }

    void StartWork()
    {
        hour = 9;
        minute = 0;
        timer = Random.Range(45f, 55f);
    }

    public void EndWork()
    {
        hour = 18;
        minute = 0;
        timer = 0;
    }

    public void TimeStop(bool isTimeStop)
    {
        this.isTimeStop = isTimeStop;
    }

    public void NewDay()
    {
        hour = 6;
        minute = 0;
        timer = 0;
        IsNextDay = false;
        EventManager.Publish(EventType.Day);
    }

    public string GetTime()
    {
        return hour + ":" + minute.ToString("D2");
    }

    public int GetHour()
    {
        return hour;
    }

    public int GetMinute()
    {
        return minute;
    }

    public int GetDay()
    {
        return day;
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

    public float GetTimer()
    {
        return timer;
    }
}
