using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreCount : MonoBehaviour
{
    public static ScoreCount instance { get; private set; }
    private int score = 0;

    [SerializeField] private Text scoreCounterText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
    }
    private void Update()
    {
        scoreCounterText.text = score.ToString();
    }

    public void IncrementScore()
    {
        score++;
    }
}
