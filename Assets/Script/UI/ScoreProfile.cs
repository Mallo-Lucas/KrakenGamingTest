using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreProfile : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text nameText;

    public void Initialize(string name, string score)
    {
        scoreText.text = "SCORE: " + score;
        nameText.text = name;
    }
}
