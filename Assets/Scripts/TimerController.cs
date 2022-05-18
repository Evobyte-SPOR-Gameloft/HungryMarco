using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    public static TimerController instance;

    [SerializeField] private Text timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;

    public string timePlayingStr;

    private void Awake()
    {

        instance = this;

        instance.timeCounter.text = "00:00:00";
        instance.timerGoing = false;

    }

    public void BeginTimer()
    {
        instance.timerGoing = true;
        instance.elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        instance.timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (instance.timerGoing)
        {

            instance.elapsedTime += 1;

            instance.timePlaying = TimeSpan.FromSeconds(instance.elapsedTime);

            timePlayingStr = instance.timePlaying.ToString("hh':'mm':'ss");

            instance.timeCounter.text = timePlayingStr;

            //Debug.Log(timePlayingStr);

            yield return new WaitForSeconds(1);
        }
    }

}//clas
