using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventsHandler : MonoBehaviour
{
    [SerializeField] private InGameUIManager gameUIManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private ObstaclesSpawnSystem obstaclesSpawnSystem;

    public static LevelEventsHandler Instance;

    public Action PlayerWin;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayerWinLevel()
    {
        PlayerWin?.Invoke();
    }

    public void ClearFadeScreenEvents()
    {
        gameUIManager.FadeInScreenEnd = null;
        gameUIManager.FadeOutScreenEnd = null;
    }

    public void SubscribeToFadeInEvent(Action action)
    {
        gameUIManager.FadeInScreenEnd += action;
    }

    public void SubscribeToFadeOutEvent(Action action)
    {
        gameUIManager.FadeOutScreenEnd += action;
    }

    public void SubscribeToPauseMenu(I_Pause pause)
    {
        pauseMenu.AddI_PauseObject(pause);
    }

    public Action GetScoreManagerBarrelEvent()
    {
        return scoreManager.BarrelJumped;
    }

    public Action GetScoreManagerShurikenEvent()
    {
        return scoreManager.ShurikenEvade;
    }
}
