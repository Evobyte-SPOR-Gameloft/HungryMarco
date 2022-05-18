using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private Text whoFought;
    [SerializeField] private Text timeAndCount;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").name == "Player01(Clone)") whoFought.text = "Marco fought valiantly but it wasn't enough";
        else if (GameObject.FindGameObjectWithTag("Player").name == "Player02(Clone)") whoFought.text = "Poco fought valiantly but it wasn't enough";
        else whoFought.text = "Your bug fought valiantly but it wasn't enough";

        timeAndCount.text = $"He has eaten a total of {NewPlayer.instance.killCount} bugs When the timer hit: {TimerController.instance.timePlayingStr}";
    }

}//class
