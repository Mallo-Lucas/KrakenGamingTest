using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform winArea;
    [SerializeField] private LayerMask playerLayer;

    private Collider[] winAreaObjects = new Collider[1];

    private void Start()
    {
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(RespawnPlayer);
        StartCoroutine(CastPlayerWinArea());
        playerModel.OnGetDamage += PlayerLost;
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

    private void PlayerLost(int lifes)
    {
        if (lifes > 0)
            return;
        LevelEventsHandler.Instance.ClearFadeScreenEvents();
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(OpenGameOverUi);
    }

    private void OpenGameOverUi()
    {
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.GAME_OVER,
            Value = 0
        });
    }

    private IEnumerator CastPlayerWinArea()
    {
        while (true)
        {
            var playerCount = Physics.OverlapBoxNonAlloc(winArea.transform.position, winArea.localScale / 2, winAreaObjects, winArea.rotation, playerLayer);
            if (playerCount > 0)
            {
                LevelEventsHandler.Instance.PlayerWinLevel();
                break;
            }
            yield return null;
        }
    }
}
