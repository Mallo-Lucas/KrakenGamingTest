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
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject abilityUi;
    [SerializeField] private TMP_Text ablityAmmountText;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image blackScreen;

    private List<GameObject> _heartsCreatedList = new List<GameObject>();
    private Dictionary<int, Action<float>> _uiEventDeployerNumbers;
    private Dictionary<int, Action<Sprite>> _uiEventDeployerSprites;
    private Dictionary<int, Action<bool>> _uiEventDeployerStates;

    public Action FadeInScreenEnd;
    public Action FadeOutScreenEnd;

    private void Awake()
    {
        quitButton.onClick.AddListener(QuitGame);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        _uiEventDeployerNumbers = new Dictionary<int, Action<float>>()
        {
           { (int)UICommands.CHANGE_SCORE, ChangeScore },
           { (int)UICommands.CHANGE_BONUS_SCORE, ChangeBonusScore },
           { (int)UICommands.SET_PLAYERS_HEARTS, SetPlayerHearts },
           { (int)UICommands.FADE_SCREEN_IN, BlackScreenFadeIn },
           { (int)UICommands.FADE_SCREEN_OUT, BlackScreenFadeOut },
           { (int)UICommands.GAME_OVER, OpenGameOverMenu },
           { (int)UICommands.CHANEGE_ABILITY_AMMOUNT, ChangeAbilityAmmount },
        };

        _uiEventDeployerStates = new Dictionary<int, Action<bool>>()
        {
           { (int)UICommands.ACTIVATE_ABILITY_UI, EnableAbilityUI },
        };

        _uiEventDeployerSprites= new Dictionary<int, Action<Sprite>>()
        {
           { (int)UICommands.CHANGE_ABILITY_ICON, ChangeAbilityIcon },
        };

        BlackScreenFadeOut(2);
    }

    private void ChangeAbilityAmmount(float ammount)
    {
        ablityAmmountText.text = ammount.ToString();
    }

    private void ChangeAbilityIcon(Sprite sprite)
    {
        abilityIcon.sprite = sprite;    
    }

    private void EnableAbilityUI(bool state)
    {
        abilityUi.SetActive(state);
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

    private void OpenGameOverMenu(float value)
    {
        gameOverMenu.SetActive(true);
    }

    private void BackToMainMenu()
    {
        ChangeSceneManager.Instance.GoToMainMenu();
    }

    private void QuitGame()
    {
        Application.Quit();
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

    public void UIEventHandlerNumbers(UIParameters parameters)
    {
        if (_uiEventDeployerNumbers.TryGetValue((int)parameters.Command, out var actionToPerform))
        {
            actionToPerform.Invoke(parameters.Value);
        }
    }

    public void UIEventHandlerState(UIParameters parameters)
    {
        if (_uiEventDeployerStates.TryGetValue((int)parameters.Command, out var actionToPerform))
        {
            actionToPerform.Invoke(parameters.State);
        }
    }

    public void UIEventHandlerSprites(UIParameters parameters)
    {
        if (_uiEventDeployerSprites.TryGetValue((int)parameters.Command, out var actionToPerform))
        {
            actionToPerform.Invoke(parameters.Sprite);
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
