using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bonusScoreText;
    [SerializeField] private GameObject hearthPrefab;
    [SerializeField] private Transform hearthContainers;
    [SerializeField] private Image blackScreen;

    private List<GameObject> _heartsCreatedList = new List<GameObject>();
    private Dictionary<int, Action<float>> _uiEventDeployer;

    public Action FadeInScreenEnd;
    public Action FadeOutScreenEnd;

    private void Awake()
    {
        _uiEventDeployer = new Dictionary<int, Action<float>>()
        {
           { (int)UICommands.CHANGE_SCORE, ChangeScore },
           { (int)UICommands.CHANGE_BONUS_SCORE, ChangeBonusScore },
           { (int)UICommands.SET_PLAYERS_HEARTS, SetPlayerHearts },
           { (int)UICommands.FADE_SCREEN_IN, BlackScreenFadeIn },
           { (int)UICommands.FADE_SCREEN_OUT, BlackScreenFadeOut },
        };

        BlackScreenFadeOut(2);
    }

    private void ChangeScore(float score)
    {
        scoreText.text = score.ToString();
    }

    private void ChangeBonusScore(float score)
    {
        bonusScoreText.text = score.ToString();
    }

    private void SetPlayerHearts(float value)
    {
        foreach (var item in _heartsCreatedList)
             Destroy(item);

        _heartsCreatedList.Clear();

        for (int i = 0; i < value; i++)
        {
            var newHeart = Instantiate(hearthPrefab);
            newHeart.transform.parent = hearthContainers.transform;
            _heartsCreatedList.Add(newHeart);
        }
    }

    private void BlackScreenFadeIn(float time)
    {
        var screenColor = blackScreen.color;
        screenColor.a = 0;
        blackScreen.color = screenColor;
        StartCoroutine(FadeScreen(time,1));
    }

    private void BlackScreenFadeOut(float time)
    {
        var screenColor = blackScreen.color;
        screenColor.a = 1;
        blackScreen.color = screenColor;
        StartCoroutine(FadeScreen(time, -1));
    }

    public void UIEventHandler(UIParameters parameters)
    {
        if (_uiEventDeployer.TryGetValue((int)parameters.Command, out var actionToPerform))
        {
            actionToPerform.Invoke(parameters.Value);
        }
    }


    IEnumerator FadeScreen(float time, int multiplier)
    {
        var timer = time;

        while(timer > 0)
        {
            timer -= Time.deltaTime / time;
            var screenColor = blackScreen.color;
            screenColor.a += Time.deltaTime / (time * multiplier);
            blackScreen.color = screenColor;
            yield return null;
        }
        if (multiplier > 0)
        {
            FadeInScreenEnd?.Invoke();
            yield break;
        }
        FadeOutScreenEnd?.Invoke();
    }
}
