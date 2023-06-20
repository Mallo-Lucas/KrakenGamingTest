using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private InGameUIManager gameUIManager;
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private Transform spawnPosition;

    private void Awake()
    {
        gameUIManager.FadeInScreenEnd += RespawnPlayer;
    }

    private void RespawnPlayer()
    {
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.FADE_SCREEN_OUT,
            Value = 2
        });
        playerModel.RespawnPlayer(spawnPosition);
    }
}
