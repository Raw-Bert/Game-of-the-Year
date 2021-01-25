using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{

    int totalScore = 0;
    public TextMeshProUGUI scoreTxt;

    // Update is called once per frame
    public void countScore(int score)
    {
        totalScore += score;
        scoreTxt.text = ("Score: "  + totalScore);
    }
}
