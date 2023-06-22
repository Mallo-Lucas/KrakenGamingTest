using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerOnY: MonoBehaviour
{
    [SerializeField] private PlayerModel player;

    private void Start()
    {
        player.OnGetDamage += StopFollowPlayer;
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(FollowPlayer);
        StartCoroutine(CheckLevel());
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        var newTargetPosition = Vector3.zero;
        var playerTransform = player.transform;
        var transform1 = transform;
        while (true) 
        {
            newTargetPosition = transform1.position;
            newTargetPosition.y = playerTransform.position.y;
            transform.position = newTargetPosition;
            yield return null;
        }
    }

    private void FollowPlayer()
    {
       StartCoroutine(FollowPlayerCoroutine());
    }

    private void StopFollowPlayer(int value)
    {
        StopAllCoroutines();
    }

    private IEnumerator CheckLevel()
    {
        float timer = 4;
        float lerpTimer = 0;
        var transform1 = transform;
        var startPosition = transform1.position;
        var newTargetPosition = transform1.position;
        newTargetPosition.y = player.transform.position.y;

        while (timer > 0)
        {           
            transform1.position = Vector3.Lerp(startPosition, newTargetPosition,lerpTimer/4);
            lerpTimer += Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        FollowPlayer();
    }
}
