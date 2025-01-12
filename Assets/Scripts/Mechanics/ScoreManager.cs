﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;    
    public TextMeshProUGUI scoreText;
  

    int score = 0;


    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
      
        scoreText.text = score.ToString() ;
     
       

    }
    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() ;
      /*  if (highscore < score)
        {

            PlayerPrefs.SetInt("highscore", score); // Salvează highscore-ul nou

        }*/
    }

}
