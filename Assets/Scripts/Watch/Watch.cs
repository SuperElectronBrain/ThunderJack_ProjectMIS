using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Watch : MonoBehaviour
{
    [SerializeField]
    GameObject hourHand;
    [SerializeField]
    GameObject minuteHand;

    [SerializeField]
    int a;

    // Start is called before the first frame update
    void Start()
    {
        GameTime.Instance.timeEvent += Clock;

        Clock();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            var bin = a & 1;
            Debug.Log(a + " = " + Convert.ToString(a, 2) + " : " + bin);
            //Debug.Log(Convert.ToString(a << 1, 2));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            bool isDialog = true;
            int action = 0;
            int dialog = 0;

            while(a != 0)
            {
                var bin = a & 1;
                if(bin == 0)
                {
                    if(isDialog)
                        dialog++;
                    else
                        action++;
                }
                else
                    isDialog = false;
                a >>= 1;

                Debug.Log(action + " : " + dialog);
            }
        }
    }

    void Clock()
    {
        StopAllCoroutines();
        Debug.Log("asdfasdf");
        //minuteHand.transform.rotation = Quaternion.Euler(0, 0, 6 * GameTime.Instance.GetMinute());
        //hourHand.transform.rotation = Quaternion.Euler(0, 0, 30 * GameTime.Instance.GetHour());
        StartCoroutine(ClockHandMove(minuteHand, 6 * (GameTime.Instance.GetMinute() + 10), GameTime.Instance.GetGameSpeed()));
        //StartCoroutine(ClockHandMove(hourHand, 30 * GameTime.Instance.GetHour(), 30));
    }

    IEnumerator ClockHandMove(GameObject hand, float nextTimeAngle, float minuteSecond)
    {
        float t = 0;

        var currentTimeAngle = hand.transform.rotation.eulerAngles.z;
        Debug.Log(currentTimeAngle);
        while (t <= minuteSecond)
        {
            
            var newAngle = Mathf.Lerp(currentTimeAngle, nextTimeAngle, t / minuteSecond);
            hand.transform.rotation = Quaternion.Euler(0,0, newAngle);

            t += Time.deltaTime;
            yield return null;
        }
    }
}
