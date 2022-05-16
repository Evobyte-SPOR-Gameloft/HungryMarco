using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public static ScoreController instance;

    public Text scoreCounter;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreCounter.text = "x00000";
    }

    private void FixedUpdate()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreCounter.text = $"x{NewPlayer.instance.killCount}";
    }

}//clas
