using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bonusScoreText;

    private Dictionary<int, Action<float>> _uiEventDeployer;

    private void Awake()
    {
        _uiEventDeployer = new Dictionary<int, Action<float>>()
        {
           { (int)UICommands.CHANGE_SCORE, ChangeScore },
           { (int)UICommands.CHANGE_BONUS_SCORE, ChangeBonusScore },
        };
    }

    private void ChangeScore(float score)
    {
        scoreText.text = score.ToString();
    }

    private void ChangeBonusScore(float score)
    {
        bonusScoreText.text = score.ToString();
    }

    public void UIEventHandler(UIParameters parameters)
    {
        if (_uiEventDeployer.TryGetValue((int)parameters.Command, out var actionToPerform))
        {
            actionToPerform.Invoke(parameters.Value);
        }
    }
}
